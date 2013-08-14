using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FnR.Databases;
using FnR.Helpers;
using FnR.Models;
using FnR.Repositories;
using FnR.WebApi.ViewModels;
using FnR.WebApi.eWay.Token.Test;

namespace FnR.WebApi.Controllers.Services
{
    public class FelixAndRoverController : ApiController
    {
        readonly FnRDbContext _db = new FnRDbContext();
        private readonly SubscriptionRepository _subRepo = new SubscriptionRepository(new FnRDbContext());
        private readonly UserRepository _userRepo = new UserRepository(new FnRDbContext());
        private readonly PetRepository _petRepo = new PetRepository(new FnRDbContext());
        private readonly ProductRepository _productRepo = new ProductRepository(new FnRDbContext());

        [HttpGet]
        [Queryable]
        public IQueryable<User> GetAllUsers()
        {
            return _db.Users
                .Include(r => r.Subscriptions)
                .Include("Subscriptions.Product")
                .Include("Subscriptions.Vet")
                .Include("Subscriptions.Vet.AvailableProducts")
                .Include("Subscriptions.Pet")
                .Include("Subscriptions.Pet.Breed")
                .Include("Subscriptions.Pet.Breed.PetType")
                .Include("Pets")
                .Include("Pets.Breed")
                .Include("Pets.Breed.PetType")
                .Include(r => r.Vet)
                .Include(r => r.Pets);
        }

        [HttpGet]
        [Queryable]
        public IQueryable<Product> GetAllProducts()
        {
            return _db.Products
                .Include(r => r.PetType)
                .Include("PetType.Breeds");
        }

        [HttpPost]
        //public User RegisterUser(User user)
        public User RegisterUser(VMRegisterUser registerUser)
        {
            if (_db.Users.Any(r => r.Username == registerUser.Username) || _db.Users.Any(r => r.Email == registerUser.Email))
                return null;

            var user = new User()
                           {
                               Username = registerUser.Username,
                               Password = registerUser.Password,
                               FirstName = registerUser.FirstName,
                               LastName = registerUser.LastName,
                               Email = registerUser.Email,
                               PhoneNumber = registerUser.PhoneNumber,
                               Address = registerUser.Address,
                               VetId = registerUser.VetId
                           };

            //var client = new managedCreditCardPaymentTestSoapClient();

            //user.TokenCustomerID = client.CreateCustomer(GetEwayHeader(), "Mr.", registerUser.FirstName, registerUser.LastName, "", "", "", "",
            //                      "", "au", registerUser.Email, "", "", "", "", "", "", "", registerUser.CCNumber,
            //                      registerUser.NameOnCard, registerUser.CCExpMonth, registerUser.CCExpYear);

            var rebillClient = new eWay.Rebill.Test.manageRebill_testSoapClient();
            var ewayHeader = GetRebillEwayHeader();
            user.RebillCustomerID =
                rebillClient.CreateRebillCustomer(ref ewayHeader, "Mr.", registerUser.FirstName, registerUser.LastName, "",
                                            "", "", "", "", "au", registerUser.Email, "", "", "", "", "", "", "").RebillCustomerID;
            _db.Users.Add(user);
            _db.SaveChanges();

            if (user.Email != null)
            {
                try
                {
                    EmailHelper.SendEmail(user.Email, "FelixAndRover Registration",
                                          "<h1>Registration Successful</h1><p>You have successfully registered to Felix and Rover App..</p>");
                }
                catch
                {
                }
            }
            return user;
        }

        [HttpGet]
        [Queryable]
        public IQueryable<Breed> GetAllBreeds()
        {
            return _db.Breeds
                .Include(r => r.PetType);
        }

        [HttpGet]
        [Queryable]
        public IQueryable<PetType> GetAllPetTypes()
        {
            return _db.PetTypes
                .Include(r => r.Breeds);
        }

        [HttpPost]
        public void SavePet(VMSavePet pet)
        {
            var newPet = new Pet()
                             {
                                 Name = pet.Name,
                                 BreedId = pet.PetBreedId,
                                 Birthday = pet.Birthday,
                                 UserId = pet.UserId,
                                 Weight = pet.Weight
                             };
            _db.Pets.Add(newPet);
            _db.SaveChanges();
        }

        [HttpPost]
        public void ForgotPassword(VMForgotPassword account)
        {
            EmailHelper.SendEmail(account.email, "Felix and Rover Account Password",
                                  "<h1>Account Password</h1><p>Your account password for Felix and Rover app is " + account.password + "</p>");
        }

        [HttpPost]
        public Subscription SubscribePet(VMSubscription subscription)
        {
            var user = _db.Users.SingleOrDefault(r => r.Id == subscription.UserId);
            var product = _db.Products.SingleOrDefault(r => r.Id == subscription.ProductId);
            var price = ((double) product.Price*100).ToString(CultureInfo.InvariantCulture);

            var day = DateTime.Now.Day;
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;

            var rebillIntDate = string.Format("{0}/{1}/{2}", (day + 2).ToString("00"), month.ToString("00"), year);
            var rebillStartDate = string.Format("{0}/{1}/{2}", 1.ToString("00"), (month + 1).ToString("00"), year);
            var rebillEndDate = string.Format("{0}/{1}/{2}", day.ToString("00"), month.ToString("00"), year + 2);

            var clientRecurring = new eWay.Rebill.Test.manageRebill_testSoapClient();
            var ewayHeader = GetRebillEwayHeader();
            var status =
                clientRecurring.CreateRebillEvent(ref ewayHeader, user.RebillCustomerID, "Felix and Rover Subscription", "Product Subscription: " + product.DisplayName, subscription.NameOnCard,
                                                  subscription.CCNumber, subscription.CCExpMonth, subscription.CCExpYear,
                                                  price, rebillIntDate, price, rebillStartDate, "1", "3",
                                                  rebillEndDate);

            if (status.Result.ToLower() == "success")
            {
                var sub = new Subscription()
                              {
                                  UserId = subscription.UserId,
                                  PetId = subscription.PetId,
                                  ProductId = subscription.ProductId,
                                  VetId = subscription.VetId,
                                  RebillID = status.RebillID
                              };
                var newSub = _subRepo.CreateSubscription(sub);

                //var client = new managedCreditCardPaymentTestSoapClient();
                //var success = client.ProcessPayment(GetEwayHeader(), Convert.ToInt64(user.TokenCustomerID), ((int) product.Price * 100),
                //                                    "Initial Payment for Subscription", "Subscribed to Product: " + product.Name).ewayTrxnStatus;
                //string RebillCustomerID, string RebillInvRef, string RebillInvDes, string RebillCCName, string RebillCCNumber, 
                //string RebillCCExpMonth, string RebillCCExpYear, string RebillInitAmt, string RebillInitDate, string RebillRecurAmt, 
                //string RebillStartDate, string RebillInterval, string RebillIntervalType, string RebillEndDate
                //var successRecurring = clientRecurring.CreateRebillEvent(ref ewayHeader, user.RebillCustomerID, )

                try
                {
                    var body = "<h1>Subscription Successful</h1>";
                    body += "<p>Thank you for subscribing to felix and rover</p>";
                    body += "<p>The pet owner " + newSub.User.FullName + " and pet named " + newSub.Pet.Name +
                            " will now receive the following product monthly:</p>";
                    body += "<p>Product name: " + newSub.Product.DisplayName + "</p>";
                    body += "<p>Amount: $" + newSub.Product.Price + "</p>";
                    body += "<p>Thank you,</p>";
                    body += "<p>Felix and Rover Team ^.^</p>";
                    EmailHelper.SendEmail(newSub.User.Email, "Felix and Rover (Product Subscription)", body);
                }
                catch (Exception)
                {
                }
                return newSub;
            }

            return null;
        }

        [HttpGet]
        public Vet GetVet(int id)
        {
            return _db.Vets.Include(r => r.AvailableProducts).SingleOrDefault(r => r.Id == id);
        }

        [HttpGet]
        public Pet GetPet(int petId)
        {
            return _db.Pets.Include(r => r.Conditions).SingleOrDefault(r => r.Id == petId);
        }

        [HttpGet]
        public IQueryable<Subscription> GetUserSubscriptions(string username)
        {
            return
                _db.Subscriptions.Include(r => r.Product).Include(r => r.User).Include(r => r.Vet).Include(r => r.Pet).Where(r => r.User.Username == username && !r.Sent);
        }

        [HttpGet]
        public Vet GetVetByUsername(string username)
        {
            return _db.Vets.SingleOrDefault(r => r.Username == username);
        }

        [HttpGet]
        public IQueryable<Vet> GetAllVets()
        {
            return _db.Vets;
        }

        [HttpGet]
        public List<Schedule> GetUserSchedules(string username)
        {
            return _db.Users.Include("Schedules.Doctor").Include("Schedules.Events").Include(r => r.Schedules).SingleOrDefault(r => r.Username == username).Schedules.ToList();
        }

        [HttpGet]
        public string Unsubscribe(int id)
        {
            var subscription = _db.Subscriptions.SingleOrDefault(r => r.Id == id);
            var user = _db.Users.SingleOrDefault(r => r.Id == subscription.UserId);

            var clientRecurring = new eWay.Rebill.Test.manageRebill_testSoapClient();
            var ewayHeader = GetRebillEwayHeader();

            var result = clientRecurring.DeleteRebillEvent(ref ewayHeader, user.RebillCustomerID, subscription.RebillID).Result;

            if (result.ToLower() == "success")
            {
                _db.Subscriptions.Remove(_db.Subscriptions.SingleOrDefault(r => r.Id == id));
                _db.SaveChanges();

                try
                {
                    var body = "<h1>Successfully Unsubscribed</h1>";
                    body += "<p>We hope you enjoyed our services.</p>";
                    body += "<p>The pet owner " + subscription.User.FullName + " and pet named " + subscription.Pet.Name + " will now stop receiving the following product monthly:</p>";
                    body += "<p>Product name: " + subscription.Product.DisplayName + "</p>";
                    body += "<p>Amount: $" + subscription.Product.Price + "</p>";
                    body += "<p>Thank you,</p>";
                    body += "<p>Felix and Rover Team ^.^</p>";
                    EmailHelper.SendEmail(subscription.User.Email, "Felix and Rover (Unsubscribed)", body);
                }
                catch (Exception)
                {
                }

                return "success";
            }
            return "fail";
        }

        public IQueryable<Breed> GetPetTypeBreeds(int petTypeId)
        {
            return _db.Breeds.Include(r => r.PetType).Where(r => r.PetTypeId == petTypeId);
        }

        [HttpPost]
        public string CreateTokenCustomer(VMCreateCustomer customer)
        {
            var client = new managedCreditCardPaymentTestSoapClient();
            return client.CreateCustomer(GetEwayHeader(), customer.Title, customer.FirstName, customer.LastName, "", "", "", "",
                                  "", "au", customer.Email, "", "", "", "", "", "", "", customer.CCNumber,
                                  customer.CCNameOnCard, customer.CCExpiryMonth, customer.CCExpiryYear);
        }

        [HttpPost]
        public string BillCustomer(VMBillCustomer customer)
        {
            var client = new managedCreditCardPaymentTestSoapClient();
            return client.ProcessPayment(GetEwayHeader(), customer.CustomerID, customer.Amount,
                                                customer.InvoiceReference, customer.InvoiceDescription).ewayTrxnStatus;
        }

        [HttpPost]
        public string CreateRebillCustomer(VMCreateRebillCustomer customer)
        {
            var client = new eWay.Rebill.Test.manageRebill_testSoapClient();
            var ewayHeader = GetRebillEwayHeader();
            return
                client.CreateRebillCustomer(ref ewayHeader, customer.Title, customer.FirstName, customer.LastName, "",
                                            "", "", "", "", "au", customer.Email, "", "", "", "", "", "", "").RebillCustomerID;
        }

        [HttpPost]
        public string BillRecurring(VMRebillCustomer customer)
        {
            var client = new eWay.Rebill.Test.manageRebill_testSoapClient();
            var ewayHeader = GetRebillEwayHeader();
            return "";//client.CreateRebillEvent(ref ewayHeader, customer.RebillCustomerID, ).Result;
        }

        private eWay.Rebill.Test.eWAYHeader GetRebillEwayHeader()
        {
            return new eWay.Rebill.Test.eWAYHeader
            {
                eWAYCustomerID =
                    ConfigurationManager.AppSettings["eWaySandBoxCustomerID"], // use eWayCustomerID for live
                Username =
                    ConfigurationManager.AppSettings["eWaySandBoxUsername"], // use eWayUsername for live
                Password =
                    ConfigurationManager.AppSettings["eWaySandBoxPassword"] // use eWayPassword for live
            };
        }


        private eWay.Token.Test.eWAYHeader GetEwayHeader()
        {
            return new eWay.Token.Test.eWAYHeader
                       {
                           eWAYCustomerID =
                               ConfigurationManager.AppSettings["eWaySandBoxCustomerID"], // use eWayCustomerID for live
                           Username =
                               ConfigurationManager.AppSettings["eWaySandBoxUsername"], // use eWayUsername for live
                           Password =
                               ConfigurationManager.AppSettings["eWaySandBoxPassword"] // use eWayPassword for live
                       };
        }

        [HttpGet]
        [Queryable]
        public IQueryable<DoctorAppointmentStatus> GetDoctorAppointmentStatuses()
        {
            return _db.DoctorAppointmentStatuses
                      .Include(r => r.Doctor)
                      .Include(r => r.Doctor.Vet);
        }

        //[HttpGet]
        //public void SendEmail()
        //{
        //    EmailHelper.SendEmail("alexander.burias@gmail.com", "test", "Test");
        //}

        //[HttpPost]
        //public bool SendEmail(string to, string subject, string body)
        //{
        //    try
        //    {
        //        EmailHelper.SendEmail(to, subject, body);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //[HttpPost]
        //public User CreateUserWithPets()
        //{
        //    return 
        //}

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}

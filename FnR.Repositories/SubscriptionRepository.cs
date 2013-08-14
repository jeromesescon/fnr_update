using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FnR.Models;

namespace FnR.Repositories
{
    public class SubscriptionRepository : Repository<Subscription>
    {
        private readonly DbContext _context;
        public SubscriptionRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public Subscription CreateSubscription(Subscription subscription)
        {
            subscription.DateSubscribed = DateTime.Now;
            var deliveryMonth = DateTime.Now.Month + 1;
            var deliveryYear = DateTime.Now.Year;
            const int deliveryDay = 1;
            var deliveryDate = new DateTime(deliveryYear, deliveryMonth, deliveryDay);
            while(deliveryDate.DayOfWeek != DayOfWeek.Monday)
            {
                deliveryDate = deliveryDate.AddDays(1);
            }
            subscription.NextDeliveryDate = deliveryDate;
            _context.Set<Subscription>().Add(subscription);
            _context.SaveChanges();
            return
                _context.Set<Subscription>().Include(r => r.User).Include(r => r.Pet).Include(r => r.Product).Include(
                    r => r.Vet).SingleOrDefault(r => r.Id == subscription.Id);
        }
    }
}

using System.Data.Entity;
using FnR.Models;

namespace FnR.Databases
{
    /// <summary>
    /// Database context, direct mapping of object classes into database table.
    /// </summary>
    public class FnRDbContext : DbContext
    {
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetType> PetTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vet> Vets { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Event> Events { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<DoctorAppointmentStatus> DoctorAppointmentStatuses { get; set; }
    }
}
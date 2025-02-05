using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SinetEmployeeLeave.Models;

namespace SinetEmployeeLeave.Data
{
    

    public class LeaveManagementDbContext : IdentityDbContext<Employee>
    {
        public LeaveManagementDbContext(DbContextOptions<LeaveManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Holiday> Holidays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.Employee)
                .WithMany(e => e.LeaveRequests)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

        //    modelBuilder.Entity<Employee>()
        //.HasMany(e => e.LeaveRequests)
        //.WithOne(lr => lr.Employee)
        //.OnDelete(DeleteBehavior.NoAction);
        }
    }

}


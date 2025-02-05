using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SinetEmployeeLeave.Models
{
    

    public class Employee : IdentityUser
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Role { get; set; } // Employee, Manager, HR Admin
        public int LeaveBalance { get; set; } = 20; // Default leave balance

        public int LeaveRequest { get; set; } // Foreign key to LeaveRequest

        // Navigation Properties
        public ICollection<LeaveRequest> LeaveRequests { get; set; }
    }


}

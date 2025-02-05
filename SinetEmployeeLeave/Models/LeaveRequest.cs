using System.ComponentModel.DataAnnotations.Schema;

namespace SinetEmployeeLeave.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; } // Foreign key to Employee
        public LeaveType LeaveType { get; set; } // Enum for leave type
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsHalfDay { get; set; } // Half-day leave indicator
        public LeaveStatus Status { get; set; } // Pending, Approved, Rejected
        public string Reason { get; set; }
        public string ApprovedBy { get; set; } // Manager or HR Admin
        public DateTime RequestDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public Employee Employee { get; set; }


        
    }

    public enum LeaveType
    {
        Vacation,
        SickLeave,
        MaternityLeave,
        PaternityLeave,
        UnpaidLeave,
        Other
    }

    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }


}


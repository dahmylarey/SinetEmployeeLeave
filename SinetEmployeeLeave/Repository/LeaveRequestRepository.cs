using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SinetEmployeeLeave.Data;
using SinetEmployeeLeave.Implementation;
using SinetEmployeeLeave.Models;

namespace SinetEmployeeLeave.Repository
{
    public class LeaveRequestRepository : Repository<LeaveRequest>
    {
        private readonly LeaveManagementDbContext _context;

        public LeaveRequestRepository(LeaveManagementDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync()
        {
            return await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Where(lr => lr.Status == LeaveStatus.Pending)
                .ToListAsync();
        }


    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SinetEmployeeLeave.Models;
using SinetEmployeeLeave.Repository;
using SinetEmployeeLeave.Service;

namespace SinetEmployeeLeave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly LeaveRequestRepository _leaveRequestRepository;
        private readonly UserManager<Employee> _userManager;

        public LeaveRequestController(LeaveRequestRepository leaveRequestRepository, UserManager<Employee> userManager)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _userManager = userManager;
        }




        //Get Pending Leave
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<LeaveRequest>>> GetPendingRequests()
        {
            var pendingRequests = await _leaveRequestRepository.GetPendingRequestsAsync();
            return Ok(pendingRequests);
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            request.EmployeeId = user.Id;
            request.Status = LeaveStatus.Pending;

            await _leaveRequestRepository.AddAsync(request);

            // Send Email Notification
            var emailService = new EmailService();
            await emailService.SendEmailAsync(
                "manager@yourcompany.com",
                "New Leave Request Submitted",
                $"Employee {user.FullName} has requested leave from {request.StartDate} to {request.EndDate}."
            );

            return Ok(new { Message = "Leave request submitted successfully" });
        }

    }
}

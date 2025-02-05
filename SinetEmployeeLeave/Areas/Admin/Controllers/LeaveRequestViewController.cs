using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SinetEmployeeLeave.Models;
using SinetEmployeeLeave.Repository;

namespace SinetEmployeeLeave.Areas.Admin.Controllers
{


    [Area("Admin")]
    public class LeaveRequestViewController : Controller
    {
        private readonly LeaveRequestRepository _leaveRequestRepository;
        private readonly UserManager<Employee> _userManager;

        public LeaveRequestViewController(LeaveRequestRepository leaveRequestRepository, UserManager<Employee> userManager)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var leaveRequests = await _leaveRequestRepository.GetAllAsync();
            return View(leaveRequests.Where(lr => lr.EmployeeId == user.Id).ToList());
        }

        public IActionResult Apply() => View();

        [HttpPost]
        public async Task<IActionResult> Apply(LeaveRequest leaveRequest)
        {
            var user = await _userManager.GetUserAsync(User);
            leaveRequest.EmployeeId = user.Id;
            leaveRequest.Status = LeaveStatus.Pending;

            await _leaveRequestRepository.AddAsync(leaveRequest);
            return RedirectToAction("Index");
        }
    }

}

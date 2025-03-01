﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SinetEmployeeLeave.Models;
using SinetEmployeeLeave.Repository;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SinetEmployeeLeave.Service;
using Microsoft.AspNetCore.SignalR;
using SinetEmployeeLeave.Hubs;
using Microsoft.EntityFrameworkCore;

namespace SinetEmployeeLeave.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Manager,HR Admin")]
    public class AdminController : Controller
    {
        private readonly LeaveRequestRepository _leaveRequestRepository;
        private readonly UserManager<Employee> _userManager;
        private readonly IHubContext<NotificationsHub> _hubContext;

        public AdminController(LeaveRequestRepository leaveRequestRepository, UserManager<Employee> userManager, IHubContext<NotificationsHub> hubContext)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _userManager = userManager;
            _hubContext = hubContext;
        }



        //Pending leave
        public async Task<IActionResult> Index()
        {
            var pendingRequests = await _leaveRequestRepository.GetPendingRequestsAsync();
            return View(pendingRequests);
        }


        //Approve Leave
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest != null)
            {
                leaveRequest.Status = LeaveStatus.Approved;
                await _leaveRequestRepository.UpdateAsync(leaveRequest);

                // Send real-time notification to employee
                await _hubContext.Clients.User(leaveRequest.EmployeeId.ToString())
                                  .SendAsync("ReceiveNotification", "Your leave request has been approved.");

                // Send real-time Email to employee
                var emailService = new EmailService();
                await emailService.SendApprovalNotificationAsync(leaveRequest.Employee.Email, "approved", leaveRequest);
            }
            return RedirectToAction("Index");
        }

        //Reject Leave
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest != null)
            {
                leaveRequest.Status = LeaveStatus.Rejected;
                await _leaveRequestRepository.UpdateAsync(leaveRequest);

                // Send real-time notification to employee
                await _hubContext.Clients.User(leaveRequest.EmployeeId.ToString())
                                  .SendAsync("ReceiveNotification", "Your leave request has been rejected.");

                // Send real-time Email to employee
                var emailService = new EmailService();
                await emailService.SendApprovalNotificationAsync(leaveRequest.Employee.Email, "rejected", leaveRequest);
            }
            return RedirectToAction("Index");
        }


        //Add Export Function
        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var leaveRequests = await _leaveRequestRepository.GetAllAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("LeaveRequests");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "Employee";
                worksheet.Cell(currentRow, 2).Value = "Leave Type";
                worksheet.Cell(currentRow, 3).Value = "Start Date";
                worksheet.Cell(currentRow, 4).Value = "End Date";
                worksheet.Cell(currentRow, 5).Value = "Status";

                foreach (var request in leaveRequests)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = request.Employee.FullName;
                    worksheet.Cell(currentRow, 2).Value = request.LeaveType.ToString();
                    worksheet.Cell(currentRow, 3).Value = request.StartDate.ToShortDateString();
                    worksheet.Cell(currentRow, 4).Value = request.EndDate.ToShortDateString();
                    worksheet.Cell(currentRow, 5).Value = request.Status.ToString();
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LeaveRequests.xlsx");
                }
            }
        }


        //Get Statistics
        //public async Task<IActionResult> LeaveStatistics()
        //{
        //    var stats = await _leaveRequestRepository.GetLeaveStatisticsAsync();

        //    return Json(stats);
        //}



    }

}

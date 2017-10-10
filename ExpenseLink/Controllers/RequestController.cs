using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using ExpenseLink.Models;
using ExpenseLink.Services;
using ExpenseLink.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ExpenseLink.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        public RequestController(IEmailService emailService)
        {
            _emailService = emailService;
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            if (User.IsInRole(RoleName.Employee))
            {
                var currentUserId = _userManager.FindById(User.Identity.GetUserId()).Id;
                var requests = _context.Requests.Include(r => r.Status).Where(r => r.ApplicationUser.Id == currentUserId).ToList();
                return View(requests);
            }

            if (User.IsInRole(RoleName.Manager))
            {
                var requests = _context.Requests.Include(r => r.Status).Where(r=>r.StatusId==StatusName.Submitted).ToList();
                return View(requests);
            }

            if (User.IsInRole(RoleName.Finance))
            {
                var requests = _context.Requests.Include(r => r.Status).Where(r => r.StatusId == StatusName.WaitingForReimbursement).ToList();
                return View(requests);
            }

            return View();
        }

        public ActionResult New(NewRequestViewModel newRequestViewModel)
        {
            ApplicationUser currentUser = _userManager.FindById(User.Identity.GetUserId());
            newRequestViewModel.Requester = currentUser.Name;
            return View(newRequestViewModel);
        }

        public ActionResult Create(NewRequestViewModel newRequestViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("New", newRequestViewModel);
            }

            ApplicationUser currentUser = _userManager.FindById(User.Identity.GetUserId());
            if (currentUser != null)
            {
                double total = 0;
                foreach (var receipt in newRequestViewModel.Receipts)
                {
                    _context.Receipts.Add(receipt);
                    total = total + receipt.ReimbursementAmount;
                }

                Request request = new Request()
                {
                    ApplicationUser = currentUser,
                    CreatedDate = newRequestViewModel.CreatedDate,
                    Receipts = newRequestViewModel.Receipts,
                    StatusId = newRequestViewModel.StatusId,
                    Total =  total
                };              

                _context.Requests.Add(request);
            }

            _context.SaveChanges();

            //Todo: Send email to manager
            _emailService.Send(new MailMessage());
            return RedirectToAction("Index", "Request");
        }

        public ActionResult Detail(int id)
        {
            var request = _context.Requests.Include(r => r.Status)
                                           .Include(r => r.Receipts)
                                           .FirstOrDefault(r => r.Id == id);
            ManagerRequestDetailViewModel viewModel = new ManagerRequestDetailViewModel();
            if (request != null)
            {
                viewModel.Id = request.Id;
                viewModel.CreatedDate = request.CreatedDate;
                viewModel.Receipts = request.Receipts;
                viewModel.StatusId = request.StatusId;
                viewModel.StatusName = request.Status.Name;
                viewModel.RequesterName = request.ApplicationUser.Name;
                viewModel.Reason = request.Reason;
                viewModel.Total = request.Total;
            }
            if (User.IsInRole(RoleName.Manager))            
                return View("ManagerDetail", viewModel);
           
            if (User.IsInRole(RoleName.Finance))            
                return View("FinanceDetail", viewModel);
            
            return View(viewModel);
        }

        [Authorize(Roles = RoleName.Manager)]
        public ActionResult Approve(int id)
        {
            try
            {
                var requestInDb = _context.Requests.Include(r => r.Status).Include(r => r.Receipts).Single(r => r.Id == id);
                requestInDb.StatusId = StatusName.WaitingForReimbursement;
                requestInDb.Reason = string.Empty;
                _context.SaveChanges();
                //Todo: Send email to finance user
                _emailService.Send(new MailMessage());
                return RedirectToAction("Index", "Request");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [Authorize(Roles = RoleName.Finance)]
        public ActionResult Reimbursed(int id)
        {
            try
            {
                var requestInDb = _context.Requests.Include(r => r.Status).Include(r => r.Receipts).Single(r => r.Id == id);
                requestInDb.StatusId = StatusName.Reimbursed;
                requestInDb.Reason = string.Empty;
                _context.SaveChanges();
                //Todo: Send email to user
                _emailService.Send(new MailMessage());
                return RedirectToAction("Index", "Request");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Authorize(Roles = RoleName.Manager)]
        public ActionResult Reject(int id, string reason)
        {
            try
            {
                var requestInDb = _context.Requests.Include(r => r.Status).Include(r => r.Receipts).Single(r => r.Id == id);
                requestInDb.StatusId = StatusName.Rejected;
                requestInDb.Reason = reason;
                _context.SaveChanges();
                return RedirectToAction("Index", "Request");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
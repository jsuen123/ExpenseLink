using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ExpenseLink.Models;
using ExpenseLink.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ExpenseLink.Controllers
{
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public RequestController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            var requests = _context.Requests.Include(r => r.Status).ToList();
            return View(requests);
        }

        public ActionResult New(NewRequestViewModel newRequestViewModel)
        {
            ApplicationUser currentUser = _userManager.FindById(User.Identity.GetUserId());
            newRequestViewModel.Requester = currentUser.Name;
            return View(newRequestViewModel);
        }

        public ActionResult Create(NewRequestViewModel newRequestViewModel)
        {
            ApplicationUser currentUser = _userManager.FindById(User.Identity.GetUserId());
            if (currentUser != null)
            {
                Request request = new Request()
                {
                    ApplicationUser = currentUser,
                    CreatedDate = newRequestViewModel.CreatedDate,
                    Receipts = newRequestViewModel.Receipts,
                    StatusId = newRequestViewModel.StatusId,
                };
                _context.Requests.Add(request);
                foreach (var receipt in request.Receipts)
                {
                    _context.Receipts.Add(receipt);
                }                
            }

            _context.SaveChanges();

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
            }

            return View(viewModel);
        }

        public ActionResult Approve(int id)
        {
            try
            {
                var requestInDb = _context.Requests.Include(r => r.Status).Include(r => r.Receipts).Single(r => r.Id == id);
                requestInDb.StatusId = StatusName.WaitingForReimbursement;
                requestInDb.Reason = string.Empty;
                _context.SaveChanges();
                return RedirectToAction("Index", "Request");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public ActionResult Reimbursed(int id)
        {
            try
            {
                var requestInDb = _context.Requests.Include(r => r.Status).Include(r => r.Receipts).Single(r => r.Id == id);
                requestInDb.StatusId = StatusName.Reimbursed;
                requestInDb.Reason = string.Empty;
                _context.SaveChanges();
                return RedirectToAction("Index", "Request");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

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
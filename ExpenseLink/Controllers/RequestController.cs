using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ExpenseLink.Models;
using ExpenseLink.ViewModels;

namespace ExpenseLink.Controllers
{
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestController()
        {
            _context = new ApplicationDbContext();
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

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Create(Receipt[] receipt)
        {
            throw new System.NotImplementedException();
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
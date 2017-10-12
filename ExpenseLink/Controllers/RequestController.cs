using System;
using System.Net.Mail;
using System.Web.Mvc;
using ExpenseLink.Models;
using ExpenseLink.Repository;
using ExpenseLink.Services;
using ExpenseLink.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ExpenseLink.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly IRequestRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public RequestController()
        {
            _emailService = new Services.EmailService();
            ApplicationDbContext context = new ApplicationDbContext();
            _repository = new RequestRepository(context);
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }

        public RequestController(IRequestRepository repository, UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _repository = repository;
            _userManager = userManager;
            _emailService = emailService;
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
        }

        public ActionResult Index()
        {
            if (User == null)
            return new HttpUnauthorizedResult();
                            
            if (User.IsInRole(RoleName.Employee))
            {
                var currentUserId = _userManager.FindById(User.Identity.GetUserId()).Id;
                var requests = _repository.GetRequestByUserId(currentUserId);
                return View("Index", requests);
            }

            if (User.IsInRole(RoleName.Manager))
            {
                var requests = _repository.GetRequestSubmitted(); 
                return View(requests);
            }

            if (User.IsInRole(RoleName.Finance))
            {
                var requests = _repository.GetRequestWaitingForReimbursement();
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
            try
            {
                //Todo: Implement Validation
                if (!ModelState.IsValid)
                    return View("New", newRequestViewModel);

                ApplicationUser currentUser = _userManager.FindById(User.Identity.GetUserId());
                if (currentUser != null)
                {
                    double total = 0;
                    foreach (var receipt in newRequestViewModel.Receipts)
                    {
                        _repository.AddReceipt(receipt);
                        total = total + receipt.ReimbursementAmount;
                    }

                    Request request = new Request()
                    {
                        ApplicationUser = currentUser,
                        CreatedDate = newRequestViewModel.CreatedDate,
                        Receipts = newRequestViewModel.Receipts,
                        StatusId = newRequestViewModel.StatusId,
                        Total = total
                    };

                    _repository.AddRequest(request);
                   
                    //Todo: Send email to manager
                    _emailService.Send(new MailMessage());

                }
            }
            catch (Exception e)
            {
                return View("Error", e);
            }

            return RedirectToAction("Index", "Request");

        }

        public ActionResult Detail(int id)
        {
            try
            {
                var request = _repository.GetRequestByRequestId(id);
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
            catch (Exception e)
            {
                return View("Error", e);
            }           
        }

        [Authorize(Roles = RoleName.Manager)]
        public ActionResult Approve(int id)
        {
            try
            {
                var requestInDb = _repository.GetRequestForApproval(id);

                _repository.SetRequestApproved(requestInDb);
                
                //Todo: Send email to finance user
                _emailService.Send(new MailMessage());
                return RedirectToAction("Index", "Request");
            }
            catch (Exception e)
            {
                return View("Error", e);
            }

        }

        [Authorize(Roles = RoleName.Finance)]
        public ActionResult Reimbursed(int id)
        {
            try
            {
                var requestInDb = _repository.GetRequestForApproval(id);

                _repository.SetRequestReimbursed(requestInDb);
                //Todo: Send email to user
                _emailService.Send(new MailMessage());
                return RedirectToAction("Index", "Request");
            }
            catch (Exception e)
            {
                return View("Error", e);
            }
        }

        [Authorize(Roles = RoleName.Manager)]
        public ActionResult Reject(int id, string reason)
        {
            try
            {
                var requestInDb = _repository.GetRequestForApproval(id);

                _repository.SetRequestRejected(requestInDb, reason);
                return RedirectToAction("Index", "Request");
            }
            catch (Exception e)
            {
                return View("Error", e);
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
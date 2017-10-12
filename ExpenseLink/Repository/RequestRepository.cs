using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ExpenseLink.Models;

namespace ExpenseLink.Repository
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public RequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Request> GetRequestSubmitted()
        {
            return _context.Requests.Include(r => r.Status).Where(r => r.StatusId == StatusName.Submitted).ToList();
        }

        public IList<Request> GetRequestByUserId(string userId)
        {
            return _context.Requests.Include(r => r.Status).Where(r => r.ApplicationUser.Id == userId).ToList();
        }

        public IList<Request> GetRequestWaitingForReimbursement()
        {
            return _context.Requests.Include(r => r.Status).Where(r => r.StatusId == StatusName.WaitingForReimbursement).ToList();
        }

        public void AddReceipt(Receipt receipt)
        {
            _context.Receipts.Add(receipt);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Request GetRequestByRequestId(int id)
        {
           return _context.Requests.Include(r => r.Status)
                .Include(r => r.Receipts)
                .FirstOrDefault(r => r.Id == id);
        }

        public Request GetRequestForApproval(int id)
        {
            return _context.Requests.Include(r => r.Status).Include(r => r.Receipts).Single(r => r.Id == id);
        }

        public void SetRequestApproved(Request requestInDb)
        {
            requestInDb.StatusId = StatusName.WaitingForReimbursement;
            requestInDb.Reason = string.Empty;
            _context.SaveChanges();
        }

        public void SetRequestReimbursed(Request requestInDb)
        {
            requestInDb.StatusId = StatusName.Reimbursed;
            requestInDb.Reason = string.Empty;
            _context.SaveChanges();
        }

        public void SetRequestRejected(Request requestInDb, string reason)
        {
            requestInDb.StatusId = StatusName.Rejected;
            requestInDb.Reason = reason;
            _context.SaveChanges();
        }

        public void AddRequest(Request request)
        {
            _context.Requests.Add(request);
            _context.SaveChanges();
        }
    }
}
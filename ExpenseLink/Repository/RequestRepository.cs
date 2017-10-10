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
    }
}
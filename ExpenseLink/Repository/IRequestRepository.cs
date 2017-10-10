using System.Collections.Generic;
using ExpenseLink.Models;

namespace ExpenseLink.Repository
{
    public interface IRequestRepository
    {
        IList<Request> GetRequestSubmitted();
        IList<Request> GetRequestByUserId(string userId);
        IList<Request> GetRequestWaitingForReimbursement();


    }
}
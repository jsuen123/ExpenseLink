using System.Collections.Generic;
using ExpenseLink.Models;

namespace ExpenseLink.Repository
{
    public interface IRequestRepository
    {
        IList<Request> GetRequestSubmitted();
        IList<Request> GetRequestByUserId(string userId);
        IList<Request> GetRequestWaitingForReimbursement();
        void AddReceipt(Receipt receipt);
        void AddRequest(Request request);
        void Dispose();
        Request GetRequestByRequestId(int id);
        Request GetRequestForApproval(int id);
        void SetRequestApproved(Request requestInDb);
        void SetRequestReimbursed(Request requestInDb);
        void SetRequestRejected(Request requestInDb, string reason);
    }
}
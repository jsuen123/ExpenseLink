using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpenseLink.Models;

namespace ExpenseLink.ViewModels
{
    public class NewRequestViewModel
    {
        public NewRequestViewModel()
        {
            Receipts = new List<Receipt>();
            StatusId = StatusName.Submitted;
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<Receipt> Receipts { get; set; }
        public byte StatusId { get; set; }
        public string Requester { get; set; }
    }
}
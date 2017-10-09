using System;
using System.Collections.Generic;
using ExpenseLink.Models;

namespace ExpenseLink.ViewModels
{
    public class ManagerRequestDetailViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }    
        public IEnumerable<Receipt> Receipts { get; set; }
        public Byte StatusId { get; set; }
        public string RequesterName { get; set; }
        public string Reason { get; set; }
    }
}
using System;
using System.Data;
using ExpenseLink.Models;

namespace ExpenseLink.ViewModels
{
    public class RequestListViewModel
    {
        public string Requester { get; set; }
        public DateTime RequestedDateTime { get; set; }
        public Status Status { get; set; }
    }
}
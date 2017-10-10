using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace ExpenseLink.Models
{
    public class Request
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public IList<Receipt> Receipts { get; set; }
        public Status Status { get; set; }
        public byte StatusId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string Reason { get; set; }
        public double Total { get; set; }
    }
}
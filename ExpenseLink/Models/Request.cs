using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public ApplicationUser CreatedBy { get; set; }
        public string Reason { get; set; }
    }
}
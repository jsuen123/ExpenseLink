using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseLink.Models
{
    public class Receipt
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int ReceiptNo { get; set; }
        [Required]
        public DateTime ReceiptDate { get; set; }
        public string ItemDescription { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public double ReimbursementAmount { get; set; }

    }
}
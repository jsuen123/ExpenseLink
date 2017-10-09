using System.ComponentModel.DataAnnotations;

namespace ExpenseLink.Models
{
    public class Status
    {
        [Required]
        public byte Id { get; set; }
        [Required]
        public string Name { get; set; }
    } 
}
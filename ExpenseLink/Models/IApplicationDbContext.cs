using System.Data.Entity;

namespace ExpenseLink.Models
{
    public interface IApplicationDbContext
    {
        DbSet<Receipt> Receipts { get; set; }
        DbSet<Request> Requests { get; set; }
        DbSet<Status> Statuses { get; set; }
    }
}
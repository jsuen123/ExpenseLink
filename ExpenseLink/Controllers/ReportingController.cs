using System.Web.Mvc;
using ExpenseLink.Models;

namespace ExpenseLink.Controllers
{
    public class ReportingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportingController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Reporting
        public ActionResult Index(string sortOrder)
        {
            //Todo: Implement reporting page
            return View();
        }
    }
}
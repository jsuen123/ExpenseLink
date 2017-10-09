using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ExpenseLink.Models;

namespace ExpenseLink.Controllers
{
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            var requests = _context.Requests.Include(r=>r.Status).ToList();
            //TODO: Include name, to create seprate table for name, instead of using the existing asp.net one

            return View(requests);
        }
    }
}
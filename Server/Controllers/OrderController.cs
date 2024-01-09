using ManagingSalesApp.Server.DB;
using ManagingSalesApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ManagingSalesApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        ApplicationContext db;  
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderController(ApplicationContext context, IHttpContextAccessor httpContextAccessor)
        {
            db = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IEnumerable<Order> Get()
        {
             var orders = db.Orders
                        .Include(o => o.Windows)  // Включение данных по связи Order -> Window
                            .ThenInclude(w => w.SubElements)  // Включение данных по связи Window -> SubElement
                        .ToList();
            foreach (var a in orders)
            {
                Console.WriteLine(a.Windows.First().Name);
            }
            return orders;
        }
    }
}

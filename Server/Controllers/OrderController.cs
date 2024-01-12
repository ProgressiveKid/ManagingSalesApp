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
        [HttpGet("GetAllOrders")]    
        public IEnumerable<Order> GetAllOrders()
        {
             var orders = db.Orders
                        .Include(o => o.Windows) 
                            .ThenInclude(w => w.SubElements)
                            .AsNoTracking()  
                        .ToList();
            return orders;
        }
        [HttpGet("GetOrdersName")]
        public IEnumerable<string> GetOrdersName()
        {
            List<string> orders = db.Orders.AsNoTracking().Select(u => u.Name).ToList();
            return orders;
        }
        [HttpGet("GetOneOrder")]
        public IEnumerable<Order> GetOneOrder(string nameOrder)
        {
            var order = db.Orders.AsNoTracking().Include(o => o.Windows) 
                            .ThenInclude(w => w.SubElements).
                            Where(u => u.Name == nameOrder).ToList();                    
            return order;
        }
        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder(Order order)
        {
            if (order.Name == "")
            {
                return BadRequest("The order doesn't have a name");
            }
            bool isUniqueName = !db.Orders.Any(o => o.Name == order.Name);
            if (isUniqueName)
            {
                db.Orders.Add(order);
                db.Windows.AddRange(order.Windows);
                foreach (var window in order.Windows)
                {
                    if (window.SubElements != null)
                    {
                        db.SubElements.AddRange(window.SubElements);
                    }                 
                }
                db.SaveChanges();
                return Ok(order);
            }
            else
            {
                return BadRequest("An order by that name already exists");
            }
        }
        [HttpPost("CreateWindow")]
        public IActionResult CreateWindow(Window window)
        {      
                db.Windows.Add(window);
                if (window.SubElements != null)
                {
                    db.SubElements.AddRange(window.SubElements);
                }
                db.SaveChanges();
                return Ok(window);
        }
        [HttpPost("CreateSubElement")]
        public IActionResult CreateSubElement(SubElement subElement)
        {
            db.SubElements.Add(subElement);
            db.SaveChanges();
            return Ok(subElement);
        }
        [HttpPut("EditOrder")]
        public IActionResult EditOrder(Order order)
        {
            Order existingOrder = db.Orders
            .Include(o => o.Windows) // Включаем связанные окна для обновления
            .ThenInclude(w => w.SubElements) // Включаем связанные подэлементы для обновления
            .FirstOrDefault(o => o.Id == order.Id);

            foreach (var updatedWindow in order.Windows)
            {
                Window existingWindow = existingOrder.Windows.FirstOrDefault(w => w.Id == updatedWindow.Id);

                if (existingWindow != null)
                {
                    existingWindow.Name = updatedWindow.Name;
                    existingWindow.QuantityOfWindows = updatedWindow.QuantityOfWindows;

                    foreach (var updatedSubElement in updatedWindow.SubElements)
                    {
                        SubElement existingSubElement = existingWindow.SubElements.FirstOrDefault(se => se.Id == updatedSubElement.Id);

                        if (existingSubElement != null)
                        {
                            existingSubElement.Type = updatedSubElement.Type;
                            existingSubElement.Width = updatedSubElement.Width;
                            existingSubElement.Height = updatedSubElement.Height;
                        }
                    }
                    var deletedSubElements = existingWindow.SubElements
                    .Where(se => !updatedWindow.SubElements.Any(use => use.Id == se.Id))
                    .ToList();

                    db.SubElements.RemoveRange(deletedSubElements);
                }
            }
            var deletedWindows = existingOrder.Windows
            .Where(w => !order.Windows.Any(uw => uw.Id == w.Id))
            .ToList();
            db.Windows.RemoveRange(deletedWindows);
            db.SaveChanges();
            return Ok(order);
        }


        [HttpPut("DeleteOrder")]
        public IActionResult DeleteOrder(Order order)
        {
            string message = $"Order {order.Name} № {order.Id} has been deleted from the database";

            Order orderToDelete = db.Orders
                .Include(o => o.Windows)
                .ThenInclude(w => w.SubElements)
                .FirstOrDefault(o => o.Id == order.Id);

            if (orderToDelete == null)
            {
                return BadRequest("Order not found in the database");
            }

            foreach (var window in orderToDelete.Windows)
            {
                db.SubElements.RemoveRange(window.SubElements);
            }

            db.Windows.RemoveRange(orderToDelete.Windows);

            db.Orders.Remove(orderToDelete);

            db.SaveChanges();
            return Ok(message);

        }

    }
}

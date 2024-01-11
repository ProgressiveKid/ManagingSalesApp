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
        // тут ShowAllOrders

        [HttpGet("GetAllOrders")]    
        public IEnumerable<Order> GetAllOrders()
        {
             var orders = db.Orders
                        .Include(o => o.Windows)  // Включение данных по связи Order -> Window
                            .ThenInclude(w => w.SubElements)
                            .AsNoTracking()  // для ускор
                        .ToList();
            foreach (var a in orders)
            {
             //   Console.WriteLine(a.Name);
            }
            return orders;
        }
        // тут Create Orders


        [HttpPut("CreateOrder")]
        public IActionResult CreateOrder(Order order)
        {
            // var existingOrder = dbContext.Orders.Find(updatedOrder.Id);
            //db.Orders.Update(u=> u.Id = order.Id);
            bool isUniqueName = !db.Orders.Any(o => o.Name == order.Name);

            if (isUniqueName)
            {


                db.Orders.Add(order);
                db.Windows.AddRange(order.Windows);

                foreach (var window in order.Windows)
                {
                    db.SubElements.AddRange(window.SubElements);
                }

                db.SaveChanges();
                var allOrder = db.Orders.ToList();
                var allWindow = db.Windows.ToList();
                return Ok(order);
                
            }
            else
            {
                ModelState.AddModelError(nameof(Order.Name), "Заказ с таким именем уже существует.");
                return BadRequest(ModelState);

            }

        }
        [HttpGet]
        public IEnumerable<Order> GetNameOrders()
        {
            var orders = db.Orders
                       .Include(o => o.Windows)  // Включение данных по связи Order -> Window
                           .ThenInclude(w => w.SubElements)
                           .AsNoTracking()  // для ускор
                       .ToList();
            foreach (var a in orders)
            {
                Console.WriteLine(a.Name);
            }
            return orders;
        }

        public IActionResult CreateOrder2(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            // Ваша логика обновления заказа
            // order содержит данные, переданные из клиентской стороны
            // Возвращаем JSON-ответ с обновленным объектом Order
            return Ok(order);
        }

    }
}

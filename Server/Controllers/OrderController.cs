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
            var order = db.Orders.AsNoTracking().Include(o => o.Windows)  // Включение данных по связи Order -> Window
                            .ThenInclude(w => w.SubElements).
                            Where(u => u.Name == nameOrder).ToList(); // для ускор                     
            return order;
        }
        // тут Create Orders
        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder(Order order)
        {
            if (order.Name == "")
            {
                return BadRequest("У заказа нет имени");
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
                //ModelState.AddModelError(nameof("Название заказа"), "Заказ с таким именем уже существует.");
                return BadRequest("Заказ с таким именем уже существует");
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
                // Находим соответствующее окно в существующем заказе
                Window existingWindow = existingOrder.Windows.FirstOrDefault(w => w.Id == updatedWindow.Id);

                if (existingWindow != null)
                {
                    // Обновление свойств окна
                    existingWindow.Name = updatedWindow.Name;
                    existingWindow.QuantityOfWindows = updatedWindow.QuantityOfWindows;

                    // Обновление свойств подэлементов окна
                    foreach (var updatedSubElement in updatedWindow.SubElements)
                    {
                        // Находим соответствующий подэлемент в существующем окне
                        SubElement existingSubElement = existingWindow.SubElements.FirstOrDefault(se => se.Id == updatedSubElement.Id);

                        if (existingSubElement != null)
                        {
                            // Обновление свойств подэлемента
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
            // Ваша логика обновления заказа
            // order содержит данные, переданные из клиентской стороны
            // Возвращаем JSON-ответ с обновленным объектом Order
            return Ok(order);
        }


        [HttpPut("DeleteOrder")]
        public IActionResult DeleteOrder(Order order)
        {
            string message = $"Заказ {order.Name} № {order.Id} был удалён из базы данных";

            // Находим заказ по его идентификатору в базе данных
            Order orderToDelete = db.Orders
                .Include(o => o.Windows)
                .ThenInclude(w => w.SubElements)
                .FirstOrDefault(o => o.Id == order.Id);

            if (orderToDelete == null)
            {
                // Заказ не найден в базе данных, обработайте это согласно вашей бизнес-логике.
                return BadRequest("Заказ не найден в базе данных");
            }

            // Удаляем все подэлементы связанных окон
            foreach (var window in orderToDelete.Windows)
            {
                db.SubElements.RemoveRange(window.SubElements);
            }

            // Удаляем все окна, связанные с заказом
            db.Windows.RemoveRange(orderToDelete.Windows);

            // Удаляем сам заказ
            db.Orders.Remove(orderToDelete);

            // Сохраняем изменения в базе данных
            db.SaveChanges();
            return Ok(message);

        }

    }
}

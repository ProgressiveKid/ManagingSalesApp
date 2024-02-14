using ManagingSalesApp.Shared;

namespace ManagingSalesApp.Server.Services.Interfaces
{
	public interface IOrderService
	{
		List<Order> GetAllOrders();

		List<string> GetOrdersName();
		List<Order> GetOneOrder(string nameOrder);

		Dictionary<bool, string> CreateOrder(Order order);

		Window CreateWindow(Window window);

		SubElement CreateSubElement(SubElement subElement);

		Order EditOrder(Order order);

		Dictionary<bool, string> DeleteOrder(Order order);
	}
}

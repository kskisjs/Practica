using System.Collections.Generic;

public class Store
{
    public Order[] OrdersArray;  // ← переименовал, чтобы не совпадало с именем класса

    public Store(Order[] orders)
    {
        OrdersArray = orders;
    }

    public List<Order> GetHighValueOrders(decimal minAmount)
    {
        List<Order> result = new List<Order>();
        foreach (Order order in OrdersArray)
        {
            if (order.TotalAmount > minAmount)
                result.Add(order);
        }
        return result;
    }

    public List<Order> GetOrdersByCustomer(string customerName)
    {
        List<Order> result = new List<Order>();
        foreach (Order order in OrdersArray)
        {
            if (order.CustomerName == customerName)
                result.Add(order);
        }
        return result;
    }
}
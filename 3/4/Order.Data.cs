public partial class Order
{
    public int OrderID;
    public string CustomerName;
    public decimal TotalAmount;

    public Order(int orderID, string customerName, decimal totalAmount)
    {
        OrderID = orderID;
        CustomerName = customerName;
        TotalAmount = totalAmount;
    }
}
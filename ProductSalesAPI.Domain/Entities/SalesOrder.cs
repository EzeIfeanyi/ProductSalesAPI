using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductSalesAPI.Domain.Entities;

public class SalesOrder
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int CustomerId { get; set; }
    public int Quantity { get; set; }
    public DateTime OrderDate { get; private set; } = DateTime.UtcNow;

    public SalesOrder() { }

    public SalesOrder(int productId, int customerId, int quantity)
    {
        ProductId = productId;
        CustomerId = customerId;
        Quantity = quantity;
        OrderDate = DateTime.UtcNow;
    }
}

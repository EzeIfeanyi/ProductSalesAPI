namespace ProductSalesAPI.Application.ApiUtilities.Shared;

public class ProductSalesResponse(int productId, string productName, int totalQuantitySold, decimal totalPriceSold)
{
    public int ProductId { get; set; } = productId;
    public string ProductName { get; set; } = productName;
    public int TotalQuantitySold { get; set; } = totalQuantitySold;
    public decimal TotalPriceSold { get; set; } = totalPriceSold;
}

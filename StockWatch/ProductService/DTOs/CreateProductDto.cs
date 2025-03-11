namespace ProductServiceAPI.DTOs
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public int CriticalStockLevel { get; set; }
        public string Location { get; set; }
    }

}

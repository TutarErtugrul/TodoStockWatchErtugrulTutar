namespace ProductServiceAPI.DTOs
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Location { get; set; }
        public int CriticalStockLevel { get; set; }
    }

}

namespace ProductServiceAPI.Models
{
    public class UpdateStock
    {
        public int Id { get; set; }
        public int StockQuantity { get; set; }
        public string OperationType { get; set; } = string.Empty;
    }
}

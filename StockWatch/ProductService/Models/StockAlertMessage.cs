namespace ProductServiceAPI.Models
{
    public class StockAlertMessage
    {
        public string ProductName { get; set; }
        public int CurrentStock { get; set; }
        public string Email { get; set; }
    }
}

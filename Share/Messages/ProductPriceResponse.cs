namespace Share.Messages
{
    public class ProductPriceResponse
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}

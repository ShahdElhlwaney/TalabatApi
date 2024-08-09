namespace Core.Entities
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; } 
        public int quantity { get; set; }
        public string Brand { get; set; }
    }
}
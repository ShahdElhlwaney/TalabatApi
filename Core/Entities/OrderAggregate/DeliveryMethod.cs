namespace Core.Entities.OrderAggregate
{
    public class DeliveryMethod:BaseEntity
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string DeliveryTime { get; set; }
        public decimal Price { get; set; }


    }
}
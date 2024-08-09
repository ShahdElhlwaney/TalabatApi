using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrderWithItemsSpecification : BaseSpecification<Order>
    {
        public OrderWithItemsSpecification(string email) : base(order=>order.BuyerEmail==email)
        {
            AddInclude(order => order.DeliveryMethod);
            AddInclude(order => order.orderItems);
            AddOrderByDesc(order => order.OrderDate);
        }
        public OrderWithItemsSpecification(int id,string email) : base(order =>order.Id==id && order.BuyerEmail == email)
        {
            AddInclude(order => order.DeliveryMethod);
            AddInclude(order => order.orderItems);
        }
    }
}

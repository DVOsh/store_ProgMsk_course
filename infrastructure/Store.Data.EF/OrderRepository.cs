using Microsoft.EntityFrameworkCore;

namespace Store.Data.EF
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextFactory dbContextFactory;
        //private readonly StoreDbContext storeDbContext;

        public OrderRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
            //storeDbContext = dbContextFactory.Create(typeof(OrderRepository));
        }

        public Order Create()
        {
            var dbContext = dbContextFactory.Create(typeof(OrderRepository));

            var dto = Order.DtoFactory.Create();
            dbContext.Orders.Add(dto);
            dbContext.SaveChanges();

            return Order.Mapper.Map(dto);
        }

        public Order GetById(int id)
        {
            var dbContext = dbContextFactory.Create(typeof(OrderRepository));

            var dto = dbContext.Orders
                               .Include(order => order.Items)
                               .Single(order => order.Id == id);

            return Order.Mapper.Map(dto);
        }

        public void Update(Order order)
        {
            var dbContext = dbContextFactory.Create(typeof(OrderRepository));

            dbContext.SaveChanges();
        }
    }
}

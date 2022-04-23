using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> orderRepo;
        private readonly IGenericRepository<DeliveryMethod> dmRepo;
        private readonly IGenericRepository<Product> productRepo;
        private readonly IBasketRepository basketRepo;
        public OrderService(
        IGenericRepository<Order> orderRepo, IGenericRepository<DeliveryMethod> dmRepo,
        IGenericRepository<Product> productRepo, IBasketRepository basketRepo)
        {
            this.basketRepo = basketRepo;
            this.productRepo = productRepo;
            this.dmRepo = dmRepo;
            this.orderRepo = orderRepo;
            
        }


    public async Task<Order> CreateOrderAsync(string email, int deliveryMethodId, string basketId, Address shippingAddres)
    {
        //get basket from basket repo
        var basket = await this.basketRepo.GetBasketAsync(basketId);

        //get items from product repo

        var items = new List<OrderItem>();
        
        foreach (var item in basket.Items)
        {
            var productItem = await this.productRepo.GetByIdAsync(item.Id);

            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, 
                                        productItem.PictureUrl);

            var oderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);

            items.Add(oderItem);
        }


        //get delivery method

        var deliveryMethod = await dmRepo.GetByIdAsync(deliveryMethodId);
        //calculate sub total

        var subtotal = items.Sum(x => x.Price * x.Quantity); 

        //create the order

        var order = new Order(email,shippingAddres,deliveryMethod,items,subtotal);
        //sve it to DB
        //todo

        return order;
    }

    public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
    {
        throw new System.NotImplementedException();
    }

    public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        throw new System.NotImplementedException();
    }
}
}
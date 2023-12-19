using Microsoft.EntityFrameworkCore;
using UserManagement.Core.Models;
using UserManagement.Service.Repository;

namespace UserManagement.EfDbRepo
{
    public class EfDbRepoService : ICustomerServiceDbRepo
    {
        private readonly ApplicationDBContext _dbContext;

        public EfDbRepoService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> AddNewCustomer(Customer customer)
        {
            try
            {
                await _dbContext.AddAsync(customer);
                var result = await _dbContext.SaveChangesAsync();
                if (result > 0)
                {
                    return customer;
                }
                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Customer> Customer(int Id)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x=> x.Id == Id);
            return customer;
        }

        public async Task<List<Customer>> Customers()
        {
            var customers = await _dbContext.Customers.ToListAsync();
            return customers;
        }

        public async Task<List<Order>> Orders()
        {
            var orders = await _dbContext.Orders.ToListAsync();
            return orders;
        }
    }
}
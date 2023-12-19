using UserManagement.Core.Interfaces;
using UserManagement.Core.Models;

namespace UserManagement.Service.Repository
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerServiceDbRepo _customerServiceRepo;

        public CustomerService(ICustomerServiceDbRepo customerServiceRepo)
        {
            _customerServiceRepo = customerServiceRepo;
        }

        public async Task<List<Customer>> Customers()
        {
            return await _customerServiceRepo.Customers();
        }
        public async Task<Customer> Customer(int Id)
        {
            return await _customerServiceRepo.Customer(Id);
        }
        public async Task<List<Order>> Orders()
        {
            return await _customerServiceRepo.Orders();
        }
        public async Task<Customer> AddNewCustomer(Customer customer)
        {
            var newCustomer = await _customerServiceRepo.AddNewCustomer(customer);
            return newCustomer;
        }
    }
}
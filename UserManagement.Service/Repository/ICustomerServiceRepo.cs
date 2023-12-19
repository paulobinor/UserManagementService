using UserManagement.Core.Models;

namespace UserManagement.Service.Repository
{
    public interface ICustomerServiceDbRepo
    {
        Task<List<Customer>> Customers();
        Task<Customer> Customer(int Id);
        Task<List<Order>> Orders();
        Task<Customer> AddNewCustomer(Customer customer);
    }
}
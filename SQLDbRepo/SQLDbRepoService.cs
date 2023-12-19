using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using UserManagement.Core.Models;
using UserManagement.Service.Repository;

namespace UserManagement.SQLDbRepo
{
    public class SQLDbRepoService : ICustomerServiceDbRepo
    {
        //IDbConnection dbConnection;
        public readonly IConfiguration _configuration;
        private string _sqlConnString;
        public SQLDbRepoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Customer> Customer(int Id)
        {
            _sqlConnString = _configuration.GetConnectionString("UserMgtConn");
            Customer customer = null;

            using (SqlConnection connection = new SqlConnection(_sqlConnString))
            {
                using (SqlCommand command = new SqlCommand("GetSingleCustomer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerId", Id);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            customer = new Customer
                            {
                                Id = Convert.ToInt32(reader["CustomerId"]),
                                Name = Convert.ToString(reader["CustomerName"])
                            };

                            if (reader["OrderId"] != DBNull.Value)
                            {
                                Order order = new Order
                                {
                                    Id = Convert.ToInt32(reader["OrderId"]),
                                    ProductName = Convert.ToString(reader["ProductName"]),
                                    Price = Convert.ToDecimal(reader["Price"])
                                };

                                customer.Orders = new List<Order> { order };
                            }
                        }
                    }
                }
            }

            return customer;
        }

        public async Task<List<Customer>> Customers()
        {
            _sqlConnString = _configuration.GetConnectionString("UserMgtConn");
            List<Customer> customers = new List<Customer>();
            using (SqlConnection connection = new SqlConnection(_sqlConnString))
            {
                using (SqlCommand command = new SqlCommand("GetAllCustomers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Customer customer = new Customer
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = Convert.ToString(reader["Name"])
                                // Add other properties as needed
                            };

                            customers.Add(customer);
                        }
                    }
                }
            }
            return customers;
        }

        public async Task<List<Order>> Orders()
        {
            List<Order> orders = new List<Order>();
            _sqlConnString = _configuration.GetConnectionString("UserMgtConn");

            using (SqlConnection connection = new SqlConnection(_sqlConnString))
            {
                using (SqlCommand command = new SqlCommand("GetOrders", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Order order = new Order
                            {
                                Id = Convert.ToInt32(reader["OrderId"]),
                                ProductName = Convert.ToString(reader["ProductName"]),
                                Price = Convert.ToDecimal(reader["Price"]),
                                CustomerId = Convert.ToInt32(reader["CustomerId"])
                                // Add other properties as needed
                            };

                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }

        public async Task<Customer> AddNewCustomer(Customer customer)
        {
            int newCustomerId = 0;
            _sqlConnString = _configuration.GetConnectionString("UserMgtConn");

            using (SqlConnection connection = new SqlConnection(_sqlConnString))
            {
                var command = new SqlCommand("AddCustomer", connection);

                command.CommandType = CommandType.StoredProcedure;

                // Add parameters
                command.Parameters.AddWithValue("@Name", customer.Name);

                // Add an output parameter to capture the newly added customer's ID
                SqlParameter outputParameter = new SqlParameter
                {
                    ParameterName = "@CustomerId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputParameter);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    // Retrieve the value of the output parameter after the stored procedure execution
                    newCustomerId = Convert.ToInt32(outputParameter.Value);
                    return new Customer { Id = newCustomerId, Name = customer.Name };
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }
    }
}
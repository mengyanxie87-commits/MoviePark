using System;
using System.Collections.Generic;
using System.Linq;
using MyApiProject.Services;
using Microsoft.EntityFrameworkCore;
namespace MyApiProject.Models
{
    /// <summary>
    /// Provides operations for managing customer data.
    /// </summary>
    public class CustomersServiceImp : IOperations<Customers, string>
    {
        private readonly NorthwndContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersServiceImp"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public CustomersServiceImp(NorthwndContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Console.WriteLine("CustomersServiceImp : DbContext object injected successfully");
        }
        /// <summary>
        /// Gets customer by ID (mock implementation).
        /// </summary>
        /// <param name="customerID">The customer ID.</param>
        /// <returns>A new Customers object with mock data.</returns>
        public Customers GetCustomerById(string customerID)
        {
            // 在這裡實現根據 customerID 查詢客戶資訊的邏輯
            // 可以從資料庫、API 或其他資料源中獲取客戶資訊
            // 然後返回一個 Customers 類型的實例

            // 以下是一個簡單的範例實現
            return new Customers
            {
                CustomerID = customerID,
                Name = "John Doe",
                Address = "123 Main St, Anytown USA"
            };
        }
        /// <summary>
        /// Finds customers by a given condition (name contains condition).
        /// </summary>
        /// <param name="condition">The condition to search for in customer names.</param>
        /// <returns>A list of customers matching the condition.</returns>
        public List<Customers> FindByCondition(string condition)
        {
            // 假設用名字查詢
            return _context.Customers
                           .Where(c => c.Name.Contains(condition))
                           .ToList();
        }

        /// <summary>
        /// Finds all customers.
        /// </summary>
        /// <returns>A list of all customers.</returns>
        public List<Customers> FindAll()
        {
            return _context.Customers.ToList();
        }

        /// <summary>
        /// Gets all customers.
        /// </summary>
        /// <returns>An enumerable collection of all customers.</returns>
        public IEnumerable<Customers> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        /// <summary>
        /// Finds a customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <returns>The customer if found, otherwise null.</returns>
        public Customers? FindById(string id)
        {
            return _context.Customers
                           .FirstOrDefault(c => c.CustomerID.ToString() == id);
        }

        /// <summary>
        /// Adds a new customer.
        /// </summary>
        /// <param name="customers">The customer to add.</param>
        /// <returns>True if the customer was added successfully, false otherwise.</returns>
        /// <exception cref="Exception">Thrown if the customer already exists or insertion fails.</exception>
        public bool AddCustomer(Customers customers)
        {
            bool r = false;

            if (_context.Customers.Any(c => c.CustomerID == customers.CustomerID))
            {
                throw new Exception($"客戶編號:{customers.CustomerID}已存在!!!");
            }
            _context.Customers.Add(customers);
            try
            {
                Int32 counter = _context.SaveChanges();//inserted success
                r = true;

            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"客戶編號:{customers.CustomerID}新增失敗!!!", ex);
            }
            return r;

        }
        /// <summary>
        /// Inserts a new customer entity.
        /// </summary>
        /// <param name="entity">The customer entity to insert.</param>
        public void Insert(Customers entity)
        {
            // Implementation code here
            _context.Customers.Add(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Inserts a new customer.
        /// </summary>
        /// <param name="customers">The customer to insert.</param>
        /// <returns>True if the customer was inserted successfully, false otherwise.</returns>
        /// <exception cref="Exception">Thrown if the customer already exists or insertion fails.</exception>
        public bool InsertCustomer(Customers customers)
        {
            bool r = false;
            // Implementation code here
            if (_context.Customers.Any(c => c.CustomerID == customers.CustomerID))
            {
                throw new Exception($"客戶編號:{customers.CustomerID}已存在!!!");
            }
            _context.Customers.Add(customers);
            try
            {

                Int32 row = _context.SaveChanges();//同步到資料庫,轉譯成Insert o SQL
                r = true;

            }
            catch (DbUpdateException)
            {

                throw new Exception($"新增客戶編號:{customers.CustomerID}失敗!!!");

            }

            return r;

        }
        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="customers">The customer with updated information.</param>
        /// <returns>True if the customer was updated successfully, false otherwise.</returns>
        /// <exception cref="Exception">Thrown if the customer does not exist or update fails.</exception>
        public bool UpdateCustomer(Customers customers)
        {
            if (!_context.Customers.Any(c => c.CustomerID == customers.CustomerID))
            {

                throw new Exception($"客戶編號:{customers.CustomerID}不存在，無法更新");
            }
            _context.Customers.Update(customers);
            try
            {
                Int32 row = _context.SaveChanges();
                //updated success
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"客戶編號:{customers.CustomerID}更新失敗!!!", ex);
            }
        }

        /// <summary>
        /// Updates an existing customer entity.
        /// </summary>
        /// <param name="entity">The customer entity to update.</param>
        public void Update(Customers entity)
        {
            UpdateCustomer(entity);
        }
        /// <summary>
        /// Deletes a customer by ID.
        /// </summary>
        /// <param name="customerID">The ID of the customer to delete.</param>
        /// <returns>True if the customer was deleted successfully, false otherwise.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the customer is not found.</exception>
        /// <exception cref="Exception">Thrown if deletion fails.</exception>
        public bool DeleteCustomer(string customerID)
        {
            var customer = FindById(customerID);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {customerID} not found.");
            }
            _context.Customers.Remove(customer);

            try
            {
                _context.SaveChanges();

            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"客戶編號:{customer.CustomerID}刪除失敗!!!", ex);
            }
            return true;
        }

        /// <summary>
        /// Deletes a customer entity.
        /// </summary>
        /// <param name="entity">The customer entity to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the customer is not found.</exception>
        /// <exception cref="Exception">Thrown if deletion fails.</exception>
        public void Delete(Customers entity)
        {
            var customer = FindById(entity.CustomerID);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {entity.CustomerID} not found.");
            }
            _context.Customers.Remove(customer);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"客戶編號:{entity.CustomerID}刪除失敗!!!", ex);
            }
        }
    }
}

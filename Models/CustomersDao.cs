// CustomersDao.cs
using System;
using System.Collections.Generic;
using System.Linq;
using MyApiProject.Services;

namespace MyApiProject.Models
{
    public class CustomersDao : IOperations<Customers, string>
    {
        private readonly NorthwndContext _context;
        private List<Customers> _customers = new List<Customers>();
        public CustomersDao(NorthwndContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<Customers> FindAll()
        {
            return _context.Customers.ToList();
        }

        public Customers FindById(string id)
        {
            // 將 id 轉換為整數
            string customerId = id.ToString();
            return _customers.Find(customer => customer.CustomerID == customerId);
        }


        public void Insert(Customers entity)
        {
            _context.Customers.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Customers entity)
        {
            try
            {
                _context.Customers.Update(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // 處理例外情況，例如記錄錯誤
                Console.WriteLine(ex.Message);
            }
        }

        public void Delete(Customers entity)
        {
            _context.Customers.Remove(entity);
            _context.SaveChanges();
        }

        public List<Customers> FindByCondition(string condition)
        {
            // 假設這是一個簡單的條件查詢
            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;
using Microsoft.AspNetCore.Mvc;
using MyApiProject.Models;
namespace MyApiProject.Models
{
    public class NorthwndContext : DbContext
    {
       

        public NorthwndContext(DbContextOptions<NorthwndContext> options)
         : base(options)
        {

        }

        // 假設你有一個 Products 資料表
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Customers> Customers { get; set; }
    }
}

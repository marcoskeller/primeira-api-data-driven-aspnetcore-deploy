using Microsoft.EntityFrameworkCore;
using Primeira_api_data_driven_asp.Models;

namespace Primeira_api_data_driven_asp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base (options)
        {           
        } 

        public DbSet<Product> Products { get; set;}

        public DbSet<Category> Categories { get; set;}

        public DbSet<User> Users { get; set;}
    }
}



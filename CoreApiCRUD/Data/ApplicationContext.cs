using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CoreApiCRUD.Models;

namespace CoreApiCRUD.Data
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext>options): base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ReactInsert> ReactInsert { get; set; }

    }
}

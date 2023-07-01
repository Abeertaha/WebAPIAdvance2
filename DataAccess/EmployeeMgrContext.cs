using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmployeeApp.Models;

namespace EmployeeApp.DataAccess
{
    public class EmployeeMgrContext : DbContext
    {
        public required DbSet<EmployeeMgrContext> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(
                "host=localhost port=5432 dbname=NewDB user=postgres password=154444 sslmode=prefer connect_timeout=10;");
        }
    }
}

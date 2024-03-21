using Microsoft.EntityFrameworkCore;
using Kafka_Employe_App.models;

namespace Kafka_Employe_App.database
{
    public class EmployedbContext : DbContext
    {
        public EmployedbContext(DbContextOptions<EmployedbContext> dbContextOptions):base(dbContextOptions) { 

        
        }   
        public DbSet<Employee> Employees { get; set; }  

    }
}

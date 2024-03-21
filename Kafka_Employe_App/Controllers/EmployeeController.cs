using Confluent.Kafka;
using Kafka_Employe_App.database;
using Kafka_Employe_App.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Kafka_Employe_App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController(EmployedbContext dbcontext,ILogger<EmployeeController> logger) : ControllerBase
    {
        

        private readonly ILogger<EmployeeController> _logger = logger;
        private readonly EmployedbContext _employedbContext =dbcontext;


        

        [HttpGet(Name = "GetEmployees")]
        public async Task< IEnumerable<Employee>> GetEmployees()
        {
            _logger.LogInformation("Requesting all employees");
            return await _employedbContext.Employees.ToListAsync();

        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(string name ,string surname)
        {
            var employe = new Employee(Guid.NewGuid(), name, surname);
            _employedbContext.Add(employe);
            await _employedbContext.SaveChangesAsync();
            //Produce the message to the consumer 
            var message = new Message<string, string>()
            {
                Key = employe.Id.ToString(),
                Value = JsonSerializer.Serialize(employe)

            };
            //consfigure  the producer 
            var producerConfig = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092",
                Acks = Acks.All //None: not waiting for info from the broker 
            };
            //build the producer : 
            var Producer = new ProducerBuilder<string,string>(producerConfig) 
                .Build();
            await Producer.ProduceAsync("test1", message);
            Producer.Dispose(); 

            //Broker1(get the data ) Broker2(replicate the data) (ISR) -broker contain some only part of the topic 

            return CreatedAtAction(nameof(CreateEmployee),new {id=employe.Id},employe);
        }
    }
}

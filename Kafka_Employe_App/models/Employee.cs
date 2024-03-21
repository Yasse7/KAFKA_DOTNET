namespace Kafka_Employe_App.models
{
    public record Employee
    {
        public Guid Id { get; set; }    
        public string Name { get; set; }
        public string Surname { get; set; }

        public Employee(Guid id ,string name , string surname)
        {
            Id = id;
            Name = name;
            Surname = surname;
        }

    }
}

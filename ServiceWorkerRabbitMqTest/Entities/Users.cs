namespace ServiceWorkerRabbitMqTest.Entities
{
    public class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        // Other properties
        public ICollection<Items> Items { get; set; }
    }
}

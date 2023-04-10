using System.ComponentModel;

namespace ServiceWorkerRabbitMqTest.Entities
{
    public class Items
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        public DateTime? DeletedTime { get; set; }
     
        public int UserId { get; set; }
        public Users Users { get; set; }
    }
}

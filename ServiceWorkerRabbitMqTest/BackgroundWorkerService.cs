using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using ServiceWorkerRabbitMqTest.Data;
using ServiceWorkerRabbitMqTest.Entities;

public class BackgroundWorkerService : IHostedService
{
    readonly ILogger<BackgroundWorkerService> _logger;
    private readonly IServiceScopeFactory _servicescopefactory;   
  
    
    public BackgroundWorkerService(ILogger<BackgroundWorkerService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger= logger;
        _servicescopefactory = serviceScopeFactory;  

    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service started.");
        while (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}.");

            StartBasicConsume();

            await Task.Delay(1000, cancellationToken);
        }        
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service stopped.");
        return Task.CompletedTask;
    }

    private void StartBasicConsume()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            DispatchConsumersAsync = true
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        var eventName = "DeleteItemTest";

        channel.QueueDeclare(eventName, false, false, false, null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += Consumer_Received;

        channel.BasicConsume(eventName, true, consumer);
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
    {        
        var message = Encoding.UTF8.GetString(e.Body.ToArray());
        var myItem = JsonConvert.DeserializeObject<Items>(message);
        await Handle(myItem).ConfigureAwait(false);        
    }
    
    private async Task Handle(Items item)
    {
        DeleteItem(new Items()
        {
            Id = item.Id,
            Name = item.Name,

            DeletedTime = DateTime.Now.ToUniversalTime(),
            UserId = item.UserId,
            IsDeleted = true,
        });
        await Task.CompletedTask;
    }
    private void DeleteItem(Items item)
    {
        using (var scope = _servicescopefactory.CreateScope())
        {
            // Resolve the scoped service within the scope
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            // Do work with the scoped service
            context.Update(item);
            context.SaveChanges();
        }
        
    }
}

using FilmesAPI.DTO;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FilmesAPI.Services;

public class RabbitMqService
{
    public async Task PublicarMensagemDeEmailAsync(MensagemEmailDTO mensagem)
    {

        var factory = new ConnectionFactory() { HostName = "rabbitmq", UserName = "moovadmin", Password="moovsenha123" };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "emails_queue",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var json = JsonSerializer.Serialize(mensagem);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(exchange: string.Empty,
                             routingKey: "emails_queue",
                             body: body);
    }
}
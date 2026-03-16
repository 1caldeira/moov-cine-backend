using FilmesAPI.DTO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;

namespace FilmesAPI.Workers;

public class EmailWorker : BackgroundService
{
    private readonly ILogger<EmailWorker> _logger;
    private readonly IConfiguration _configuration;

    public EmailWorker(ILogger<EmailWorker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Email Worker iniciado, aguardando mensagens...");

        var rabbitHost = _configuration["RabbitMQ:HostName"] ?? "rabbitmq";
        var factory = new ConnectionFactory()
        {
            HostName = rabbitHost,
            UserName = "moovadmin",
            Password = "moovsenha123",
        };

        var connection = await factory.CreateConnectionAsync(stoppingToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(queue: "emails_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var emailDto = JsonSerializer.Deserialize<MensagemEmailDTO>(json);

                _logger.LogInformation($"Processando e-mail para: {emailDto.Destinatario}");

                await EnviarEmailAsync(emailDto);

                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);

                _logger.LogInformation("E-mail enviado e removido da fila com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao processar mensagem: {ex.Message}");

                await channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await channel.BasicConsumeAsync("emails_queue", autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }


    private async Task EnviarEmailAsync(MensagemEmailDTO email)
    {
        var host = _configuration["Smtp:Host"];
        var port = int.Parse(_configuration["Smtp:Port"]);
        var user = _configuration["Smtp:Username"];
        var pass = _configuration["Smtp:Password"];

        string remetenteEmail = "nao-responda@moovcine.site";
        string remetenteNome = "Moov Cine";

        var smtpClient = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(user, pass),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("nao-responda@moovcine.site", "Moov Cine"),
            Subject = email.Assunto,
            Body = email.Corpo,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(email.Destinatario);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
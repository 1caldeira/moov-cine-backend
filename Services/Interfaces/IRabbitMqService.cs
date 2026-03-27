using FilmesAPI.DTO;
using System.Threading.Tasks;

namespace FilmesAPI.Services.Interfaces;

public interface IRabbitMqService
{
    Task PublicarMensagemDeEmailAsync(MensagemEmailDTO mensagem);
}

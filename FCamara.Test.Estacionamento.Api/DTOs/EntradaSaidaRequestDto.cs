using System.ComponentModel.DataAnnotations;

namespace FCamara.Test.Estacionamento.Api.DTOs
{
    public class EntradaSaidaRequestDto
    {
        [Required(ErrorMessage = "A Placa do veículo é obrigatória.")]
        public string Placa { get; set; }
    }
}

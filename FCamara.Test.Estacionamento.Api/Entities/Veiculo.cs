using System.ComponentModel.DataAnnotations;

namespace FCamara.Test.Estacionamento.Api.Entities
{
    public enum TipoVeiculo
    {
        Carro = 0,
        Moto = 1
    }

    public class Veiculo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Marca é obrigatório")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "O campo Modelo é obrigatório")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "O campo Cor é obrigatório")]
        public string Cor { get; set; }

        [Required(ErrorMessage = "O campo Placa é obrigatório")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "O campo Tipo é obrigatório")]
        public TipoVeiculo Tipo { get; set; }
    }
}

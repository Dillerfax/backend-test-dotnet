using System.ComponentModel.DataAnnotations;

namespace FCamara.Test.Estacionamento.Api.Entities
{
    public class Estabelecimento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo CNPJ é obrigatório")]
        public string CNPJ { get; set; }

        [Required(ErrorMessage = "O campo Endereço é obrigatório")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "O campo Telefone é obrigatório")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O campo Quantidade de Vagas para Motos é obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade de vagas para motos deve ser um valor não negativo")]
        public int QuantidadeVagasMotos { get; set; }

        [Required(ErrorMessage = "O campo Quantidade de Vagas para Carros é obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade de vagas para carros deve ser um valor não negativo")]
        public int QuantidadeVagasCarros { get; set; }
    }
}

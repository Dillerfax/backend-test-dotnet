using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCamara.Test.Estacionamento.Api.Entities
{
    public class RegistroEstacionamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EstabelecimentoId { get; set; }

        [Required]
        public int VeiculoId { get; set; }

        [Required]
        public DateTime HoraEntrada { get; set; }

        public DateTime? HoraSaida { get; set; }

        [ForeignKey("EstabelecimentoId")]
        public virtual Estabelecimento Estabelecimento { get; set; }

        [ForeignKey("VeiculoId")]
        public virtual Veiculo Veiculo { get; set; }
    }
}

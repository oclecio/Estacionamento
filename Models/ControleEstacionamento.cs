using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Estacionamento.Models
{
    public class ControleEstacionamento
    {

        [Key]
        public int Id { get; set; }

        [Column("placa", TypeName = "varchar(7)")]
        [RegularExpression(@"[a-zA-Z]{3}-[0-9]{4}$", ErrorMessage = "Digite somente letras e números")]
        [DisplayName("Placa")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Placa { get; set; }

        [Column("data_entrada", TypeName = "datetime")]
        [DisplayName("Entrada")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public DateTime DataEntrada { get; set; }

        [Column("data_saida", TypeName = "datetime")]
        [DisplayName("Saída")]
        public Nullable<DateTime>DataSaida { get; set; }

        [Column("valor_final", TypeName = "decimal(10,2)")]
        [DisplayName("Valor final")]
        [DataType(DataType.Currency)]
        public Nullable<decimal> ValorCobrado { get; set; }

        [Column("tempo_total", TypeName = "time")]
        [DisplayName("Tempo total")]
        public TimeSpan TempoTotal { get; set; }

    }
}

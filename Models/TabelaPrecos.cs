using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Estacionamento.Models
{
    public class TabelaPrecos
    {
        [Key]
        public int Id { get; set; }

        [Column("data_inicio", TypeName = "datetime")]
        [DisplayName("Data de início")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public DateTime DataInicio { get; set; }

        [Column("data_fim", TypeName = "datetime")]
        [DisplayName("Data de término")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public DateTime DataFim { get; set; }

        [Column("preco_inicial", TypeName = "decimal(10,2)")]
        [DisplayName("Preço inicial")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public decimal PrecoInicial{ get; set; }

        [Column("preco_adicional", TypeName = "decimal(10,2)")]
        [DisplayName("Preço adicional")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public decimal PrecoAdicional { get; set; }
    }
}

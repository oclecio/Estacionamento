using Estacionamento.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estacionamento.Data
{
    public class EstacionamentoContext:DbContext
    {
        public EstacionamentoContext()
        {

        }
        public EstacionamentoContext(DbContextOptions<EstacionamentoContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ControleEstacionamento> ControleEstacionamento { get; set; }

        public DbSet<TabelaPrecos> TabelaPrecos { get; set; }
    }
}

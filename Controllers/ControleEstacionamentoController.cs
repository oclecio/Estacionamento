using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Estacionamento.Data;
using Estacionamento.Models;

namespace Estacionamento.Controllers
{
    public class ControleEstacionamentoController : Controller
    {
        private readonly EstacionamentoContext _context;

        public ControleEstacionamentoController(EstacionamentoContext context)
        {
            _context = context;
        }

        // GET: ControleEstacionamento
        public async Task<IActionResult> Index()
        {
            return View(await _context.ControleEstacionamento.OrderBy(c => c.DataSaida).ToListAsync());
        }

        public IActionResult Consulta()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Consulta([Bind("Id,Placa,DataEntrada,DataSaida")] ControleEstacionamento controleEstacionamento)
        {
            if (ModelState.IsValid)
            {

                var novoRegistro = this.VerificaCarroEstacionamento(controleEstacionamento.Placa, controleEstacionamento);
                if(novoRegistro.Count() == 0)
                {
                    controleEstacionamento.DataEntrada = DateTime.Now;
                    _context.Add(controleEstacionamento);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(Saida), new { placa=novoRegistro.First().Placa});
                }
            }
            return View(controleEstacionamento);
        }

        public async Task<IActionResult> Saida(string placa)
        {
            if (placa == null)
            {
                return NotFound();
            }
            var tabelaPrecos = _context.TabelaPrecos.Where(x => x.DataInicio <= DateTime.Today && x.DataFim>= DateTime.Today).FirstOrDefault();
            var retorno = await _context.ControleEstacionamento.Where(c => c.Placa == placa && c.DataSaida == null).FirstAsync();
            retorno.DataSaida = DateTime.Now;

            TimeSpan tempoTotal = (TimeSpan)(retorno.DataSaida - retorno.DataEntrada);
            Decimal valorCobrado = 0;
            if (tempoTotal.Hours == 0 && tempoTotal.Minutes <= 30)
            {
                valorCobrado = tabelaPrecos.PrecoInicial / 2;
            }
            else if ((tempoTotal.Hours == 0 && tempoTotal.Minutes > 30) || (tempoTotal.Hours == 1 && tempoTotal.Minutes <= 10))
            {
                valorCobrado = tabelaPrecos.PrecoInicial;
            }
            else if (tempoTotal.Hours >= 1 && tempoTotal.Minutes <= 10)
            {
                valorCobrado = ((tempoTotal.Hours - 1) * tabelaPrecos.PrecoAdicional) + tabelaPrecos.PrecoInicial;
            }
            else
            {
                valorCobrado = (tempoTotal.Hours * tabelaPrecos.PrecoAdicional) + tabelaPrecos.PrecoInicial;
            }
            retorno.TempoTotal = tempoTotal;
            retorno.ValorCobrado = valorCobrado;

            _context.Update(retorno);
            await _context.SaveChangesAsync();

            if (retorno == null)
            {
                return NotFound();
            }

            return View(retorno);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controleEstacionamento = await _context.ControleEstacionamento.FindAsync(id);
            if (controleEstacionamento == null)
            {
                return NotFound();
            }

            return View(controleEstacionamento);
        }

        private IQueryable<ControleEstacionamento> VerificaCarroEstacionamento(string placa, ControleEstacionamento controleEstacionamento)
        {
            var controle = _context.ControleEstacionamento.Where(c => c.Placa == placa && c.DataSaida == null);
            return controle;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Placa,DataEntrada,DataSaida")] ControleEstacionamento controleEstacionamento)
        {
            if (id != controleEstacionamento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    controleEstacionamento.DataSaida = DateTime.Now;
                    _context.Update(controleEstacionamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ControleEstacionamentoExists(controleEstacionamento.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), "TabelaPrecos");
            }
            return View(controleEstacionamento);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controleEstacionamento = await _context.ControleEstacionamento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (controleEstacionamento == null)
            {
                return NotFound();
            }

            return View(controleEstacionamento);
        }


        // POST: ControleEstacionamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var controleEstacionamento = await _context.ControleEstacionamento.FindAsync(id);
            _context.ControleEstacionamento.Remove(controleEstacionamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ControleEstacionamentoExists(int id)
        {
            return _context.ControleEstacionamento.Any(e => e.Id == id);
        }
    }
}

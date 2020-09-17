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
    public class TabelaPrecosController : Controller
    {
        private readonly EstacionamentoContext _context;
        public decimal precoInicial;

        public TabelaPrecosController(EstacionamentoContext context)
        {
            _context = context;
        }

        // GET: TabelaPrecos
        public async Task<IActionResult> Index()
        {
            /*precoInicial = this.GetPrecoPeriodo().PrecoInicial;*/
            return View(await _context.TabelaPrecos.ToListAsync());
        }

        // GET: TabelaPrecos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tabelaPrecos = await _context.TabelaPrecos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tabelaPrecos == null)
            {
                return NotFound();
            }

            return View(tabelaPrecos);
        }

        // GET: TabelaPrecos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TabelaPrecos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DataInicio,DataFim,PrecoInicial,PrecoAdicional")] TabelaPrecos tabelaPrecos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tabelaPrecos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tabelaPrecos);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tabelaPrecos = await _context.TabelaPrecos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tabelaPrecos == null)
            {
                return NotFound();
            }

            return View(tabelaPrecos);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tabelaPrecos = await _context.TabelaPrecos.FindAsync(id);
            _context.TabelaPrecos.Remove(tabelaPrecos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TabelaPrecosExists(int id)
        {
            return _context.TabelaPrecos.Any(e => e.Id == id);
        }

        public TabelaPrecos GetPrecoPeriodo()
        {

            var tabelaPrecos = _context.TabelaPrecos.Where(x => x.DataInicio <= DateTime.Now && x.DataFim >= DateTime.Now).FirstOrDefault();
            return tabelaPrecos;
        }
    }
}

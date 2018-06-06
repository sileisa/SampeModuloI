using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sampe.Models;

namespace Sampe.Controllers
{
    public class OrdemProducaoPecasController : Controller
    {
        private SampeContext db = new SampeContext();
        public int OPnoMes = 0;
        // GET: OrdemProducaoPecas
        public ActionResult Index()
        {
            var ordemProducaoPecas = db.OrdemProducaoPecas.Include(o => o.Expectativa);
            return View(ordemProducaoPecas.ToList());
        }

        // GET: OrdemProducaoPecas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdemProducaoPeca ordemProducaoPeca = db.OrdemProducaoPecas.Find(id);
            ordemProducaoPeca.Expectativa = db.Expectativas.Find(ordemProducaoPeca.ExpectativaId);
            db.Entry(ordemProducaoPeca).Reference(f => f.Expectativa).Load();
            if (ordemProducaoPeca == null)
            {
                return HttpNotFound();
            }
            return View(ordemProducaoPeca);
        }

        // GET: OrdemProducaoPecas/Create
        public ActionResult Create()
        {
            ViewBag.ExpectativaId = new SelectList(db.Expectativas, "ExpectativaId", "Produto");
            return View();
        }

        // POST: OrdemProducaoPecas/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoIdentificador,ExpectativaId,Data,MateriaPrima," +
            "MPLote,MPConsumo,ProdIncio,ProdFim,Maquina,Produto,ProdutoCor,MasterLote,Fornecedor,TempAgua," +
            "NivelOleo,Galho,OffSpec,RefugoKg,UnidadesProduzidas,ContadorInicial,ContadorFinal")] OrdemProducaoPeca ordemProducaoPeca)
        {
            var a = Request.Form["Parada.HoraParada"];
            var b = Request.Form["Parada.HoraRetorno"];
            var c = Request.Form["Parada.Motivo"];
            var d = Request.Form["Parada.Observacoes"];
            if (ModelState.IsValid)
            {
                string[] hrP = a.Split(',').ToArray();
                string[] hrR = b.Split(',').ToArray();
                string[] mot = c.Split(',').ToArray();
                string[] obs = d.Split(',').ToArray();
                List<Parada> parada = new List<Parada>();
                for (int x = 0; x < hrP.Count(); x++)
                {
                    Parada p = new Parada();
                    p.HoraParada = hrP[x];
                    p.HoraRetorno = hrR[x];
                    p.Motivo = mot[x];
                    p.Observacoes = obs[x];
                    parada.Add(p);
                }

                ordemProducaoPeca.Paradas = parada;

                var qtdOp = 0;
                var mesAnterior=0;
                if (db.OrdemProducaoPecas.ToList().Count() > 0)
                {
                    qtdOp = db.OrdemProducaoPecas.ToList().Last().OPnoMes;
                    mesAnterior = db.OrdemProducaoPecas.ToList().Last().Data.Month;
                }
                if (mesAnterior != ordemProducaoPeca.Data.Month)
                {
                    ordemProducaoPeca.OPnoMes = 1;
                }
                else
                ordemProducaoPeca.OPnoMes = qtdOp + 1;
                ordemProducaoPeca.Expectativa = db.Expectativas.Find(ordemProducaoPeca.ExpectativaId);
                //db.Entry(ordemProducaoPeca).Reference(f => f.Expectativa).Load();
                ordemProducaoPeca.ConfiguracaoInicial();
                db.OrdemProducaoPecas.Add(ordemProducaoPeca);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ExpectativaId = new SelectList(db.Expectativas, "ExpectativaId", "Produto", ordemProducaoPeca.ExpectativaId);
            return View(ordemProducaoPeca);
        }

        // GET: OrdemProducaoPecas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdemProducaoPeca ordemProducaoPeca = db.OrdemProducaoPecas.Find(id);
            if (ordemProducaoPeca == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExpectativaId = new SelectList(db.Expectativas, "ExpectativaId", "Produto", ordemProducaoPeca.ExpectativaId);
            return View(ordemProducaoPeca);
        }

        // POST: OrdemProducaoPecas/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoIdentificador,ExpectativaId,Data,MateriaPrima,MPLote,MPConsumo,ProdIncio,ProdFim,Maquina,Produto,ProdutoCor,MasterLote,Fornecedor,TempAgua,NivelOleo,Galho,OffSpec,RefugoKg,UnidadesProduzidas,ContadorInicial,ContadorFinal")] OrdemProducaoPeca ordemProducaoPeca)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordemProducaoPeca).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExpectativaId = new SelectList(db.Expectativas, "ExpectativaId", "Produto", ordemProducaoPeca.ExpectativaId);
            return View(ordemProducaoPeca);
        }

        // GET: OrdemProducaoPecas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdemProducaoPeca ordemProducaoPeca = db.OrdemProducaoPecas.Find(id);
            if (ordemProducaoPeca == null)
            {
                return HttpNotFound();
            }
            return View(ordemProducaoPeca);
        }

        // POST: OrdemProducaoPecas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            OrdemProducaoPeca ordemProducaoPeca = db.OrdemProducaoPecas.Find(id);
            db.OrdemProducaoPecas.Remove(ordemProducaoPeca);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

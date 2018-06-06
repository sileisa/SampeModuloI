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
    public class OrdemProducaoKitsController : Controller
    {
        private SampeContext db = new SampeContext();
        public int OPnoMes = 0;

        // GET: OrdemProducaoKits
        public ActionResult Index()
        {
            var ordemProducaoKits = db.OrdemProducaoKits;
            //var ordemProducaoKits = db.OrdemProducaoKits.Include(o => o.Cliente);
            return View(ordemProducaoKits.ToList());
        }

        // GET: OrdemProducaoKits/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdemProducaoKit ordemProducaoKit = db.OrdemProducaoKits.Find(id);
            if (ordemProducaoKit == null)
            {
                return HttpNotFound();
            }
            return View(ordemProducaoKit);
        }

        // GET: OrdemProducaoKits/Create
        public ActionResult Create()
        {
            ViewData["Cliente"] = db.Clientes.ToList();
            var a = db.OrdemProducaoPecas.ToList();
            var x = db.OrdemProducaoPecas.Where(f => f.Produto.Contains("ANEL DE VEDAÇÃO")).ToList();
            var y = db.OrdemProducaoPecas.Where(f => f.Produto.Contains("CHAPÉU")).ToList();
            var z = db.OrdemProducaoPecas.Where(f => f.Produto.Contains("CAPA")).ToList();

            ViewBag.Produto = db.OrdemProducaoPecas.ToList();
            ViewBag.Anel = new SelectList(x, "CodigoIdentificador", "CodigoIdentificador");
            ViewBag.Chapeu = new SelectList(y, "CodigoIdentificador", "CodigoIdentificador"); ;
            ViewBag.Capa = new SelectList(z, "CodigoIdentificador", "CodigoIdentificador"); ;
            ViewBag.ClienteId = new SelectList(db.Clientes, "ClienteId", "NomeCliente");
			ViewBag.Clientes = db.Clientes.ToList();
            return View();
        }

        // POST: OrdemProducaoKits/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoIdentificadorKit,Data,ProdIncio,ProdFim,TotalProduzido,NivelamentoBalanca,Obs,Especificacoes,Operdor,ClienteId")] OrdemProducaoKit ordemProducaoKit, String Capa, String Chapeu, String Anel)
        {
            var a = Request.Form["Especificacao.TipoKit"];
            var b = Request.Form["Especificacao.CorKit"];
            var c = Request.Form["Especificacao.Parafuso"];
            var d = Request.Form["Especificacao.QuantProduzido"];
            var e = Request.Form["ClienteId"];

            var f = Request.Form["ParadaKit.HoraParada"];
            var g = Request.Form["ParadaKit.HoraRetorno"];
            var h = Request.Form["ParadaKit.Motivo"];
            var i = Request.Form["ParadaKit.Observacoes"];

            if (ModelState.IsValid)
            {
                List<KitPeca> kits = new List<KitPeca>();
                KitPeca k = new KitPeca();
                KitPeca k2 = new KitPeca();
                KitPeca k3 = new KitPeca();
                var p1 = db.OrdemProducaoPecas.Find(Capa);
                var p2 = db.OrdemProducaoPecas.Find(Chapeu);
                var p3 = db.OrdemProducaoPecas.Find(Anel);

                k.OrdemProducaoPeca = p1;
                kits.Add(k);
                k2.OrdemProducaoPeca = p2;
                kits.Add(k2);
                k3.OrdemProducaoPeca = p3;
                kits.Add(k3);
                ordemProducaoKit.KitPecas = kits;


                string[] tipo = a.Split(',').ToArray();
                string[] cor = b.Split(',').ToArray();
                Boolean[] parafuso = c.Split(',').Select(Boolean.Parse).ToArray();
                int[] quant = d.Split(',').Select(Int32.Parse).ToArray();
                List<Especificacao> esp = new List<Especificacao>();
                for (int x = 0; x < tipo.Count(); x++)
                {
                    Especificacao e1 = new Especificacao();
                    e1.TipoKit = tipo[x];
                    e1.CorKit = cor[x];
                    e1.Parafuso = parafuso[x];
                    e1.QuantProduzido = quant[x];
                    ordemProducaoKit.calculaProdTotal(quant[x]);
                    esp.Add(e1);
                }
                ordemProducaoKit.Especificacoes = esp;

                string[] hrP = f.Split(',').ToArray();
                string[] hrR = g.Split(',').ToArray();
                string[] mot = h.Split(',').ToArray();
                string[] obs = i.Split(',').ToArray();
                List<ParadaKit> parakit = new List<ParadaKit>();
                for (int y = 0; y < hrP.Count(); y++)
                {
                    ParadaKit p = new ParadaKit();
                    p.HoraParada = hrP[y];
                    p.HoraRetorno = hrR[y];
                    p.Motivo = mot[y];
                    p.Observacoes = obs[y];
                    parakit.Add(p);
                }

                ordemProducaoKit.ParadasKit = parakit;
                var qtdOp = 0;
                var mesAnterior = 0;
                if (db.OrdemProducaoPecas.ToList().Count() > 0)
                {
                    qtdOp = db.OrdemProducaoPecas.ToList().Last().OPnoMes;
                    mesAnterior = db.OrdemProducaoPecas.ToList().Last().Data.Month;
                }
                if (mesAnterior != ordemProducaoKit.Data.Month)
                {
                    ordemProducaoKit.OPnoMes = 1;
                }
                else
                    ordemProducaoKit.OPnoMes = qtdOp + 1;

                ordemProducaoKit.GerarCodigo();
                db.OrdemProducaoKits.Add(ordemProducaoKit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.ClienteId = new SelectList(db.Clientes, "ClienteId", "NomeCliente", ordemProducaoKit.ClienteId);
            return View(ordemProducaoKit);
        }

        // GET: OrdemProducaoKits/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdemProducaoKit ordemProducaoKit = db.OrdemProducaoKits.Find(id);
            if (ordemProducaoKit == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ClienteId = new SelectList(db.Clientes, "ClienteId", "NomeCliente", ordemProducaoKit.ClienteId);
            return View(ordemProducaoKit);
        }

        // POST: OrdemProducaoKits/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoIdentificadorKit,Data,ProdIncio,ProdFim,TotalProduzido,NivelamentoBalanca,Obs,OPnoMes,Operdor,ClienteId")] OrdemProducaoKit ordemProducaoKit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordemProducaoKit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.ClienteId = new SelectList(db.Clientes, "ClienteId", "NomeCliente", ordemProducaoKit.ClienteId);
            return View(ordemProducaoKit);
        }

        // GET: OrdemProducaoKits/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdemProducaoKit ordemProducaoKit = db.OrdemProducaoKits.Find(id);
            if (ordemProducaoKit == null)
            {
                return HttpNotFound();
            }
            return View(ordemProducaoKit);
        }

        // POST: OrdemProducaoKits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            OrdemProducaoKit ordemProducaoKit = db.OrdemProducaoKits.Find(id);
            db.OrdemProducaoKits.Remove(ordemProducaoKit);
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

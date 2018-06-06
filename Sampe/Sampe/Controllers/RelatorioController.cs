using PagedList;
using Rotativa;
using Rotativa.Options;
using Sampe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sampe.Controllers
{
    public class RelatorioController : Controller
    {
        private SampeContext db = new SampeContext();
        // GET: Relatorio
       
        public ActionResult BuscaOS()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BuscaOS(DateTime dt1, DateTime dt2, int? pagina, Boolean? gerarPdf)
        {
            var buscaOS = db.FormularioOrdemServicoes.Where(a => a.Dt >= dt1 && a.Dt <= dt2).ToList();
            ViewBag.dt1 = dt1;
            ViewBag.dt2 = dt2;
            if (gerarPdf == true)
            {
                int pgQtdRegistro = 2;
                int pgNav = (pagina ?? 1);

                return View(buscaOS.ToPagedList(pgNav, pgQtdRegistro));
            }
            else
            {
                int paginaNumero = 1;
                var pdf = new ViewAsPdf
                {
                    ViewName = "OsData",
                    PageSize = Size.A4,
                    IsGrayScale = false,
                    Model = buscaOS.ToPagedList(paginaNumero, buscaOS.Count)
                };
                return pdf;
            }
        }
        public ActionResult BuscaTM()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BuscaTM(DateTime dt1, DateTime dt2, int? pagina, Boolean? gerarPdf, int? id)
        {
            var buscaTM = db.FormularioTrocaMoldes.Where(a => a.DtRetirada >= dt1 && a.DtRetirada <= dt2).ToList();
            ViewBag.dt1 = dt1;
            ViewBag.dt2 = dt2;
            if (gerarPdf == true)
            {
                int pgQtdRegistro = 10;
                int pgNav = (pagina ?? 1);

                return View(buscaTM.ToPagedList(pgNav, pgQtdRegistro));
            }
            else
            {
                int paginaNumero = 1;
                var pdf = new ViewAsPdf
                {
                    ViewName = "TmData",
                    PageSize = Size.A4,
                    IsGrayScale = false,
                    Model = buscaTM.ToPagedList(paginaNumero, buscaTM.Count)
                };
                return pdf;
            }
        }

        public ActionResult BuscaOpPeca()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BuscaOpPeca(DateTime dt1, DateTime dt2, int? pagina, Boolean? gerarPdf, string id)
        {
            var buscaOpPeca = db.OrdemProducaoPecas.Where(a => a.Data >= dt1 && a.Data <= dt2).ToList();
            ViewBag.dt1 = dt1;
            ViewBag.dt2 = dt2;
            if (gerarPdf == true)
            {
                int pgQtdRegistro = 10;
                int pgNav = (pagina ?? 1);

                return View(buscaOpPeca.ToPagedList(pgNav, pgQtdRegistro));
            }
            else
            {
                int paginaNumero = 1;
                var pdf = new ViewAsPdf
                {
                    ViewName = "OpPecaData",
                    PageSize = Size.A4,
                    IsGrayScale = false,
                    Model = buscaOpPeca.ToPagedList(paginaNumero, buscaOpPeca.Count)
                };
                return pdf;
            }
        }

        public ActionResult BuscaOpKit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BuscaOpKit(DateTime dt1, DateTime dt2, int? pagina, Boolean? gerarPdf, string id)
        {
            var buscaOpKit = db.OrdemProducaoKits.Where(a => a.Data >= dt1 && a.Data <= dt2).ToList();
            ViewBag.dt1 = dt1;
            ViewBag.dt2 = dt2;
            if (gerarPdf == true)
            {
                int pgQtdRegistro = 10;
                int pgNav = (pagina ?? 1);

                return View(buscaOpKit.ToPagedList(pgNav, pgQtdRegistro));
            }
            else
            {
                int paginaNumero = 1;
                var pdf = new ViewAsPdf
                {
                    ViewName = "OpKitData",
                    PageSize = Size.A4,
                    IsGrayScale = false,
                    Model = buscaOpKit.ToPagedList(paginaNumero, buscaOpKit.Count)
                };
                return pdf;
            }
        }

        public ActionResult OsMaquina(int? pagina, Boolean? gerarPdf, int id)
        {
            var osMaquina = db.FormularioOrdemServicoes.Where(a => a.MaquinaId == id).ToList();
            if (gerarPdf == true)
            {
                int pgQtdRegistro = 10;
                int pgNav = (pagina ?? 1);

                return View(osMaquina.ToPagedList(pgNav, pgQtdRegistro));
            }
            else
            {
                int paginaNumero = 1;
                var pdf = new ViewAsPdf
                {
                    ViewName = "OpKitData",
                    PageSize = Size.A4,
                    IsGrayScale = false,
                    Model = osMaquina.ToPagedList(paginaNumero, osMaquina.Count)
                };
                return pdf;
            }
        }
    }
}
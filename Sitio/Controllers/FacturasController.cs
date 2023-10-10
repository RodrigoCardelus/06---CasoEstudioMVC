using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


//agregar--------------------------------
using Sitio.Models;
//--------------------------------------


namespace Sitio.Controllers
{
    public class FacturasController : Controller
    {
        // GET: Facturas
        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public ActionResult FormFacturaAlta()
        {
            try
            {
                //muestro la Vista
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();

            }
        }

        [HttpPost]
        public ActionResult FormFacturaAlta(Factura F)
        {
            try
            {
                //determino que usario que genera la factura
                F.Usu = (Usuario)Session["Logueo"];

                //creo la lista de lineas - para ir agregando las lineas
                //(el objeto se crea vacio en la vista, no hay lista
                F.ListaL = new List<LineasFacturas>();

                //guardo en Session para agregar Lineas
                Session["Factura"] = F;

                return RedirectToAction("FormLineasFacturaAlta", "Facturas");
            }
            catch(Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }

        }

        [HttpGet]
        public ActionResult FormLineaFacturaAlta()
        {
            try
            {
                //carga la lista de articulos para mostrar en una lista desplegable en la vista
                List<Articulo> _listaA = new ArticulosBD().ListarArticulo();
                ViewBag.ListarArticulos = new SelectList(_listaA, "Codigo", "Nombre");

                //muestro la vista
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.ListarArticulos = new SelectList(null);
                ViewBag.Mensaje = ex.Message;
                return View();

            }
        }

        [HttpPost]
        public ActionResult FormLineaFacturaAlta(LineasFacturas LF)
        {
            try
            {
                if (LF.CodigoArticulo.Trim().Length > 0)
                {
                    int _Codigo = Convert.ToInt32(LF.CodigoArticulo);
                    LF.Art = new ArticulosBD().BuscarArticulo(_Codigo);

                }

                //Valido la Linea
                LF.Validar();

                //Agrego Linea de Factura en la session
                ((Factura)Session["Factura"]).ListaL.Add(LF);

                //muestro la vista de nuevo, pero que la muestre vacia, provoco el get
                return RedirectToAction("FormLineasFacturaAlta", "Facturas"); 

            }
            catch (Exception ex)
            {
                List<Articulo> _ListaA = new ArticulosBD().ListarArticulo();
                ViewBag.ListarArticulo = new SelectList(_ListaA, "Codigo", "Nombre");
                ViewBag.Mensaje = ex.Message;
                return View();

            }
        }

        [HttpGet]
        public ActionResult GuardarFactura()
        {
            try
            {
                //Obtengo Factura Agregada
                Factura F = ((Factura)Session["Factura"]);

                //Valido el Modelo
                F.Validar();

                //si todo ok
                return RedirectToAction("FormAltaExito", "Facturas");
            }
            catch (Exception ex)
            {
                Session["ErrorFactura"] = ex.Message;
                return RedirectToAction("FormAltaError", "Facturas");

            }
        }

        public ActionResult FormAltaExito()
        { 
            return View();
        }

        public ActionResult FormAltaError()
        {
            ViewBag.Mensaje = Session["ErrorFactura"].ToString();
            return View();
        }

        public ActionResult FormFacturasListar(string FechaFiltro, string UsuarioFiltro)
        {
            try
            {
                List<Factura> _lista = null;

                //obtengo lista de Articulos
                if (Session["Lista"] == null)
                {
                    _lista = new FacturasDB().ListarFacturas();
                    Session["Lista"] = _lista;

                }
                else
                    _lista = (List < Factura >)Session["Lista"];

                //no hay facturas
                if (_lista.Count == 0)
                    throw new Exception("No hay facturas para mostrar");

                //filtros o no
                if(!String.IsNullOrEmpty(FechaFiltro))
                {
                    _lista = (from unF in _lista
                              where unF.Fecha.Date == Convert.ToDateTime(FechaFiltro).Date
                              select unF).ToList();
                }

                if (!String.IsNullOrEmpty(UsuarioFiltro))
                {
                    _lista = (from unF in _lista
                              where unF.Usu.UsuLog == UsuarioFiltro.Trim()
                              select unF).ToList();
                }

                //Retorno resultado
                return View(_lista);


            }
            catch(Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Factura>());
            }
        }

        public ActionResult FormFacturaConsultar(int Nro)
        {
            try
            {
                //obtengo el articulos
                List<LineasFacturas> _listaLineas = new LineasFacturasDB().ListarLineas(Nro);
                if (_listaLineas != null)
                    return View(_listaLineas);
                else
                    throw new Exception("Error - No se encontraron las Lineas de las Facturas");


            }
            catch(Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<LineasFacturas>());

            }


        }



    }

}
 
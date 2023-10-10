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
    public class ArticulosController : Controller
    {
        // GET: Articulos
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult FormArticulosListar()
        {
            try
            {
                //obtengo lista de Articulos
                List<Articulo> _lista = new ArticulosBD().ListarArticulo();
                if (_lista.Count > 1)
                    return View(_lista);
                else
                    throw new Exception("No hay Articulos Para Mostrar");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Articulo>());

            }
        }

        [HttpGet]
        public ActionResult FormArticuloNuevo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FormArticuloNuevo(Articulo A)
        {
            try
            {
                //valido objeto correctamente
                A.Validar();

                //intento agregar articulo en la bd
                new ArticulosBD().AgregarArticulo(A);

                //no hubo error, alta correcta
                return RedirectToAction("FormArticuloListar", "Articulos");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();

            }


        }

        [HttpGet]
        public ActionResult FormArticuloModificar(int Codigo)
        {
            try
            {
                Articulo _A = new ArticulosBD().BuscarArticulo(Codigo);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No Existe el Articulo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Articulo());

            }
        }


        [HttpPost]
        public ActionResult FormArticuloModificar(Articulo A)
        {
            try
            {
                //valido objeto correctamente
                A.Validar();

                //intento agregar articulo en la bd
                new ArticulosBD().ModificarArticulo(A);

                return View(new Articulo());
                //sino, puedo directo redireccionar a la vista de listado para que se vean los cambios
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();

            }
        }



        [HttpGet]
        public ActionResult FormArticuloConsultar(int Codigo)
        {

            try
            {
                Articulo _A = new ArticulosBD().BuscarArticulo(Codigo);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No Existe el Articulo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Articulo());

            }
        }


        [HttpGet]
        public ActionResult FormArticuloEliminar(int Codigo)
        {
            try
            {
                Articulo _A = new ArticulosBD().BuscarArticulo(Codigo);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No Existe el Articulo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Articulo());

            }
        }

        [HttpPost]
        public ActionResult FormArticulosEliminar(Articulo A)
        {

            try
            {

                //intento agregar articulo en la bd
                new ArticulosBD().EliminarArticulo(A);
                ViewBag.Mensaje = "Eliminacion Exitosa";
                return View(new Articulo());

            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();

            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Sitio.Models;

namespace Sitio.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Session["Logueo"] = null;
            return View();

        }

        public ActionResult Principal()
        {
            //limpio variables de Listados
            Session["Lista"] = null;
            return View();

        }

        [HttpGet]
        public ActionResult FormLogueo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FormLogueo(Usuario U)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    new UsuariosDB().Logueo(U);

                    Session["Logueo"] = U;
                    return RedirectToAction("Principal", "Home");

                }
                else
                    return View();

            }

            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();

            }
        }
    }
}
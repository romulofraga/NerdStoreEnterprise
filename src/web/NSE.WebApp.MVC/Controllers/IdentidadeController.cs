using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Controllers
{
    public class IdentidadeController : Controller
    {
        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return View(usuarioRegistro);

            // API REGISTRO

            if (false) return View(usuarioRegistro);

            // REALIZAR LOGIN NO APP

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return View(usuarioLogin);

            // API LOGIN

            if (false) return View(usuarioLogin);

            // REALIZAR LOGIN NO APP

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            //LIMPAR O COOKIE DE AUTH
            return RedirectToAction("Index", "Home");
        }

    }
}

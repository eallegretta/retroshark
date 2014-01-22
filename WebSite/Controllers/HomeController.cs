using System.Web.Mvc;
using RetroShark.Application.Backend.Services;
using RetroShark.Application.Models;

namespace RetroShark.Application.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public HomeController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _authenticationService.Authenticate(model.User, false);

            return Redirect(model.ReturnUrl);
        }
    }
}

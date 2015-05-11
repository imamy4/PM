using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using УправлениеПроектами.Models.КлассыДляФормВвода;

namespace УправлениеПроектами.Controllers
{
    public class LoginController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new ДанныеВходаВСистему());
        }

        [HttpPost]
        public ActionResult Index(ДанныеВходаВСистему данныеВхода)
        {
            var пользователь = Аутентификация.Войти(данныеВхода.Email, данныеВхода.Пароль, данныеВхода.ПостояннаяАвторизация);
            if (пользователь != null)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState["Email"].Errors.Add("Пользователь с таким логином и паролем не найден.");
            return View(данныеВхода);
        }

        public ActionResult Logout()
        {
            Аутентификация.Выйти();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Ajax() 
        {
            return View(new ДанныеВходаВСистему()); 
        }

        [HttpPost]
        public ActionResult Ajax(ДанныеВходаВСистему данныеВхода) 
        { 
            //if (ModelState.IsValid) 
            //{
                var пользователь = Аутентификация.Войти(данныеВхода.Email, данныеВхода.Пароль, данныеВхода.ПостояннаяАвторизация);
                if (пользователь != null) 
                { 
                    //return RedirectToAction("Index", "Home"); 
                    return View("_Ok");
                }
                ModelState["Email"].Errors.Add("Пользователь с таким логином и паролем не найден."); 
            //} 
            return View(данныеВхода); 
        }
    }
}

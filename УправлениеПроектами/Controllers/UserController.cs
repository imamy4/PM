using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using МенеджерБД;
using МенеджерБД.Домен;
using УправлениеПроектами.Models.КлассыДляФормВвода;

namespace УправлениеПроектами.Controllers
{
    public class UserController : BaseController
    {
        int idРолиПользователь = 2;
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            ПользовательРегистрации новыйПользователь = new ПользовательРегистрации();
            return View(новыйПользователь);
        }

        [HttpPost]
        public ActionResult Register(ПользовательРегистрации пользователь)
        {
            bool отменитьСохранение = false;
            
            //if (userView.Captcha != "1234")
            //{
            //    ModelState.AddModelError("Captcha", "Текст с картинки введен неверно");
            //}
            if (!string.IsNullOrWhiteSpace(пользователь.Email))
            {
                var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.Compiled);
                var match = regex.Match(пользователь.Email);

                if (!match.Success || match.Length != пользователь.Email.Length)
                {
                    ModelState.AddModelError("Email", "Неверный формат Email");
                    отменитьСохранение = true;
                }
                else
                {
                    var пользовательСРавнымEmail = МенеджерБД.СуществуетЗапись<Пользователь> (p => string.Compare(p.Email, пользователь.Email) == 0);
                    if (пользовательСРавнымEmail)
                    {
                        ModelState.AddModelError("Email", "Пользователь с таким email уже зарегистрирован");
                        отменитьСохранение = true;
                    }
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Не введен Email");
                отменитьСохранение = true;
            }

            if (string.IsNullOrEmpty(пользователь.Пароль) || пользователь.Пароль.Length < 6)
            {
                ModelState.AddModelError("Password", "Пароль должен быть больше 6 символов");
                отменитьСохранение = true;
            }
            else 
            {
                if (string.IsNullOrEmpty(пользователь.ПодтверждениеПароля) || пользователь.Пароль != пользователь.ПодтверждениеПароля)
                {
                    ModelState.AddModelError("ConfirmPassword", "Потверждение пароля введено неверно");
                    отменитьСохранение = true;
                }
            }

            if (string.IsNullOrWhiteSpace(пользователь.Имя))
            {
                ModelState.AddModelError("Name", "Незаполнено поле имя");
                отменитьСохранение = true;
            }

            if (string.IsNullOrWhiteSpace(пользователь.Фамилия))
            {
                ModelState.AddModelError("Surname", "Незаполнено поле фамилия");
                отменитьСохранение = true;
            }

            if (!отменитьСохранение)
            {
                Пользователь новыйПользователь = пользователь.ПеревестиВСущностьБД();

                МенеджерБД.СоздатьЗаписьБД<Пользователь>(новыйПользователь);
                //МенеджерБД.ДобавитьРольПользователю(новыйПользователь, idРолиПользователь);

                return RedirectToAction("Success");
            }

            return View(пользователь);
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}

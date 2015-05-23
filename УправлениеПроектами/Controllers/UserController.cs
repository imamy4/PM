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

        /// <summary>
        /// Возвращает список требований назначенных на текущего пользователя 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAssignmentedUserStories()
        {
            List<Требование> требования = new List<Требование>();

            if (ТекущийПользователь != null)
            {
                требования.AddRange(ТекущийПользователь.НазначенныеТреования());
            }

            return this.Json(требования
                                .Select(x => new
                                {
                                    id = x.Id,
                                    name = x.Название,
                                    importance = x.Важность,
                                    estimate = x.Оценка,
                                    executorId = x.Исполнитель() == null ? 0 : x.Исполнитель().Id,
                                    executorName = x.Исполнитель() == null ? string.Empty : string.Format("{0} {1}", x.Исполнитель().Имя, x.Исполнитель().Фамилия),
                                    statusId = x.Статус == null ? 0 : x.Статус.Id,
                                    statusName = x.Статус == null ? string.Empty : x.Статус.Название,
                                    statusIsResolved = x.Статус == null ? false : x.Статус.Решенное,
                                    author_name = x.Автор.Имя,
                                    author_surname = x.Автор.Фамилия,
                                    categoryId = x.Категория == null ? 0 : x.Категория.Id,
                                    categoryName = x.Категория == null ? string.Empty : x.Категория.Название
                                }),
                JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Возвращает список требований созданных текущим пользователем
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCreatedUserStories()
        {
            List<Требование> требования = new List<Требование>();

            if (ТекущийПользователь != null)
            {
                требования.AddRange(ТекущийПользователь.СозданныеТребования);
            }
            
            return this.Json(требования
                                .Select(x => new
                                {
                                    id = x.Id,
                                    name = x.Название,
                                    importance = x.Важность,
                                    estimate = x.Оценка,
                                    executorId = x.Исполнитель() == null ? 0 : x.Исполнитель().Id,
                                    executorName = x.Исполнитель() == null ? string.Empty : string.Format("{0} {1}", x.Исполнитель().Имя, x.Исполнитель().Фамилия),
                                    statusId = x.Статус == null ? 0 : x.Статус.Id,
                                    statusName = x.Статус == null ? string.Empty : x.Статус.Название,
                                    statusIsResolved = x.Статус == null ? false : x.Статус.Решенное,
                                    author_name = x.Автор.Имя,
                                    author_surname = x.Автор.Фамилия,
                                    categoryId = x.Категория == null ? 0 : x.Категория.Id,
                                    categoryName = x.Категория == null ? string.Empty : x.Категория.Название
                                }),
                JsonRequestBehavior.AllowGet);
        }
    }
}

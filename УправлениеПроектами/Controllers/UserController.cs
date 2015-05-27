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
                требования.AddRange(ТекущийПользователь.НазначенныеТребования().ОткрытыеТребования());
            }

            return this.Json(требования
                                .Select(x => new
                                {
                                    id = x.Id,
                                    name = x.Название,
                                    projectId = x.Проект.Id,
                                    projectName= x.Проект.Название,    
                                    importance = x.Важность,
                                    estimate = x.Оценка,
                                    spentTime  = x.Активности.Sum(активность => активность.ЗатраченноеВремя),
                                    executorId = x.Исполнитель() == null ? 0 : x.Исполнитель().Id,
                                    executorName = x.Исполнитель() == null ? string.Empty : x.Исполнитель().ФИО,
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
                требования.AddRange(ТекущийПользователь.СозданныеТребования.ОткрытыеТребования());
            }
            
            return this.Json(требования
                                .Select(x => new
                                {
                                    id = x.Id,
                                    name = x.Название,
                                    projectId = x.Проект.Id,
                                    projectName = x.Проект.Название,
                                    importance = x.Важность,
                                    estimate = x.Оценка,
                                    spentTime = x.Активности.Sum(активность => активность.ЗатраченноеВремя),
                                    executorId = x.Исполнитель() == null ? 0 : x.Исполнитель().Id,
                                    executorName = x.Исполнитель() == null ? string.Empty : x.Исполнитель().ФИО,
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

        public JsonResult GetLastMonthActivity()
        {
            if (ТекущийПользователь == null)
            {
                return this.Json(new List<string>(), JsonRequestBehavior.AllowGet);
            }

            IEnumerable<Активность> активности = ТекущийПользователь.Активности.Where(активность => активность.ДатаКонца > new  DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));

            return this.Json(активности.Select(активность =>
                new
                {
                    projectName = активность.Требование.Проект.Название,
                    userStoryName = активность.Требование.Название,
                    userName = ТекущийПользователь.ФИО,
                    dateStart = активность.ДатаНачала.ToString("o"),
                    dateFinish = активность.ДатаКонца.ToString("o"),
                    activityTime = активность.ЗатраченноеВремя
                }),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCurrentActivity()
        {
            Активность активность = ТекущийПользователь.ТекущаяАктивность();
            return this.Json(new
            {
                success = активность != null,
                userStory = активность != null
                ? new
                {
                    id = активность != null ? активность.Требование.Id.ToString() : string.Empty,
                    name = активность != null ? активность.Требование.Название : string.Empty,
                    dateStart = активность != null ? активность.ДатаНачала.ToString("o") : string.Empty,
                }
                : null,
                user = new { userName = ТекущийПользователь != null ? ТекущийПользователь.ФИО : string.Empty }
            },
            JsonRequestBehavior.AllowGet);
        }
    
        public JsonResult CompleteCurrentActivity()
        {
            bool success = false;

            Активность активность = ТекущийПользователь.ТекущаяАктивность();
            if (активность != null)
            {
                активность.ДатаКонца = DateTime.UtcNow;
                success = МенеджерБД.ОбновитьЗаписьБД<Активность>(активность);
            }

            return this.Json(new { success }, JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult StrartActivity(int userStoryId)
        {
            bool success = false;
            Требование требование = МенеджерБД.ПолучитьЗаписьБДПоId<Требование>(userStoryId);

            if (требование != null && ТекущийПользователь.ЯвляетсяУчастникомПроекта(требование.Проект))
            {
                success = ТекущийПользователь.ВзятьВРаботу(требование);
            }

            return this.Json(new { success }, JsonRequestBehavior.AllowGet);
        }
 
    }
}

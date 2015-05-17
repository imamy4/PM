using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using МенеджерБД.Домен;
using УправлениеПроектами.Models.КлассыДляФормВвода;

namespace УправлениеПроектами.Controllers
{
    public class UserStoriesController : BaseController
    {
        /// <summary>
        /// Выводит все требования
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(МенеджерБД.Записи<Требование>());
        }

        /// <summary>
        /// Основная страница конкретного требования
        /// </summary>
        /// <param name="id">Id требования</param>
        /// <returns></returns>
        public ActionResult UserStory(int id)
        {
            return View(МенеджерБД.ПолучитьЗаписьБДПоId<Требование>(id));
        }

        /// <summary>
        /// Страница создания нового требование
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ТребованиеДляФормы новоеТребование = new ТребованиеДляФормы();
            return View(новоеТребование);
        }

        /// <summary>
        /// Страница создания нового требования, с проверкой на валидность введенных значений
        /// </summary>
        /// <param name="требование"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ТребованиеДляФормы требование)
        {
            bool отменитьСохранение = false;

            // проверка на наличие проекта
            var проект = МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(требование.IdПроект);
            if (проект == null)
            {
                ModelState.AddModelError("IdПроект", "Необходимо выбрать проект");
                отменитьСохранение = true;
            }

            if (string.IsNullOrWhiteSpace(требование.Название))
            {
                ModelState.AddModelError("Название", "Необходимо заполнить название");
                отменитьСохранение = true;
            }

            if (string.IsNullOrWhiteSpace(требование.Описание))
            {
                ModelState.AddModelError("Описание", "Необходимо заполнить описание");
                отменитьСохранение = true;
            }

            if (требование.Важность < 0 || требование.Важность > 250)
            {
                ModelState.AddModelError("Важность", "Важность должна быть от 0 до 250");
                отменитьСохранение = true;
            }

            if (требование.Оценка < 0)
            {
                ModelState.AddModelError("Оценка", "Оценка не может быть отрицательным числом");
                отменитьСохранение = true;
            }

            if (string.IsNullOrWhiteSpace(требование.Описание))
            {
                ModelState.AddModelError("Описание", "Необходимо заполнить Описание");
                отменитьСохранение = true;
            }

            if (!отменитьСохранение)
            {
                требование.IdАвтор = ТекущийПользователь.Id;
                Требование новоеТребование = требование.ПеревестиВСущностьБД();
                
                МенеджерБД.СоздатьЗаписьБД<Требование>(новоеТребование);

                return RedirectToAction("Success", new { id = новоеТребование.Id });
            }

            return View(требование);
        }

        /// <summary>
        /// Страница с уведомлением об удачном создании требования
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Success(int id)
        {
            return View(МенеджерБД.ПолучитьЗаписьБДПоId<Требование>(id));
        }

        public ActionResult Delete(int id)
        {
            МенеджерБД.УдалитьЗаписьБД<Требование>(id);
            return RedirectToAction("Index");
        }

        public JsonResult GetProjects()
        {
            IEnumerable<Проект> проекты = МенеджерБД.АктуальныеПроекты();

            return this.Json(проекты.Select(x => new { Id = x.Id, Name = x.Название }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCategories(int? projectId)
        {
            IEnumerable<КатегорияТребования> категории = МенеджерБД.Записи<КатегорияТребования>(x => projectId.HasValue && x.Проект.Id == projectId.Value);

            return this.Json(категории.Select(x => new { Id = x.Id, Name = x.Название }), JsonRequestBehavior.AllowGet);
        }
    }
}

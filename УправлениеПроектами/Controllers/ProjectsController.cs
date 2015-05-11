using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using МенеджерБД.Домен;
using УправлениеПроектами.Models.КлассыДляФормВвода;

namespace УправлениеПроектами.Controllers
{
    public class ProjectsController : BaseController
    {
        /// <summary>
        /// Выводит актуальные проекты.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(МенеджерБД.АктуальныеПроекты());
        }

        /// <summary>
        /// Основная страница конкретного проекта
        /// </summary>
        /// <param name="id">Id проекта</param>
        /// <returns></returns>
        public ActionResult Project(int id)
        {
            return View(МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(id));
        }

        /// <summary>
        /// Страница создания нового проекта
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ПроектДляФормы новыйПроект = new ПроектДляФормы();
            return View(новыйПроект);
        }

        /// <summary>
        /// Страница создания нового проекта, с проверкой на валидность введенных значений
        /// </summary>
        /// <param name="проект"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ПроектДляФормы проект)
        {
            bool отменитьСохранение = false;

            if (string.IsNullOrWhiteSpace(проект.Название))
            {
                ModelState.AddModelError("Название", "Необходимо заполнить название");
                отменитьСохранение = true;
            }

            if (проект.ДатаНачала >= проект.ДатаКонца)
            {
                ModelState.AddModelError("ДатаКонца", "Дата окончания не может быть меньше даты начала проекта");
                отменитьСохранение = true;
            }

            if (!отменитьСохранение)
            {
                Проект новыйПроект = проект.ПеревестиВСущностьБД();

                МенеджерБД.СоздатьЗаписьБД<Проект>(новыйПроект);

                return RedirectToAction("Success", new { id = новыйПроект.Id });
            }

            return View(проект);
        }

        /// <summary>
        /// Страница с уведомлением об удачном создании проекта
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Success(int id)
        {
            return View(МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(id));
        }

    }
}

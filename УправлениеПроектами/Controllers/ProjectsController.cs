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
        public ActionResult Index()
        {
            return View(МенеджерБД.Записи<Проект>());
        }

        public ActionResult Project(int id)
        {
            return View(МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(id));
        }

        public ActionResult UserBar()
        {
            return View(ТекущийПользователь);
        }

        public ActionResult Create()
        {
            ПроектДляФормы новыйПроект = new ПроектДляФормы();
            return View(новыйПроект);
        }

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

        public ActionResult Success(int id)
        {
            return View(МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(id));
        }

    }
}

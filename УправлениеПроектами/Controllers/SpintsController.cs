using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using МенеджерБД.Домен;
using УправлениеПроектами.Models.КлассыДляФормВвода;

namespace УправлениеПроектами.Controllers
{
    public class SprintsController : BaseController
    {
        /// <summary>
        /// Выводит актуальные спринты
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(МенеджерБД.Записи<Спринт>().Where(проект => проект.ДатаНачала <= DateTime.Now && проект.ДатаКонца >= DateTime.Now));
        }

        /// <summary>
        /// Основная страница конкретного спринта
        /// </summary>
        /// <param name="id">Id проекта</param>
        /// <returns></returns>
        public ActionResult Sprint(int id)
        {
            return View(МенеджерБД.ПолучитьЗаписьБДПоId<Спринт>(id));
        }

        /// <summary>
        /// Страница создания нового спринта
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            СпринтДляФормы новыйСпринт = new СпринтДляФормы();
            return View(новыйСпринт);
        }

        /// <summary>
        /// Страница создания нового спринта, с проверкой на валидность введенных значений
        /// </summary>
        /// <param name="спринт"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(СпринтДляФормы спринт)
        {
            bool отменитьСохранение = false;

            // проверка на наличие проекта
            var проект = МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(спринт.IdПроекта);
            if (проект == null)
            {
                ModelState.AddModelError("IdПроекта", "Необходимо выбрать проект");
                отменитьСохранение = true;
            }

            if (string.IsNullOrWhiteSpace(спринт.Название))
            {
                ModelState.AddModelError("Название", "Необходимо заполнить название");
                отменитьСохранение = true;
            }

            if (спринт.ДатаНачала >= спринт.ДатаКонца)
            {
                ModelState.AddModelError("ДатаКонца", "Дата окончания не может быть меньше даты начала проекта");
                отменитьСохранение = true;
            }

            // проверка на то, что границы спринта входят в границы проекта
            if (проект != null && проект.ДатаНачала > спринт.ДатаНачала)
            {
                ModelState.AddModelError("ДатаНачала", "Спринт не может начаться раньше проекта");
                отменитьСохранение = true;
            }

            if (проект != null && проект.ДатаКонца < спринт.ДатаКонца)
            {
                ModelState.AddModelError("ДатаКонца", "Спринт не может заканчиваться после окончания проекта");
                отменитьСохранение = true;
            }

            if (!отменитьСохранение)
            {
                Спринт новыйСпринт = спринт.ПеревестиВСущностьБД();

                МенеджерБД.СоздатьЗаписьБД<Спринт>(новыйСпринт);

                return RedirectToAction("Success", new { id = новыйСпринт.Id });
            }

            return View(спринт);
        }

        /// <summary>
        /// Страница с уведомлением об удачном создании спринта
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Success(int id)
        {
            return View(МенеджерБД.ПолучитьЗаписьБДПоId<Спринт>(id));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using МенеджерБД.Домен;
using УправлениеПроектами.Helpers;
using УправлениеПроектами.Models.КлассыДляФормВвода;

namespace УправлениеПроектами.Controllers
{
    public class SprintsController : BaseEntityController<Спринт>
    {
        #region Реализация BaseEntityController
        
        [HttpPost]
        public ActionResult Create(СпринтДляФормы модельЗадачи)
        {
            return Create((БазоваяМодельСущностиБД<Спринт>)модельЗадачи);
        }

        protected override IEnumerable<Спринт> ПолучитьСущности()
        {
            return МенеджерБД.АктуальныеСпринты();
        }

        protected override БазоваяМодельСущностиБД<Спринт> ПолучитьЭкземплярМодели()
        {
            return new СпринтДляФормы();
        }

        protected override bool ПроверитьМодельНаВалидность(БазоваяМодельСущностиБД<Спринт> модельСущности)
        {
            СпринтДляФормы спринт = модельСущности as СпринтДляФормы;

            bool отменитьСохранение = false;

            // проверка на наличие проекта
            var проект = МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(спринт.IdПроект);
            if (проект == null)
            {
                ModelState.AddModelError("IdПроект", "Необходимо выбрать проект");
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

            return !отменитьСохранение;
        }

        protected override Спринт ПолучитьСущностьДляСоздания()
        {
            Спринт спринт = new Спринт();

            int idПроекта = Конвертер.ВЧисло32(Request["projectId"]);
            if (idПроекта != 0)
            {
                спринт.Проект = new Проект() { Id = idПроекта };
            }

            спринт.Название = Request["name"];

            спринт.ДатаНачала = Convert.ToDateTime(Request["dateStart"]);
            спринт.ДатаКонца = Convert.ToDateTime(Request["dateFinish"]);

            return спринт;
        }

        protected override Спринт ПолучитьСущностьДляОбновления()
        {
            int id = Конвертер.ВЧисло32(Request["id"]);
            Спринт спринт = МенеджерБД.ПолучитьЗаписьБДПоId<Спринт>(id);

            if (спринт != null)
            {
                int idПроекта = Конвертер.ВЧисло32(Request["projectId"]);
                if (idПроекта != 0)
                {
                    спринт.Проект = new Проект() { Id = idПроекта };
                }
                if (Request["name"] != null)
                {
                    спринт.Название = Request["name"];
                }
                if (Request["dateStart"] != null)
                {
                    спринт.ДатаНачала = Convert.ToDateTime(Request["dateStart"]);
                }
                if (Request["dateFinish"] != null)
                {
                    спринт.ДатаКонца = Convert.ToDateTime(Request["dateFinish"]);
                }
            }

            return спринт;
        }

        #endregion

        public ActionResult Desktop(int id)
        {
            Спринт спринт = МенеджерБД.ПолучитьЗаписьБДПоId<Спринт>(id);

            return View(спринт);
        }

        public JsonResult GetUserStories(int id)
        {
            Спринт спринт = МенеджерБД.ПолучитьЗаписьБДПоId<Спринт>(id);
            ISet<Требование> требования = спринт != null ? спринт.Требования : new HashSet<Требование>();

            return this.Json(требования
                       .Select(x => new
                       {
                           id = x.Id,
                           name = x.Название,
                           importance = x.Важность,
                           estimate = x.Оценка,
                           executorId = x.Исполнитель() == null ? 0 : x.Исполнитель().Id,
                           executorName = x.Исполнитель() == null ? string.Empty : string.Format("{0} {1}", x.Исполнитель().Имя, x.Исполнитель().Фамилия),
                           author_name = x.Автор.Имя,
                           author_surname = x.Автор.Фамилия,
                           categoryId = x.Категория == null ? 0 : x.Категория.Id,
                           categoryName = x.Категория == null ? "" : x.Категория.Название
                       }),
        JsonRequestBehavior.AllowGet);
        }
    }
}

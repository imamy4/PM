using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using МенеджерБД.Домен;
using УправлениеПроектами.Helpers;
using УправлениеПроектами.Models.КлассыДляФормВвода;

namespace УправлениеПроектами.Controllers
{
    public class UserStoriesController : BaseEntityController<Требование>
    {
        #region Реализация BaseEntityController

        [HttpPost]
        public ActionResult Create(ТребованиеДляФормы модельЗадачи)
        {
            return Create((БазоваяМодельСущностиБД<Требование>)модельЗадачи);
        }

        protected override IEnumerable<Требование> ПолучитьСущности()
        {
            return МенеджерБД.АктуальныеТребования();
        }

        protected override БазоваяМодельСущностиБД<Требование> ПолучитьЭкземплярМодели()
        {
            return new ТребованиеДляФормы() { IdАвтор = ТекущийПользователь.Id };
        }

        protected override bool ПроверитьМодельНаВалидность(БазоваяМодельСущностиБД<Требование> модельСущности)
        {
            ТребованиеДляФормы требование = модельСущности as ТребованиеДляФормы;

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

            return !отменитьСохранение;
        }

        protected override Требование ПолучитьСущностьДляСоздания()
        {
            Требование требование = new Требование();
            требование.Автор = ТекущийПользователь;

            требование.Название = Request["name"];
            требование.Описание = Request["description"];

            требование.Важность = Конвертер.ВЧисло32(Request["importance"]);
            требование.Оценка = Конвертер.ВЧисло32(Request["estimate"]);

            требование.Проект = new Проект { Id = Конвертер.ВЧисло32(Request["projectId"]) };
            int idКатегории = Конвертер.ВЧисло32(Request["categoryId"]);
            if (idКатегории != 0)
            {
                требование.Категория = new КатегорияТребования { Id = idКатегории };
            }

            return требование;
        }

        protected override Требование ПолучитьСущностьДляОбновления()
        {
            int id = Конвертер.ВЧисло32(Request["id"]);
            Требование требование = МенеджерБД.ПолучитьЗаписьБДПоId<Требование>(id);

            if (требование != null)
            {
                if (Request["name"] != null)
                {
                    требование.Название = Request["name"];
                }
                if (Request["description"] != null)
                {
                    требование.Описание = Request["description"];
                }
                if (Request["importance"] != null)
                {
                    требование.Важность = Конвертер.ВЧисло32(Request["importance"]);
                }
                if (Request["estimate"] != null)
                {
                    требование.Оценка = Конвертер.ВЧисло32(Request["estimate"]);
                }
                if (Request["categoryId"] != null)
                {
                    int idКатегории = Конвертер.ВЧисло32(Request["categoryId"], -1);
                    if (idКатегории != -1)
                    {
                        требование.Категория = idКатегории != 0
                            ? new КатегорияТребования { Id = idКатегории }
                            : null;
                    }
                }
                if (Request["sprintId"] != null)
                {
                    int idСпринта = Конвертер.ВЧисло32(Request["sprintId"], -1);
                    if (idСпринта != -1)
                    {
                        требование.Спринт = idСпринта != 0
                            ? new Спринт { Id = idСпринта }
                            : null;
                    }
                }
            }

            return требование;
        }

        #endregion

        /// <summary>
        /// Список требований для UI создания сущности
        /// </summary>
        /// <returns></returns>
        public JsonResult GetProjects()
        {
            IEnumerable<Проект> проекты = МенеджерБД.АктуальныеПроекты();

            return this.Json(проекты.Select(x => new { Id = x.Id, Name = x.Название }), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Список категоий для UI содания сущности
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public JsonResult GetCategories(int? projectId, bool includeEmpty = false)
        {
            IEnumerable<КатегорияТребования> категории = МенеджерБД.Записи<КатегорияТребования>(x => projectId.HasValue && x.Проект.Id == projectId.Value);
            if (includeEmpty)
            {
                категории = new List<КатегорияТребования>(категории);
                ((List<КатегорияТребования>)категории).Add(new КатегорияТребования() { Id = 0, Название = "-" });
            }

            return this.Json(категории.Select(x => new { Id = x.Id, Name = x.Название }), JsonRequestBehavior.AllowGet);
        }
    }
}

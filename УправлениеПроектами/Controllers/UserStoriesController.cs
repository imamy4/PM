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
        public JsonResult GetCategories(int? projectId)
        {
            IEnumerable<КатегорияТребования> категории = МенеджерБД.Записи<КатегорияТребования>(x => projectId.HasValue && x.Проект.Id == projectId.Value);

            return this.Json(категории.Select(x => new { Id = x.Id, Name = x.Название }), JsonRequestBehavior.AllowGet);
        }
    }
}

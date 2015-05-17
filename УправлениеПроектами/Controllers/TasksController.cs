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
    public class TasksController : BaseEntityController<Задача>
    {
        #region Реализация BaseEntityController

        [HttpPost]
        public ActionResult Create(ЗадачаДляФормы модельЗадачи)
        {
            return Create((БазоваяМодельСущностиБД<Задача>)модельЗадачи);
        }

        protected override IEnumerable<Задача> ПолучитьСущности()
        {
            return МенеджерБД.Записи<Задача>();
        }

        protected override БазоваяМодельСущностиБД<Задача> ПолучитьЭкземплярМодели()
        {
            return new ЗадачаДляФормы() { IdАвтор = ТекущийПользователь.Id };
        }

        protected override bool ПроверитьМодельНаВалидность(БазоваяМодельСущностиБД<Задача> модельСущности)
        {
            ЗадачаДляФормы задача = модельСущности as ЗадачаДляФормы;

            bool отменитьСохранение = false;

            // проверка на наличие требования
            var требование = МенеджерБД.ПолучитьЗаписьБДПоId<Требование>(задача.IdТребование);
            if (требование == null)
            {
                ModelState.AddModelError("IdIdТребование", "Необходимо выбрать требование");
                отменитьСохранение = true;
            }

            if (string.IsNullOrWhiteSpace(задача.Название))
            {
                ModelState.AddModelError("Название", "Необходимо заполнить название");
                отменитьСохранение = true;
            }

            if (string.IsNullOrWhiteSpace(задача.Описание))
            {
                ModelState.AddModelError("Описание", "Необходимо заполнить описание");
                отменитьСохранение = true;
            }

            return !отменитьСохранение;
        }

        #endregion

        /// <summary>
        /// Список требований для UI создания сущности
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserStories()
        {
            IEnumerable<Требование> требования = МенеджерБД.АктуальныеТребования();

            return this.Json(требования.Select(x => new { Id = x.Id, Name = x.Название }), JsonRequestBehavior.AllowGet);
        }
    }
}

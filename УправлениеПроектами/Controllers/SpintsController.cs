using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using МенеджерБД.Домен;
using УправлениеПроектами.Models.КлассыДляФормВвода;

namespace УправлениеПроектами.Controllers
{
    public class SprintsController : BaseEntityController<Спринт>
    {
        #region Реализация BaseEntityController

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

        #endregion
    }
}

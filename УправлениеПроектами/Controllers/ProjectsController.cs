using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using МенеджерБД.Домен;
using УправлениеПроектами.Models.КлассыДляФормВвода;

namespace УправлениеПроектами.Controllers
{
    public class ProjectsController : BaseEntityController<Проект>
    {
        #region Реализация BaseEntityController
      
        [HttpPost]
        public ActionResult Create(ПроектДляФормы модельЗадачи)
        {
            return Create((БазоваяМодельСущностиБД<Проект>)модельЗадачи);
        }
        
        protected override IEnumerable<Проект> ПолучитьСущности()
        {
            return МенеджерБД.АктуальныеПроекты();
        }

        protected override БазоваяМодельСущностиБД<Проект> ПолучитьЭкземплярМодели()
        {
            return new ПроектДляФормы();
        }

        protected override bool ПроверитьМодельНаВалидность(БазоваяМодельСущностиБД<Проект> модельСущности)
        {
            ПроектДляФормы проект = модельСущности as ПроектДляФормы;

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

            return !отменитьСохранение;
        }

        #endregion
    }
}

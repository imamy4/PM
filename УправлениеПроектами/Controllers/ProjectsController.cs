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
            return МенеджерБД.АктуальныеПроекты()
                            .Where(проект => ТекущийПользователь.ЯвляетсяУчастникомПроекта(проект));
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

        /// <summary>
        /// Рабочая страница проекта
        /// </summary>
        /// <param name="id">Id проекта</param>
        /// <returns></returns>
        public ActionResult Desktop(int id)
        {
            if (!ТекущийПользователь.ЯвляетсяУчастникомПроекта(id))
            {
                return View("_AuthError");
            }

            return View(МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(id));
        }

        public JsonResult GetBacklog(int projectId)
        {
            IEnumerable<Требование> бэклог = new List<Требование>();

            if (ТекущийПользователь.ЯвляетсяУчастникомПроекта(projectId))
            {
                бэклог = МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(projectId).Требования
                    .Where(x => x.Спринт == null).OrderBy(x => -x.Важность);
            }

            return this.Json(бэклог
                                .Select(x => new 
                                { 
                                    id = x.Id,
                                    name = x.Название, 
                                    importance = x.Важность, 
                                    estimate = x.Оценка,
                                    author_name = x.Автор.Имя,
                                    author_surname = x.Автор.Фамилия,
                                    categoryId = x.Категория == null ? 0 : x.Категория.Id,
                                    categoryName = x.Категория == null ? "" : x.Категория.Название
                                }),
                 JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCurrentSprints(int projectId)
        {
            IEnumerable<Спринт> спринты = new List<Спринт>();

            if (ТекущийПользователь.ЯвляетсяУчастникомПроекта(projectId))
            {

                спринты = МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(projectId).Спринты
                    .Where(спринт => спринт.ДатаНачала <= DateTime.Now && спринт.ДатаКонца >= DateTime.Now)
                    .OrderBy(x => x.ДатаКонца);
            }

            return this.Json(спринты
                                .Select(x => new
                                {
                                    id = x.Id,
                                    name = x.Название,
                                    dateStart = x.ДатаНачала.ToString("o"),
                                    dateFinish = x.ДатаКонца.ToString("o"),
                                    usCount = x.КоличествоТребований(),
                                    newUsCount = x.КоличествоНовыхТребований(),
                                    processedUsCount = x.КоличествоТребованийВРаботе(),
                                    resolvedUsCount = x.КоличествоРешенныхТребований(),
                                    assigmentUsCount = x.КоличествоНазначенныхТребований(),
                                    notAssigmentusCount = x.КоличествоНеНазначенныхТребований(),
                                    sprintPower = x.ВременнаяМощностьСпринта(),
                                    sumEstimate = x.СуммарнаяОценкаРабот()
                                }),
                 JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPastSprints(int projectId)
        {
            IEnumerable<Спринт> спринты = new List<Спринт>();

            if (ТекущийПользователь.ЯвляетсяУчастникомПроекта(projectId))
            {
                спринты = МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(projectId).Спринты
                    .Where(спринт => спринт.ДатаКонца < DateTime.Now)
                    .OrderBy(x => x.ДатаКонца);
            }

            return this.Json(спринты
                                .Select(x => new
                                {
                                    id = x.Id,
                                    name = x.Название,
                                    dateStart = x.ДатаНачала.ToString("o"),
                                    dateFinish = x.ДатаКонца.ToString("o"),
                                    usCount = x.КоличествоТребований(),
                                    newUsCount = x.КоличествоНовыхТребований(),
                                    processedUsCount = x.КоличествоТребованийВРаботе(),
                                    resolvedUsCount = x.КоличествоРешенныхТребований(),
                                    assigmentUsCount = x.КоличествоНазначенныхТребований(),
                                    notAssigmentusCount = x.КоличествоНеНазначенныхТребований(),
                                    sprintPower = x.ВременнаяМощностьСпринта(),
                                    sumEstimate = x.СуммарнаяОценкаРабот()
                                }),
                 JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFutureSprints(int projectId)
        {
            IEnumerable<Спринт> спринты = new List<Спринт>();

            if (ТекущийПользователь.ЯвляетсяУчастникомПроекта(projectId))
            {
                спринты = МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(projectId).Спринты
                    .Where(спринт => спринт.ДатаНачала > DateTime.Now)
                    .OrderBy(x => x.ДатаКонца);
            }
            return this.Json(спринты
                                .Select(x => new
                                {
                                    id = x.Id,
                                    name = x.Название,
                                    dateStart = x.ДатаНачала.ToString("o"),
                                    dateFinish = x.ДатаКонца.ToString("o"),
                                    usCount = x.КоличествоТребований(),
                                    newUsCount = x.КоличествоНовыхТребований(),
                                    processedUsCount = x.КоличествоТребованийВРаботе(),
                                    resolvedUsCount = x.КоличествоРешенныхТребований(),
                                    assigmentUsCount = x.КоличествоНазначенныхТребований(),
                                    notAssigmentusCount = x.КоличествоНеНазначенныхТребований(),
                                    sprintPower = x.ВременнаяМощностьСпринта(),
                                    sumEstimate = x.СуммарнаяОценкаРабот()
                                }),
                 JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUsers(int projectId, bool includeEmpty = false)
        {
            List<Пользователь> пользователи = new List<Пользователь>();

            if (ТекущийПользователь.ЯвляетсяУчастникомПроекта(projectId))
            {
                пользователи.Add(new Пользователь() { Id = 0, Имя = "-", Фамилия = "" });
                пользователи.AddRange(МенеджерБД.Записи<Пользователь>());
            }

            return this.Json(пользователи
                                .Select(x => new
                                {
                                    id = x.Id,
                                    name = x.Имя + " " + x.Фамилия,
                                }),
                 JsonRequestBehavior.AllowGet);
        }
    }
}

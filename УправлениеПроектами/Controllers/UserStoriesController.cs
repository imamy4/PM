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
            int idПроекта = Конвертер.ВЧисло32(Request["projectId"]);

            if (!ТекущийПользователь.ЯвляетсяУчастникомПроекта(idПроекта))
            {
                return null;
            }

            Требование требование = new Требование();
            требование.Автор = ТекущийПользователь;

            требование.Название = Request["name"];
            требование.Описание = Request["description"];

            if (ТекущийПользователь.ЯвляетсяМенеджеромПроект(idПроекта))
            {
                требование.Важность = Конвертер.ВЧисло32(Request["importance"]);
            }
            требование.Оценка = Конвертер.ВЧисло32(Request["estimate"]);

            требование.Проект = new Проект { Id = idПроекта };
            int idКатегории = Конвертер.ВЧисло32(Request["categoryId"]);
            if (idКатегории != 0)
            {
                требование.Категория = new КатегорияТребования { Id = idКатегории };
            }
            требование.Статус = МенеджерБД.Записи<СтатусТребования>(статус => статус.Проект.Id == idПроекта && статус.Новое).FirstOrDefault();
          
            return требование;
        }

        protected override Требование ПолучитьСущностьДляОбновления()
        {
            int id = Конвертер.ВЧисло32(Request["id"]);
            Требование требование = МенеджерБД.ПолучитьЗаписьБДПоId<Требование>(id);

            if (требование != null && ТекущийПользователь.ЯвляетсяУчастникомПроекта(требование.Проект))
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
                if (Request["executorId"] != null)
                {
                    int idИсполнителя = Конвертер.ВЧисло32(Request["executorId"], -1);
                    if (idИсполнителя != -1)
                    {
                        if (idИсполнителя != 0)
                        {
                            требование.Назначения
                                .Add(new Назначение()
                                {
                                    Исполнитель = new Пользователь() { Id = idИсполнителя },
                                    Требование = new Требование() { Id = id },
                                    ДатаНазначения = DateTime.Now
                                });
                        }
                        else
                        {
                            Назначение назначение = требование.АктуальноеНазначение();
                            if (назначение != null)
                            {
                                назначение.ДатаСнятия = DateTime.Now;
                            }
                        }
                    }
                }
                if (Request["statusId"] != null)
                {
                    int idСтатуса = Конвертер.ВЧисло32(Request["statusId"], -1);
                    if (idСтатуса != -1)
                    {
                        требование.Статус = idСтатуса != 0
                            ? new СтатусТребования { Id = idСтатуса }
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
            IEnumerable<Проект> проекты = МенеджерБД.АктуальныеПроекты()
                .Where(проект => ТекущийПользователь.ЯвляетсяУчастникомПроекта(проект));

            return this.Json(проекты.Select(x => new { id = x.Id, name = x.Название }), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Список категоий для UI содания сущности
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public JsonResult GetCategories(int? projectId, bool includeEmpty = false)
        {
            IEnumerable<КатегорияТребования> категории = new List<КатегорияТребования>();
            if (projectId.HasValue && ТекущийПользователь.ЯвляетсяУчастникомПроекта(projectId.Value))
            {
                if (includeEmpty)
                {
                    ((List<КатегорияТребования>)категории).Add(new КатегорияТребования() { Id = 0, Название = "-" });
                }
                ((List<КатегорияТребования>)категории).AddRange(МенеджерБД.Записи<КатегорияТребования>(x => projectId.HasValue && x.Проект.Id == projectId.Value));
            }

            return this.Json(категории.Select(x => new { id = x.Id, name = x.Название }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStatusJumps(int? statusId)
        {
            List<СтатусТребования> статусы = new List<СтатусТребования>();

            if (statusId.HasValue)
            {
                СтатусТребования статус = МенеджерБД.ПолучитьЗаписьБДПоId<СтатусТребования>(statusId.Value);
                статусы.Add(статус);
                статусы.AddRange(статус.ВозможныеПереходы);
            }

            return this.Json(статусы.Select(x => new { id = x.Id, name = x.Название }), JsonRequestBehavior.AllowGet);
        }
    }
}

﻿using System;
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

        /// <summary>
        /// Рабочая страница проекта
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Desktop(int id)
        {
            return View(МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(id));
        }

        public JsonResult GetBacklog(int projectId)
        {
            IEnumerable<Требование> бэклог = МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(projectId).Требования
                .Where(x => x.Спринт == null).OrderBy(x => -x.Важность);
        
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
            IEnumerable<Спринт> спринты = МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(projectId).Спринты
                .Where(спринт => спринт.ДатаНачала <= DateTime.Now && спринт.ДатаКонца >= DateTime.Now)
                .OrderBy(x => x.ДатаКонца);

            return this.Json(спринты
                                .Select(x => new
                                {
                                    id = x.Id,
                                    name = x.Название,
                                    dateStart = x.ДатаНачала.ToString("o"),
                                    dateFinish = x.ДатаКонца.ToString("o")
                                }),
                 JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPastSprints(int projectId)
        {
            IEnumerable<Спринт> спринты = МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(projectId).Спринты
                .Where(спринт => спринт.ДатаКонца < DateTime.Now)
                .OrderBy(x => x.ДатаКонца);

            return this.Json(спринты
                                .Select(x => new
                                {
                                    id = x.Id,
                                    name = x.Название,
                                    dateStart = x.ДатаНачала.ToString("o"),
                                    dateFinish = x.ДатаКонца.ToString("o")
                                }),
                 JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFutureSprints(int projectId)
        {
            IEnumerable<Спринт> спринты = МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(projectId).Спринты
                .Where(спринт => спринт.ДатаНачала > DateTime.Now)
                .OrderBy(x => x.ДатаКонца);

            return this.Json(спринты
                                .Select(x => new
                                {
                                    id = x.Id,
                                    name = x.Название,
                                    dateStart = x.ДатаНачала.ToString("o"),
                                    dateFinish = x.ДатаКонца.ToString("o")
                                }),
                 JsonRequestBehavior.AllowGet);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Ninject;
using УправлениеПроектами.Global.Auth;
using МенеджерБД;
using МенеджерБД.Домен;
using УправлениеПроектами.Models.КлассыДляФормВвода;

namespace УправлениеПроектами.Controllers
{
    public abstract class BaseEntityController<T> : BaseController
        where T : class, IЗаписьБД, new()
    {
        #region Action's

        /// <summary>
        /// Вывод списка сущностей
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            return View(ПолучитьСущности());
        }

        /// <summary>
        /// Страница сущности
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ActionResult Id(int id)
        {
            return View(МенеджерБД.ПолучитьЗаписьБДПоId<T>(id));
        }
            
        /// <summary>
        /// Страница создания новой сущности
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Create()
        {
            return View(ПолучитьЭкземплярМодели());
        }

        /// <summary>
        /// Страница создания новой сущности, с проверкой на валидность введенных значений
        /// </summary>
        /// <param name="модельСущности"></param>
        /// <returns></returns>
        [NonAction]
        public virtual ActionResult Create(БазоваяМодельСущностиБД<T> модельСущности)
        {
            if (ПроверитьМодельНаВалидность(модельСущности))
            {
                T новаяСущность = модельСущности.ПеревестиВСущностьБД();

                МенеджерБД.СоздатьЗаписьБД<T>(новаяСущность);

                return RedirectToAction("Success", new { id = новаяСущность.Id });
            }

            return View(модельСущности);
        }

        /// <summary>
        /// Страница с уведомлением об удачном создании сущности
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ActionResult Success(int id)
        {
            return View(МенеджерБД.ПолучитьЗаписьБДПоId<T>(id));
        }

        /// <summary>
        /// Метод удаления сущности
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            МенеджерБД.УдалитьЗаписьБД<T>(id);
            return RedirectToAction("Index");
        }

        #endregion

        /// <summary>
        /// Получение списка сущостей для вывода 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected abstract IEnumerable<T> ПолучитьСущности();

        /// <summary>
        /// Создает модель сущности
        /// </summary>
        /// <returns></returns>
        protected abstract БазоваяМодельСущностиБД<T> ПолучитьЭкземплярМодели();

        /// <summary>
        /// Проверка модели на валидность
        /// </summary>
        /// <param name="модельСущности"></param>
        /// <returns>True если модель валидна</returns>
        protected abstract bool ПроверитьМодельНаВалидность(БазоваяМодельСущностиБД<T> модельСущности);
    }
}

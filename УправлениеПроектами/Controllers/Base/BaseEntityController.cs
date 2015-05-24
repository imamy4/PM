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
            return ТекущийПользователь != null 
                        ? View(ПолучитьСущности())
                        : View("_AuthError");
        }

        /// <summary>
        /// Страница сущности
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ActionResult Id(int id)
        {
            return ТекущийПользователь != null
                        ? View(МенеджерБД.ПолучитьЗаписьБДПоId<T>(id))
                        : View("_AuthError");
        }
            
        /// <summary>
        /// Страница создания новой сущности
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Create()
        {
            БазоваяМодельСущностиБД<T> модель = ПолучитьЭкземплярМодели();
            return ТекущийПользователь != null && модель != null
                        ? View(модель)
                        : View("_AuthError");
        }

        /// <summary>
        /// Страница создания новой сущности, с проверкой на валидность введенных значений
        /// </summary>
        /// <param name="модельСущности"></param>
        /// <returns></returns>
        [NonAction]
        public virtual ActionResult Create(БазоваяМодельСущностиБД<T> модельСущности)
        {
            if (ТекущийПользователь == null)
            {
                return View("_AuthError");
            }

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
            return ТекущийПользователь != null
                        ? View(МенеджерБД.ПолучитьЗаписьБДПоId<T>(id))
                        : View("_AuthError");
        }

        /// <summary>
        /// Метод удаления сущности
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            if (ТекущийПользователь == null)
            {
                return View("_AuthError");
            }

            МенеджерБД.УдалитьЗаписьБД<T>(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Создание новой сущности, возвращает JSON результат с полем success
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateExt()
        {
            bool удачноеСохранение = false;

            if (ТекущийПользователь != null)
            {
                T сущность = ПолучитьСущностьДляСоздания();
                if (сущность != null)
                {
                    удачноеСохранение = МенеджерБД.СоздатьЗаписьБД<T>(сущность);
                }
            }

            return this.Json(new { success = удачноеСохранение }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Обновление сущности, возвращает JSON результат с полем success
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateExt()
        {
            bool удачноеОбновление = false;

            if (ТекущийПользователь != null)
            {
                T сущность = ПолучитьСущностьДляОбновления();
                if (сущность != null)
                {
                    удачноеОбновление = МенеджерБД.ОбновитьЗаписьБД<T>(сущность);
                }
            }
          
            return this.Json(new { success = удачноеОбновление }, JsonRequestBehavior.AllowGet);
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

        /// <summary>
        /// Создает сущность на основе Request
        /// </summary>
        /// <returns></returns>
        protected virtual T ПолучитьСущностьДляСоздания()
        {
            return null;
        }

        /// <summary>
        /// Получает сущность на основе Request и заполняет поля, пришежшие в Request
        /// </summary>
        /// <returns></returns>
        protected virtual T ПолучитьСущностьДляОбновления()
        {
            return null;
        }
    }
}

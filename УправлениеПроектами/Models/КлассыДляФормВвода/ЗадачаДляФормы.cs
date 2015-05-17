using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using МенеджерБД.Домен;

namespace УправлениеПроектами.Models.КлассыДляФормВвода
{
    public class ЗадачаДляФормы : БазоваяМодельСущностиБД<Задача>
    {
        public string Название { get; set; }

        public string Описание { get; set; }

        public int IdТребование { get; set; }

        public int IdАвтор { get; set; }

        public override Задача ПеревестиВСущностьБД()
        {
            Задача задача = base.ПеревестиВСущностьБД();
            
            задача.Требование = МенеджерБД.ПолучитьЗаписьБДПоId<Требование>(IdТребование);
            задача.Автор = МенеджерБД.ПолучитьЗаписьБДПоId<Пользователь>(IdАвтор);

            return задача;
        }
    }
}
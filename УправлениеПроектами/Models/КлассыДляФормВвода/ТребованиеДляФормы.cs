using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using МенеджерБД.Домен;

namespace УправлениеПроектами.Models.КлассыДляФормВвода
{
    public class ТребованиеДляФормы : БазоваяМодельСущностиБД<Требование>
    {
        public int IdПроект { get; set; }

        public int IdКатегория { get; set; }
      
        public int IdАвтор { get; set; }

        public string Название { get; set; }

        public string Описание { get; set; }

        public int Оценка { get; set; }

        public int Важность { get; set; }

        public override Требование ПеревестиВСущностьБД()
        {
            Требование требование = base.ПеревестиВСущностьБД();
            требование.Проект = МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(IdПроект);
            требование.Категория = МенеджерБД.ПолучитьЗаписьБДПоId<КатегорияТребования>(IdКатегория);
            требование.Автор = МенеджерБД.ПолучитьЗаписьБДПоId<Пользователь>(IdАвтор);

            return требование;
        }

    }
}
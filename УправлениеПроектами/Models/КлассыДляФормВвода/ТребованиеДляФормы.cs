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
        public int IdПроекта { get; set; }

        public int IdАвтора { get; set; }

        public int IdКатегории { get; set; }

        public string Название { get; set; }

        public string Описание { get; set; }

        public int Оценка { get; set; }

        public int Важность { get; set; }

        public IEnumerable<SelectListItem> ПроектыSelectList
        {
            get
            {
                foreach (Проект проект in МенеджерБД.АктуальныеПроекты())
                {
                    yield return new SelectListItem
                    {
                        Value = проект.Id.ToString(),
                        Text = проект.Название.ToString(),
                        Selected = false
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> КатегорииSelectList
        {
            get
            {
                Проект выбранныйПроект = МенеджерБД.ПолучитьЗаписьБДПоId<Проект>(IdПроекта);
                if (выбранныйПроект != null)
                {
                    foreach (КатегорияТребования категория in выбранныйПроект.Категории)
                    {
                        yield return new SelectListItem
                        {
                            Value = категория.Id.ToString(),
                            Text = категория.Название.ToString(),
                            Selected = false
                        };
                    }
                }
            }
        }
    }
}
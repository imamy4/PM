using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using МенеджерБД.Домен;

namespace УправлениеПроектами.Models.КлассыДляФормВвода
{
    public class СпринтДляФормы : БазоваяМодельСущностиБД<Спринт>
    {
        public int IdПроекта { get; set; }

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

        public string Название { get; set; }

        public DateTime ДатаНачала
        {
            get
            {
                return new DateTime(ГодДатаНачала, МесяцДатаНачала, ДеньДатаНачала);
            }
        }

        public int ДеньДатаНачала { get; set; }

        public int МесяцДатаНачала { get; set; }

        public int ГодДатаНачала { get; set; }

        public IEnumerable<SelectListItem> ДеньДатаНачалаSelectList
        {
            get
            {
                for (int i = 1; i < 32; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = ДеньДатаНачала == i
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> МесяцДатаНачалаSelectList
        {
            get
            {
                for (int i = 1; i < 13; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = new DateTime(2013, i, 1).ToString("MMMM"),
                        Selected = МесяцДатаНачала == i
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> ГодДатаНачалаSelectList
        {
            get
            {
                for (int i = 2013; i <= DateTime.Now.Year + 2; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = ГодДатаНачала == i
                    };
                }
            }
        }

        public DateTime ДатаКонца
        {
            get
            {
                return new DateTime(ГодДатаКонца, МесяцДатаКонца, ДеньДатаКонца, 23, 59, 59);
            }
        }

        public int ДеньДатаКонца { get; set; }

        public int МесяцДатаКонца { get; set; }

        public int ГодДатаКонца { get; set; }

        public IEnumerable<SelectListItem> ДеньДатаКонцаSelectList
        {
            get
            {
                for (int i = 1; i < 32; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = ДеньДатаКонца == i
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> МесяцДатаКонцаSelectList
        {
            get
            {
                for (int i = 1; i < 13; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = new DateTime(2013, i, 1).ToString("MMMM"),
                        Selected = МесяцДатаКонца == i
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> ГодДатаКонцаSelectList
        {
            get
            {
                for (int i = 2013; i <= DateTime.Now.Year + 2; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = ГодДатаКонца == i
                    };
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using МенеджерБД.Домен;

namespace УправлениеПроектами.Models.КлассыДляФормВвода
{
    public class ПроектДляФормы : БазоваяМодельСущностиБД<Проект>
    {
        public string Название { get; set; }
        public string Описание { get; set; }

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
                        Text = new DateTime(2000, i, 1).ToString("MMMM"),
                        Selected = МесяцДатаНачала == i
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> ГодДатаНачалаSelectList
        {
            get
            {
                for (int i = 2000; i <= DateTime.Now.Year; i++)
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
                return new DateTime(ГодДатаКонца, МесяцДатаКонца, ДеньДатаКонца);
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
                        Text = new DateTime(2000, i, 1).ToString("MMMM"),
                        Selected = МесяцДатаКонца == i
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> ГодДатаКонцаSelectList
        {
            get
            {
                for (int i = 2000; i <= DateTime.Now.Year; i++)
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
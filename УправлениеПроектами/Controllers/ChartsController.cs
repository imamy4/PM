using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using МенеджерБД.Домен;

namespace УправлениеПроектами.Controllers
{
    /// <summary>
    /// Контроллер для предоставления данных для графиков
    /// </summary>
    public class ChartsController : BaseController
    {
        public JsonResult GetBurndownData(int sprintId)
        {
            Спринт спринт = МенеджерБД.ПолучитьЗаписьБДПоId<Спринт>(sprintId);

            List<BurndownItem> данныеДляГрафика = new List<BurndownItem>();

            if (спринт != null)
            {
                decimal идеальныеЗначения = спринт.СуммарнаяОценкаРабот(),
                    реальныеЗначения = идеальныеЗначения,
                    шаг = идеальныеЗначения / (спринт.ДатаКонца - спринт.ДатаНачала).Days;

                for (DateTime дата = спринт.ДатаНачала; дата <= DateTime.Now && дата <= спринт.ДатаКонца; дата = дата.AddDays(1))
                {
                    данныеДляГрафика.Add(new BurndownItem
                    {
                        date = дата.ToString("o"),
                        ideal = идеальныеЗначения,
                        real = реальныеЗначения
                        //usCount = количествоТребований,
                        //resolvedUsCount = решеноТребований
                    });

                    //решеноТребований = спринт.Требования.Where(требование => требование.Статус.Решенное && требование.ДатаЗакрытия >= дата && требование.ДатаЗакрытия < дата.AddDays(1)).Count();
                    //количествоТребований -= решеноТребований;
                    идеальныеЗначения -= шаг;
                    реальныеЗначения -= спринт.Требования.Where(требование => требование.Статус.Решенное && требование.ДатаЗакрытия >= дата && требование.ДатаЗакрытия < дата.AddDays(1))
                                                            .Sum(x => x.Оценка);
                }
            }

            return this.Json(данныеДляГрафика, JsonRequestBehavior.AllowGet);
        }

        class BurndownItem
        {
            public string date { get; set; }
            public decimal ideal { get; set; }
            public decimal real { get; set; }
            //public decimal usCount { get; set; }
            //public decimal resolvedUsCount { get; set; }
        }
    }
}

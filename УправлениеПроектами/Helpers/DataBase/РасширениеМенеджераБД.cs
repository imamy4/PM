using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using МенеджерБД;
using МенеджерБД.Домен;

namespace УправлениеПроектами
{
    public static class РасширениеМенеджераБД
    {
        /// <summary>
        /// Возвращает список актуальных проектов
        /// </summary>
        /// <param name="менеджерБД"></param>
        /// <returns></returns>
        public static IEnumerable<Проект> АктуальныеПроекты(this IМенеджерБД менеджерБД)
        {
            return менеджерБД.Записи<Проект>()
                .Where(проект => проект.ДатаНачала <= DateTime.Now
                        && проект.ДатаКонца >= DateTime.Now);
        }
    }
}
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
        
        /// <summary>
        /// Возвращает список актуальных спринтов
        /// </summary>
        /// <param name="менеджерБД"></param>
        /// <returns></returns>
        public static IEnumerable<Спринт> АктуальныеСпринты(this IМенеджерБД менеджерБД)
        {
            return менеджерБД.Записи<Спринт>()
                .Where(спринт => спринт.ДатаНачала <= DateTime.Now
                        && спринт.ДатаКонца >= DateTime.Now);
        }

        /// <summary>
        /// Возвращает список актуальных спринтов
        /// </summary>
        /// <param name="менеджерБД"></param>
        /// <returns></returns>
        public static IEnumerable<Требование> АктуальныеТребования(this IМенеджерБД менеджерБД)
        {
            return менеджерБД.Записи<Требование>()
                .Where(x => true);
        }
    }
}
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

        
        public static IEnumerable<Проект> АктуальныеПроекты(this IEnumerable<Проект> проекты)
        {
            return проекты.Where(проект => проект.ДатаНачала <= DateTime.Now
                && проект.ДатаКонца >= DateTime.Now);
        }


        public static IEnumerable<Требование> НовыеТребования(this IEnumerable<Требование> требования)
        {
            return требования.Where(требование => требование.Статус.Новое);
        }

        public static IEnumerable<Требование> ОткрытыеТребования(this IEnumerable<Требование> требования)
        {
            return требования.Where(требование => требование.Статус.Новое || требование.Статус.ВРаботе());
        }
        
        public static IEnumerable<Требование> ТребованияВРаботе(this IEnumerable<Требование> требования)
        {
            return требования.Where(требование => требование.Статус.ВРаботе());
        }

        public static IEnumerable<Требование> РешенныеТребования(this IEnumerable<Требование> требования)
        {
            return требования.Where(требование => требование.Статус.Решенное);
        }
    
    }
}
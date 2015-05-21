using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using МенеджерБД.Домен;

namespace УправлениеПроектами
{
    public static class РасширениеСущностейБД
    {
        public static int КоличествоТребований(this Спринт спринт)
        {
            return спринт.Требования.Count;
        }

        public static int КоличествоРешенныхТребований(this Спринт спринт)
        {
            return 0;
        }

        public static int КоличествоНазначенныхТребований(this Спринт спринт)
        {
            return спринт.Требования.Where(x => x.Исполнитель() != null).Count();
        }

        public static int КоличествоНеНазначенныхТребований(this Спринт спринт)
        {
            return спринт.Требования.Where(x => x.Исполнитель() == null).Count();
        }

        public static int СуммарнаяОценкаРабот(this Спринт спринт)
        {
            return спринт.Требования.Sum(x => x.Оценка);
        }
        
        public static int ВременнаяМощностьСпринта(this Спринт спринт)
        {
            return (спринт.ДатаКонца - спринт.ДатаНачала).Days * 8;
        }

        public static Пользователь Исполнитель(this Требование требование)
        {
            Назначение назначение = требование.АктуальноеНазначение();
            return назначение != null ? назначение.Исполнитель : null;
        }

        public static Назначение АктуальноеНазначение(this Требование требование)
        {
            DateTime нулеваяДата = new DateTime();
            return требование.Назначения.FirstOrDefault(x => x.ДатаСнятия == null || x.ДатаСнятия == нулеваяДата);
        }
    }
}
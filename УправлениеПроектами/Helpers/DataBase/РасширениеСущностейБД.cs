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

        public static Пользователь Исполнитель(this Требование требование)
        {
            Назначение назначение = требование.Назначения.FirstOrDefault(x => x.ДатаСнятия > DateTime.Now);
            return назначение != null ? назначение.Исполнитель : null;
        }
    }
}
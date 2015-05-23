﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using МенеджерБД;
using МенеджерБД.Домен;

namespace УправлениеПроектами
{
    public static class РасширениеСущностейБД
    {
        #region Спринт
 
        public static IEnumerable<Требование> РешенныеТребования(this Спринт спринт)
        {
            return спринт.Требования.Where(требование => требование.Статус.Решенное);
        }

        public static IEnumerable<Требование> НовыеТребования(this Спринт спринт)
        {
            return спринт.Требования.Where(требование => требование.Статус.Новое);
        }

        public static IEnumerable<Требование> ТребованияВРаботе(this Спринт спринт)
        {
            return спринт.Требования.Where(требование => требование.Статус.ВРаботе());
        }

        public static IEnumerable<Требование> НеназначенныеТребования(this Спринт спринт)
        {
            return спринт.Требования.Where(требование => требование.Исполнитель() == null);
        }

        public static IEnumerable<Требование> НазначенныеТребования(this Спринт спринт)
        {
            return спринт.Требования.Where(требование => требование.Исполнитель() != null);
        }

        public static int КоличествоТребований(this Спринт спринт)
        {
            return спринт.Требования.Count;
        }

        public static int КоличествоНовыхТребований(this Спринт спринт)
        {
            return спринт.НовыеТребования().Count();
        }
       
        public static int КоличествоТребованийВРаботе(this Спринт спринт)
        {
            return спринт.ТребованияВРаботе().Count();
        }
    
        public static int КоличествоРешенныхТребований(this Спринт спринт)
        {
            return спринт.РешенныеТребования().Count();
        }

        public static int КоличествоНазначенныхТребований(this Спринт спринт)
        {
            return спринт.НазначенныеТребования().Count();
        }

        public static int КоличествоНеНазначенныхТребований(this Спринт спринт)
        {
            return спринт.НеназначенныеТребования().Count();
        }

        public static int СуммарнаяОценкаРабот(this Спринт спринт)
        {
            return спринт.Требования.Sum(x => x.Оценка);
        }
        
        public static int ВременнаяМощностьСпринта(this Спринт спринт)
        {
            return (спринт.ДатаКонца - спринт.ДатаНачала).Days * 8;
        }
      
        #endregion

        #region Требования
       
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

        #endregion

        #region СтатусТребования

        public static bool ВРаботе(this СтатусТребования статус)
        {
            return !статус.Новое && !статус.Решенное;
        }

        #endregion

        #region Пользователь

        public static IEnumerable<Требование> НазначенныеТреования(this Пользователь пользователь)
        {
            return DependencyResolver.Current.GetService<IМенеджерБД>()
                .Записи<Требование>(требование => требование.Исполнитель() == пользователь);
        }

        #endregion
    }
}
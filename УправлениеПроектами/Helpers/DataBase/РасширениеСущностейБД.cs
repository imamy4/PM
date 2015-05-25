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

        public static decimal СуммарнаяОценкаРабот(this Спринт спринт)
        {
            return спринт.Требования.ОткрытыеТребования().Sum(x => x.Оценка);
        }
        
        public static int ВременнаяМощностьСпринта(this Спринт спринт)
        {
            return (спринт.ДатаКонца - спринт.ДатаНачала).Days * 8 * спринт.Проект.КоличествоПользователей();
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
            DateTime нулеваяДата = new DateTime(3, 2, 1);
            return требование.Назначения.FirstOrDefault(x => x.ДатаСнятия == null || x.ДатаСнятия <= нулеваяДата);
        }

        #endregion

        #region СтатусТребования

        public static bool ВРаботе(this СтатусТребования статус)
        {
            return !статус.Новое && !статус.Решенное;
        }

        #endregion

        #region Пользователь

        public static IEnumerable<Требование> НазначенныеТребования(this Пользователь пользователь)
        {
            return DependencyResolver.Current.GetService<IМенеджерБД>()
                .Записи<Требование>(требование => требование.Исполнитель() != null && требование.Исполнитель().Id == пользователь.Id);
        }

        public static bool ЯвляетсяМенеджеромПроект(this Пользователь пользователь, Проект проект)
        {
            return ЯвляетсяМенеджеромПроект(пользователь, проект.Id);
        }

        public static bool ЯвляетсяМенеджеромПроект(this Пользователь пользователь, int idПроекта)
        {
            return пользователь != null && пользователь.Роли.Any(роль => роль.Проект != null && роль.Проект.Id == idПроекта && роль.Название == Роль.МенеджерПроекта || роль.Название == Роль.Администратор);
        }

        public static bool ЯвляетсяУчастникомПроекта(this Пользователь пользователь, Проект проект)
        {
            return ЯвляетсяУчастникомПроекта(пользователь, проект.Id);
        }

        public static bool ЯвляетсяУчастникомПроекта(this Пользователь пользователь, int idПроекта)
        {
            return пользователь != null && пользователь.Роли.Any(роль => роль.Проект != null && роль.Проект.Id == idПроекта || роль.Название == Роль.Администратор);
        }

        public static bool ЯвляетсяАдминистратором(this Пользователь пользователь)
        {
            return пользователь != null && пользователь.Роли.Any(роль => роль.Название == Роль.Администратор);
        }

        public static IEnumerable<Проект> Проекты(this Пользователь пользователь)
        {
            HashSet<Проект> проекты = new HashSet<Проект>(пользователь.Роли
                .Where(роль => роль.Проект != null)
                .Select(роль => роль.Проект), new ЗаписьБДEqualityComparer());

            return проекты;
        }

        #endregion

        #region Проект

        public static IEnumerable<Требование> РешенныеТребования(this Проект проект)
        {
            return проект.Требования.Where(требование => требование.Статус.Решенное);
        }

        public static IEnumerable<Требование> НовыеТребования(this Проект проект)
        {
            return проект.Требования.Where(требование => требование.Статус.Новое);
        }

        public static IEnumerable<Требование> ТребованияВРаботе(this Проект проект)
        {
            return проект.Требования.Where(требование => требование.Статус.ВРаботе());
        }

        public static IEnumerable<Требование> НеназначенныеТребования(this Проект проект)
        {
            return проект.Требования.Where(требование => требование.Исполнитель() == null);
        }

        public static IEnumerable<Требование> НазначенныеТребования(this Проект проект)
        {
            return проект.Требования.Where(требование => требование.Исполнитель() != null);
        }

        public static int КоличествоТребований(this Проект проект)
        {
            return проект.Требования.Count;
        }

        public static int КоличествоНовыхТребований(this Проект проект)
        {
            return проект.НовыеТребования().Count();
        }

        public static int КоличествоТребованийВРаботе(this Проект проект)
        {
            return проект.ТребованияВРаботе().Count();
        }

        public static int КоличествоРешенныхТребований(this Проект проект)
        {
            return проект.РешенныеТребования().Count();
        }

        public static int КоличествоНазначенныхТребований(this Проект проект)
        {
            return проект.НазначенныеТребования().Count();
        }

        public static int КоличествоНеНазначенныхТребований(this Проект проект)
        {
            return проект.НеназначенныеТребования().Count();
        }

        public static decimal СуммарнаяОценкаРабот(this Проект проект)
        {
            return проект.Требования.ОткрытыеТребования().Sum(x => x.Оценка);
        }

        public static IEnumerable<Пользователь> Пользователи(this Проект проект)
        {
            return DependencyResolver.Current.GetService<IМенеджерБД>()
                                .Записи<Пользователь>(пользователь => пользователь.Роли
                                    .Any(роль => роль.Проект != null && проект != null && роль.Проект.Id == проект.Id));
        }

        public static int КоличествоПользователей(this Проект проект)
        {
            return проект.Пользователи().Count();
        }

        #endregion

        class ЗаписьБДEqualityComparer : IEqualityComparer<IЗаписьБД>
        {
            public bool Equals(IЗаписьБД x, IЗаписьБД y)
            {
                return object.Equals(x.Id, y.Id);
            }

            public int GetHashCode(IЗаписьБД obj)
            {
                return obj.GetType().Name.GetHashCode() + obj.Id.GetHashCode();
            }
        }
    }
}
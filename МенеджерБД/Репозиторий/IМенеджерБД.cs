using System;
using System.Collections.Generic;

namespace МенеджерБД
{
    using System.Linq.Expressions;
    using Домен;

    public interface IМенеджерБД
    {
        IEnumerable<T> Записи<T>() where T : class, IЗаписьБД;

        IEnumerable<T> Записи<T>(Func<T, bool> критерийОтбора) where T : class, IЗаписьБД;

        bool СуществуетЗапись<T>(Func<T, bool> критерийОтбора) where T : class, IЗаписьБД;

        bool СоздатьЗаписьБД<T>(T запись) where T : class, IЗаписьБД;

        bool ОбновитьЗаписьБД<T>(T запись) where T : class, IЗаписьБД;

        bool УдалитьЗаписьБД<T>(int idЗаписи) where T : class, IЗаписьБД;

        bool УдалитьЗаписьБД<T>(T запись) where T : class, IЗаписьБД;

        T ПолучитьЗаписьБДПоId<T>(int idЗаписи) where T : class, IЗаписьБД;

        T ПолучитьПервуюЗаписьБД<T>(Func<T, bool> критерийОтбора) where T : class, IЗаписьБД;


        Пользователь Авторизоваться(string email, string пароль);
    }
}

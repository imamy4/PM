using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace УправлениеПроектами.Helpers
{
    /// <summary>
    /// Обертка над Converter, при ошибках конвертации возвращает дефолтное значение
    /// </summary>
    public static class Конвертер
    {
        public static int ВЧисло32(string строка, int значениеПоУмолчанию = 0, bool выдаватьИсключение = false)
        {
            int результат = значениеПоУмолчанию;
            try
            {
                результат = Convert.ToInt32(строка);
            }
            catch
            {
                if (выдаватьИсключение)
                {
                    throw;
                }
            }

            return результат;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using МенеджерБД.Домен;

namespace УправлениеПроектами.Models.КлассыДляФормВвода
{
    public abstract class БазоваяМодельСущностиБД<T> where T: IЗаписьБД, new ()
    {
        public virtual T ПеревестиВСущностьБД()
        {
            T сущность = new T();

            // проходим по открытым свойствам типа Т и ищем свойства с тем же названием в текущем типе
            foreach (PropertyInfo свойствоСущности in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                PropertyInfo свойствоМодели = this.GetType().GetProperty(свойствоСущности.Name);
                if (свойствоМодели != null) // можно добавить на соответствие типов
                {
                    object значениеМодели = свойствоМодели.GetValue(this);
                    свойствоСущности.SetValue(сущность, значениеМодели);
                }
            }

            return сущность;
        }
    }
}
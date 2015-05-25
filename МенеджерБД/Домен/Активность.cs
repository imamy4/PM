using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МенеджерБД.Домен
{
    public class Активность : IЗаписьБД
    {
        public virtual int Id { get; set; }

        public virtual DateTime ДатаНачала { get; set; }
        public virtual DateTime ДатаКонца { get; set; }

        public virtual double ЗатраченноеВремя
        {
            get
            {
                return (ДатаКонца - ДатаНачала).TotalHours;
            }
        }

        public virtual Требование Требование { get; set; }
        public virtual Пользователь Пользователь { get; set; }
    }
}

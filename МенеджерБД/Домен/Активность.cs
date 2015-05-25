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
                return Math.Round(Math.Max((ДатаКонца - ДатаНачала).TotalHours, 0), 2);
            }
        }

        public virtual Требование Требование { get; set; }
        public virtual Пользователь Пользователь { get; set; }
    }
}

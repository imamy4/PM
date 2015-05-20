using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МенеджерБД.Домен
{
    public class Назначение : IЗаписьБД
    {
        public virtual int Id { get; set; }

        public virtual DateTime ДатаНазначения { get; set; }
        public virtual DateTime ДатаСнятия { get; set; }

        public virtual Требование Требование { get; set; }
        public virtual Пользователь Исполнитель { get; set; }
    }
}

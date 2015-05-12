using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МенеджерБД.Домен
{
    public class Спринт : IЗаписьБД
    {
        public Спринт()
        {
            Требования = new HashSet<Требование>();
        }

        public virtual int Id { get; set; }
        public virtual string Название { get; set; }
        public virtual DateTime ДатаНачала { get; set; }
        public virtual DateTime ДатаКонца { get; set; }

        public virtual Проект Проект { get; set; }

        public virtual ISet<Требование> Требования { get; protected set; }
        public virtual ISet<Задача> Требования { get; protected set; }
    }
}

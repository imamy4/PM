using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МенеджерБД.Домен
{
    public class СтатусТребования : IЗаписьБД
    {
        public СтатусТребования()
        {
            Требования = new HashSet<Требование>();
            ВозможныеПереходы = new List<СтатусТребования>();
        }

        public virtual int Id { get; set; }
        public virtual string Название { get; set; }

        public virtual bool Новое { get; set; }
        public virtual bool Решенное { get; set; }

        public virtual Проект Проект { get; set; }

        public virtual ISet<Требование> Требования { get; protected set; }
        public virtual IList<СтатусТребования> ВозможныеПереходы { get; protected set; }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МенеджерБД.Домен
{
    public class Проект : IЗаписьБД
    {
        public Проект()
        {
            Спринты = new HashSet<Спринт>();
            Требования = new HashSet<Требование>();
            Категории = new HashSet<КатегорияТребования>();
        }

        public virtual int Id { get; protected set; }
        public virtual string Название { get; set; }
        public virtual string Описание { get; set; }
        public virtual DateTime ДатаНачала { get; set; }
        public virtual DateTime ДатаКонца { get; set; }

        public ISet<Спринт> Спринты { get; protected set; }
        public ISet<Требование> Требования { get; protected set; }
        public ISet<КатегорияТребования> Категории { get; protected set; }
    }
}

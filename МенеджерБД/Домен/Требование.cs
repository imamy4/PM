using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МенеджерБД.Домен
{
    public class Требование : IЗаписьБД
    {
        public Требование()
        {
            Задачи = new HashSet<Задача>();
        }

        public virtual int Id { get; set; }

        public virtual string Название { get; set; }
        public virtual string Описание { get; set; }
        public virtual int Оценка { get; set; }
        public virtual int Важность { get; set; }
        
        public virtual Проект Проект { get; set; }
        public virtual Спринт Спринт { get; set; }
        public virtual Пользователь Автор { get; set; }
        public virtual КатегорияТребования Категория { get; set; }

        public virtual ISet<Задача> Задачи { get; protected set; }
    }
}

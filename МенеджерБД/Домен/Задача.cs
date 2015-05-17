using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МенеджерБД.Домен
{
    public class Задача : IЗаписьБД
    {
        public virtual int Id { get; set; }

        public virtual string Название { get; set; }
        public virtual string Описание { get; set; }

        public virtual Требование Требование { get; set; }
        public virtual Спринт Спринт { get; set; }
        public virtual Пользователь Автор { get; set; }
        public virtual Пользователь Исполнитель { get; set; }
    }
}

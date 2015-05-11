using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МенеджерБД.Домен
{
    public class Требование : IЗаписьБД
    {
        public virtual int Id { get; set; }
        public virtual int IdПроекта { get; set; }
        public virtual int IdСпринта { get; set; }
        public virtual int IdАвтора { get; set; }
        public virtual int IdКатегории { get; set; }
        public virtual string Название { get; set; }
        public virtual string Описание { get; set; }
        public virtual int Оценка { get; set; }
        public virtual int Важность { get; set; }
    }
}

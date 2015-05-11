using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МенеджерБД.Домен
{
    public class Задачи : IЗаписьБД
    {
        public virtual int Id { get; set; }
        public virtual int IdТребования { get; set; }
        public virtual int IdСпринта { get; set; }
        public virtual int IdАвтора { get; set; }
        public virtual int IdИсполнителя { get; set; }
        public virtual string Название { get; set; }
        public virtual string Описание { get; set; }
    }
}

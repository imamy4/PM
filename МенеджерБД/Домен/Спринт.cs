using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МенеджерБД.Домен
{
    public class Спринт : IЗаписьБД
    {
        public virtual int Id { get; set; }
        public virtual int IdПроекта{ get; set; }
        public virtual string Название { get; set; }
        public virtual DateTime ДатаНачала { get; set; }
        public virtual DateTime ДатаКонца { get; set; }
    }
}

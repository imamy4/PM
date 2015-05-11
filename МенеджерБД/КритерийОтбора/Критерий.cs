using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МенеджерБД.КритерийОтбора
{
    public class Критерий
    {
        public string Ключ { get; set; }
        public object Значение { get; set; }

        public ТипыСравнения _типСравнения = ТипыСравнения.Равно;
        public ТипыСравнения ТипСравнения
        {
            get { return _типСравнения; }
            set { _типСравнения = value; }
        }
        public ДопУсловия ДопУсловия { get; set; }
    }
}

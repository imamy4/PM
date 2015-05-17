using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МенеджерБД.Домен
{
    /// <summary>
    /// Интерфейс просто для ограничения в ниверсальных методах IМенеджерБД
    /// </summary>
    public interface IЗаписьБД
    {
        int Id { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using МенеджерБД.Домен;

namespace УправлениеПроектами.Global.Auth
{
    public interface IПредоставительПользователя
    {
        Пользователь Пользователь { get; set; }
    }
}

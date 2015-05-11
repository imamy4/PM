using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using МенеджерБД;
using Безопасность;
using МенеджерБД.Домен;

namespace УправлениеПроектами.Models.КлассыДляФормВвода
{
    /// <summary>
    /// Представляет связь формы регистрации с пользователям БД.
    /// </summary>
    public class ПользовательРегистрации : БазоваяМодельСущностиБД<Пользователь>
    {
        public string Имя { set; get; }
        public string Фамилия { set; get; }

        public string Email { set; get; }

        public string Пароль { set; get; }
        public string ПодтверждениеПароля { set; get; }

        //public Пользователь ПеревестиВПользователяБД()
        //{
        //    Пользователь новыйПользователь = new Пользователь();

        //    новыйПользователь.Имя = Имя;
        //    новыйПользователь.Фамилия = Фамилия;

        //    новыйПользователь.Email = Email;

        //    новыйПользователь.ХэшПароля = Шифрование.ПолучитьХешMD5Строкой(Пароль);

        //    return новыйПользователь;
        //}
    }
}
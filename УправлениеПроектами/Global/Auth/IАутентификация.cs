using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using МенеджерБД;
using МенеджерБД.Домен;

namespace УправлениеПроектами.Global.Auth
{
    /// <summary>
    /// Интерфейс для авторизации
    /// </summary>
    public interface IАутентификация
    {
        /// <summary>
        /// Контекст (тут мы получаем доступ к запросу и кукисам)
        /// </summary>
        HttpContext HttpContext { get; set; }

        /// <summary>
        /// Процедура входа
        /// </summary>
        /// <returns></returns>
        Пользователь Войти(string логин, string пароль, bool постояннаяАвторизация);

        /// <summary>
        /// Входим без пароля (использовать осторожно)
        /// </summary>
        /// <returns></returns>
        Пользователь Войти(string логин);

        /// <summary>
        /// Выход
        /// </summary>
        void Выйти();

        /// <summary>
        /// Данные о текущем пользователе
        /// </summary>
        IPrincipal ТекущийПользователь { get; }
    }
}

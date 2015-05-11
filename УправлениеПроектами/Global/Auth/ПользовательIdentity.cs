using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

using МенеджерБД;
using МенеджерБД.Домен;

namespace УправлениеПроектами.Global.Auth
{
    /// <summary>
    /// Реализация интерфейса для идентификации пользователя
    /// </summary>
    public class ПользовательIdentity : IIdentity, IПредоставительПользователя
    {
        /// <summary>
        /// Текщий пользователь
        /// </summary>
        public Пользователь Пользователь { get; set; }

        #region Реализация IIdentity
        /// <summary>
        /// Тип класса для пользователя
        /// </summary>
        public string AuthenticationType
        {
            get
            {
                return typeof(Пользователь).ToString();
            }
        }

        /// <summary>
        /// Авторизован или нет
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return Пользователь != null;
            }
        }

        /// <summary>
        /// Имя пользователя (уникальное) [у нас это счас Email]
        /// </summary>
        public string Name
        {
            get
            {
                if (Пользователь != null)
                {
                    return Пользователь.Email;
                }
                //иначе аноним
                return "anonym";
            }
        }
        #endregion

        /// <summary>
        /// Инициализация по имени
        /// </summary>
        /// <param name="email">имя пользователя [email]</param>
        public void Инициализация(string email, IМенеджерБД менеджерБД)
        {
            if (!string.IsNullOrEmpty(email))
            {
                Пользователь = менеджерБД.ПолучитьПервуюЗаписьБД<Пользователь>(p => string.Compare(p.Email, email, true) == 0);
            }
        }
    }
}
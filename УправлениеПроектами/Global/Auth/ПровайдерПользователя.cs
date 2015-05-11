using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

using МенеджерБД;

namespace УправлениеПроектами.Global.Auth
{
    /// <summary>
    /// Реализация интерфейса Principal
    /// </summary>
    public class ПровайдерПользователя : IPrincipal
    {
        private ПользовательIdentity пользовательIdentity { get; set; }

        #region Реализация IPrincipal

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public IIdentity Identity
        {
            get
            {
                return пользовательIdentity;
            }
        }

        /// <summary>
        /// Находится в данной роли или нет
        /// </summary>
        /// <param name="role">имя роли</param>
        /// <returns>есть такая роль или нет</returns>
        public bool IsInRole(string кодРоли)
        {
            if (пользовательIdentity.Пользователь == null)
            {
                return false;
            }
            return пользовательIdentity.Пользователь.ИмеетРоль(кодРоли);
        }

        #endregion

        /// <summary>
        /// конструктор  
        /// </summary>
        /// <param name="name"></param>
        /// <param name="repository"></param>
        public ПровайдерПользователя(string логин, IМенеджерБД менеджерБД)
        {
            пользовательIdentity = new ПользовательIdentity();
            пользовательIdentity.Инициализация(логин, менеджерБД);
        }


        /// <summary>
        /// Имя пользователя
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return пользовательIdentity.Name;
        }
    }
}
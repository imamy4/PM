using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

using Ninject;
using МенеджерБД;
using МенеджерБД.Домен;

namespace УправлениеПроектами.Global.Auth
{
    public class АутентификацияПользователя : IАутентификация
    {
        private const string наименованиеCookie = "__AUTH_COOKIE";

        public HttpContext HttpContext { get; set; }

        [Inject]
        public IМенеджерБД МенеджерБД { get; set; }

        #region Реализация IАутентификация

        public Пользователь Войти(string логин, string пароль, bool постояннаяАвторизация)
        {
            Пользователь авторизованыйПользователь = МенеджерБД.Авторизоваться(логин, пароль);
            if (авторизованыйПользователь != null)
            {
                СоздатьCookie(логин, постояннаяАвторизация);
            }
            return авторизованыйПользователь;
        }

        public Пользователь Войти(string логин)
        {
            Пользователь авторизованыйПользователь = МенеджерБД.ПолучитьПервуюЗаписьБД<Пользователь>(p => string.Compare(p.Email, логин, true) == 0);
            if (авторизованыйПользователь != null)
            {
                СоздатьCookie(логин);
            }
            return авторизованыйПользователь;
        }

        private void СоздатьCookie(string логин, bool постояннаяАвторизация = false)
        {
            var ticket = new FormsAuthenticationTicket(
                  1,
                  логин,
                  DateTime.Now,
                  DateTime.Now.Add(FormsAuthentication.Timeout),
                  постояннаяАвторизация,
                  string.Empty,
                  FormsAuthentication.FormsCookiePath);

            // шифруем тикет
            var encTicket = FormsAuthentication.Encrypt(ticket);

            // создаем куки
            var AuthCookie = new HttpCookie(наименованиеCookie)
            {
                Value = encTicket,
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };
            HttpContext.Response.Cookies.Set(AuthCookie);
        }

        public void Выйти()
        {
            var httpCookie = HttpContext.Response.Cookies[наименованиеCookie];
            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }
        }

        private IPrincipal _текущийПользователь;

        public IPrincipal ТекущийПользователь
        {
            get
            {
                if (_текущийПользователь == null)
                {
                    try
                    {
                        HttpCookie аутентификационныйCookie = HttpContext.Request.Cookies.Get(наименованиеCookie);
                        if (аутентификационныйCookie != null && !string.IsNullOrEmpty(аутентификационныйCookie.Value))
                        {
                            var ticket = FormsAuthentication.Decrypt(аутентификационныйCookie.Value);
                            _текущийПользователь = new ПровайдерПользователя(ticket.Name, МенеджерБД);
                        }
                        else
                        {
                            _текущийПользователь = new ПровайдерПользователя(null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        _текущийПользователь = new ПровайдерПользователя(null, null);
                    }
                }
                return _текущийПользователь;
            }
        }
        #endregion
    }
}
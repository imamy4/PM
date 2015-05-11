using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Ninject;
using УправлениеПроектами.Global.Auth;
using МенеджерБД;
using МенеджерБД.Домен;

namespace УправлениеПроектами.Controllers
{
    public abstract class BaseController : Controller
    {
        [Inject]
        public IМенеджерБД МенеджерБД { get; set; }

        [Inject]
        public IАутентификация Аутентификация { get; set; }

        public Пользователь ТекущийПользователь
        {
            get
            {
                return ((IПредоставительПользователя)Аутентификация.ТекущийПользователь.Identity).Пользователь;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Net;
using System.Text;
using System.Collections.Specialized;

namespace УправлениеПроектами.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View(ТекущийПользователь);
        }

        public ActionResult UserLogin()
        {
            return View(ТекущийПользователь);
        }

        public ActionResult Menu()
        {
            return View(ТекущийПользователь);
        }
    }
}

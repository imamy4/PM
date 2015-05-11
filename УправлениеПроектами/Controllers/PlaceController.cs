using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace СистемаРезервированияБилетов.Controllers
{
    public class PlaceController : BaseController
    {
        //
        // GET: /Place/

        public ActionResult Index()
        {
            return View(МенеджерБД.Залы);
        }

        public ActionResult Place(int id)
        {
            return View(МенеджерБД.Залы.FirstOrDefault(m => m.Id == id));
        }

        public ActionResult UserBar()
        {
            return View(ТекущийПользователь);
        }

    }
}

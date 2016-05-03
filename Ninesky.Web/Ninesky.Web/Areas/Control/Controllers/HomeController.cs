using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ninesky.Web.Areas.Control.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 主控制器
        /// </summary>
        /// <returns></returns>
        [AdminAuthorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}
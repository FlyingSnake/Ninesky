using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninesky.Core;
using Ninesky.Core.General;
using Ninesky.Web.Areas.Control.Models;

namespace Ninesky.Web.Areas.Control.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        private readonly AdministratorManager _adminManager = new AdministratorManager();

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                string passowrd = Security.Sha256(loginViewModel.Password);
                var response = _adminManager.Verify(loginViewModel.Accounts, passowrd);
                if (response.Code == 1)
                {
                    var admin = _adminManager.Find(loginViewModel.Accounts);
                    Session.Add("AdminID", admin.AdministratorId);
                    Session.Add("Accounts", admin.Accounts);
                    admin.LoginTime = DateTime.Now;
                    admin.LoginIp = Request.UserHostAddress;
                    _adminManager.Update(admin);
                    return RedirectToAction("Index", "Home");
                }
                else if (response.Code == 2) ModelState.AddModelError("Accounts", response.Message);
                else if (response.Code == 3) ModelState.AddModelError("Password", response.Message);
                else ModelState.AddModelError("", response.Message);
            }
            return View(loginViewModel);
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
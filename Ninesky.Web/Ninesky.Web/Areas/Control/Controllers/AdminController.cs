using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninesky.Core;
using Ninesky.Core.General;
using Ninesky.Core.Types;
using Ninesky.Web.Areas.Control.Models;

namespace Ninesky.Web.Areas.Control.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        private readonly AdministratorManager _adminManager = new AdministratorManager();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 管理员列表
        /// </summary>
        /// <returns></returns>
        public JsonResult ListJson()
        {
            return Json(_adminManager.FindList());
        }

        /// <summary>
        /// 添加【分部视图】
        /// </summary>
        /// <returns></returns>
        public PartialViewResult AddPartialView()
        {
            return PartialView();
        }

        /// <summary>
        /// 删除 
        /// Response.Code:1-成功，2-部分删除，0-失败
        /// Response.Data:删除的数量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteJson(List<int> ids)
        {
            int total = ids.Count();
            Response res = new Response();
            int currentAdminId = int.Parse(Session["AdminID"].ToString());
            if (ids.Contains(currentAdminId))
            {
                ids.Remove(currentAdminId);
            }
            res = _adminManager.Delete(ids);
            if (res.Code == 1 && res.Data < total)
            {
                res.Code = 2;
                res.Message = "共提交删除" + total + "名管理员,实际删除" + res.Data + "名管理员。\n原因：不能删除当前登录的账号";
            }
            else if (res.Code == 2)
            {
                res.Message = "共提交删除" + total + "名管理员,实际删除" + res.Data + "名管理员。";
            }
            return Json(res);
        }

        /// <summary>
        /// 重置密码【Ninesky】
        /// </summary>
        /// <param name="id">管理员ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ResetPassword(int id)
        {
            string password = "Ninesky";
            Response resp = _adminManager.ChangePassword(id, Security.Sha256(password));
            if (resp.Code == 1) resp.Message = "密码重置为：" + password;
            return Json(resp);
        }

        /// <summary>
        /// 我的资料
        /// </summary>
        /// <returns></returns>
        public ActionResult MyInfo()
        {
            return View(_adminManager.Find(Session["Accounts"].ToString()));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MyInfo(FormCollection form)
        {
            var admin = _adminManager.Find(Session["Accounts"].ToString());

            if (admin.Password != form["Password"])
            {
                admin.Password = Security.Sha256(form["Password"]);
                var resp = _adminManager.ChangePassword(admin.AdministratorId, admin.Password);
                if (resp.Code == 1)
                {
                    ViewBag.Message =
                        "<div class=\"alert alert-success\" role=\"alert\"><span class=\"glyphicon glyphicon-ok\"></span>修改密码成功！</div>";
                }
                else
                {
                    ViewBag.Message = "<div class=\"alert alert-danger\" role=\"alert\"><span class=\"glyphicon glyphicon-remove\"></span>修改密码失败！</div>";
                }
            }
            return View(admin);
        }

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
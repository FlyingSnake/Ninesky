using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninesky.Core;
using Ninesky.Web.Models;

namespace Ninesky.Web.Areas.Control.Controllers
{
    [AdminAuthorize]
    public class RoleController : Controller
    {
        private readonly RoleManager _roleManager = new RoleManager();

        // GET: Control/Role
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///  添加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="role">角色实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Role role)
        {
            if (ModelState.IsValid)
            {
                if (_roleManager.Add(role).Code == 1)
                {
                    return View("Prompt", new Prompt()
                    {
                        Title = "添加角色成功",
                        Message = "你已成功添加了角色【" + role.Name + "】",
                        Buttons = new List<string>() { "<a href=\"" + Url.Action("Index", "Role") + "\" class=\"btn btn-default\">角色管理</a>", "<a href=\"" + Url.Action("Add", "Role") + "\" class=\"btn btn-default\">继续添加</a>" }
                    });
                }
            }
            return View(role);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id">RoleID</param>
        /// <returns></returns>
        public ActionResult Modify(int id)
        {
            var role = _roleManager.Find(id);
            if (role == null) return View("Prompt", new Prompt()
            {
                Title = "错误",
                Message = "ID为【" + id + "】的角色不存在",
                Buttons = new List<string>() { "<a href=\"" + Url.Action("Index", "Role") + "\" class=\"btn btn-default\">角色管理</a>" }
            });
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modify(Role role)
        {
            if (ModelState.IsValid)
            {
                var resp = _roleManager.Update(role);
                if (resp.Code == 1) return View("Prompt",new Prompt()
                {
                    Title="角色修改成功",
                    Message="您已成功修改了角色【"+role.Name+"】",
                    Buttons=new List<string>()
                    {
                        "<a href=\""+Url.Action("Index","Role")+"\" class=\"btn btn-default\">角色管理</a>",
                        "<a href=\"" + Url.Action("Modify", "Role", new { id = role.RoleId }) + "\" class=\"btn btn-default\">查看</a>",
                        "<a href=\"" + Url.Action("Add", "Role") + "\" class=\"btn btn-default\">添加</a>"
                    }
                });
                else return View("Prompt", new Prompt()
                {
                    Title = "修改角色失败",
                    Message = "失败原因：" + resp.Message,
                    Buttons = new List<string>()
                    {
                        "<a href=\"" + Url.Action("Index", "Role") + "\" class=\"btn btn-default\">角色管理</a>",
                        "<a href=\"" + Url.Action("Modify", "Role", new { id = role.RoleId }) + "\" class=\"btn btn-default\">返回</a>"
                    }
                });
            }
            else return View(role);
        }

        /// <summary>
        /// 删除【Json】
        /// </summary>
        /// <param name="id">RoleID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteJson(int id)
        {
            return Json(_roleManager.Delete(id));
        }

        /// <summary>
        ///  列表【Json】
        /// </summary>
        /// <returns></returns>
        public JsonResult ListJson()
        {
            return Json(_roleManager.FindList());
        }
    }
}
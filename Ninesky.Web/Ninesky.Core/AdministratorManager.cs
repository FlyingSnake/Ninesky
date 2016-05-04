using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninesky.Core.Types;

namespace Ninesky.Core
{
    public class AdministratorManager : BaseManager<Administrator>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="admin">管理员实体</param>
        /// <returns></returns>
        public override Response Add(Administrator admin)
        {
            Response resp = new Response();
            if (HasAccounts(admin.Accounts))
            {
                resp.Code = 0;
                resp.Message = "帐号已存在";
            }
            else resp = base.Add(admin);
            return resp;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="administratorId">主键</param>
        /// <param name="password">新密码【密文】</param>
        /// <returns></returns>
        public Response ChangePassword(int administratorId, string password)
        {
            Response resp = new Response();
            var admin = Find(administratorId);
            if (admin == null)
            {
                resp.Code = 0;
                resp.Message = "该主键的管理员不存在";
            }
            else
            {
                admin.Password = password;
                resp = Update(admin);
            }
            return resp;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="administratorId">主键</param>
        /// <returns></returns>
        public override Response Delete(int administratorId)
        {
            Response resp = new Response();
            if (Count() == 1)
            {
                resp.Code = 0;
                resp.Message = "不能删除唯一的管理员帐号";
            }
            else resp = base.Delete(administratorId);
            return resp;
        }

        /// <summary>
        /// 删除【批量】返回值Code：1-成功，2-部分删除，0-失败
        /// </summary>
        /// <param name="administratorIdList"></param>
        /// <returns></returns>
        public Response Delete(List<int> administratorIdList)
        {
            Response resp = new Response();
            int totalDel = administratorIdList.Count;
            int totalAdmin = Count();
            foreach (int i in administratorIdList)
            {
                if (totalAdmin > 1)
                {
                    base.Repository.Delete(new Administrator() { AdministratorId = i }, false);
                    totalAdmin--;
                }
                else resp.Message = "最少需保留1名管理员";
            }
            resp.Data = base.Repository.Save();
            if (resp.Data == totalDel)
            {
                resp.Code = 1;
                resp.Message = "成功删除" + resp.Data + "名管理员";
            }
            else if (resp.Data > 0)
            {
                resp.Code = 2;
                resp.Message = "成功删除" + resp.Data + "名管理员";
            }
            else
            {
                resp.Code = 0;
                resp.Message = "删除失败";
            }
            return resp;
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="accounts">帐号</param>
        /// <returns></returns>
        public Administrator Find(string accounts)
        {
            return base.Repository.Find(a => a.Accounts == accounts);
        }

        /// <summary>
        /// 帐号是否存在
        /// </summary>
        /// <param name="accounts">帐号</param>
        /// <returns></returns>
        public bool HasAccounts(string accounts)
        {
            return base.Repository.IsContains(a => a.Accounts.ToUpper() == accounts.ToUpper());
        }

        /// <summary>
        /// 更新登录信息
        /// </summary>
        /// <param name="administratorId">主键</param>
        /// <param name="ip">IP地址</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public Response UpadateLoginInfo(int administratorId, string ip, DateTime time)
        {
            Response resp = new Response();
            var admin = Find(administratorId);
            if (admin == null)
            {
                resp.Code = 0;
                resp.Message = "该主键的管理员不存在";
            }
            else
            {
                admin.LoginIp = ip;
                admin.LoginTime = time;
                resp = Update(admin);
            }
            return resp;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="accounts">帐号</param>
        /// <param name="password">密码【密文】</param>
        /// <returns>Code:1-成功;2-帐号不存在;3-密码错误</returns>
        public Response Verify(string accounts, string password)
        {
            Response resp = new Response();
            var admin = base.Repository.Find(a => a.Accounts == accounts);
            if (admin == null)
            {
                resp.Code = 2;
                resp.Message = "帐号为:【" + accounts + "】的管理员不存在";
            }
            else if (admin.Password == password)
            {
                resp.Code = 1;
                resp.Message = "验证通过";
            }
            else
            {
                resp.Code = 3;
                resp.Message = "帐号密码错误";
            }
            return resp;
        }
    }
}

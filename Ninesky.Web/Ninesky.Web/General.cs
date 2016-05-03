using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Ninesky.Web
{
    /// <summary>
    /// 通用类
    /// </summary>
    public class General
    {
        /// <summary>
        /// 获取模型错误
        /// </summary>
        /// <param name="modelState">模型状态</param>
        /// <returns></returns>
        public static string GetModelErrorString(ModelStateDictionary modelState)
        {
            StringBuilder sb = new StringBuilder();
            var errorModelState = modelState.Where(m => m.Value.Errors.Any());
            foreach (var item in errorModelState)
            {
                foreach (var modelError in item.Value.Errors)
                {
                    sb.AppendLine(modelError.ErrorMessage);
                }
            }
            return sb.ToString();
        }
    }
}
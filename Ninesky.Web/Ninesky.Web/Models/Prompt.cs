using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninesky.Web.Models;

namespace Ninesky.Web.Models
{
    /// <summary>
    /// 提示类
    /// </summary>
    public class Prompt
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 按钮组
        /// </summary>
        public List<string> Buttons { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Ninesky.Core
{
    /// <summary>
    /// 项目的数据数据上下文，使模型和数据库的表进行对应
    /// </summary>
    public class NineskyContext:DbContext
    {
        /// <summary>
        /// 管理员集合
        /// </summary>
        public DbSet<Administrator> Administrators { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        public NineskyContext():base("DefaultConnection")
        {
            Database.SetInitializer<NineskyContext>(new CreateDatabaseIfNotExists<NineskyContext>());
        }
    }
}

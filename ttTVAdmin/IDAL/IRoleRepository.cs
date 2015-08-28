using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IDAL
{
    public interface IRoleRepository
    {
        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        object Get();
        Task<IdentityResult> Add(IdentityRole role);
    }
}

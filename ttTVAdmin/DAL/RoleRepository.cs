using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL
{
    class RoleRepository : IRoleRepository
    {
        System.Web.Mvc.ModelStateDictionary state = new System.Web.Mvc.ModelStateDictionary();
        private readonly RoleManager<IdentityRole> _manager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        public object Get()
        {
            IQueryable<IdentityRole> roles = _manager.Roles;
            var data = new { data = roles.ToViewModels() };
            return data;
        }
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Add(IdentityRole role)
        {
            IdentityRole NewRole = new IdentityRole();
            if (state.IsValid)
            {
                NewRole.Name = role.Name;
            }
            var result = await _manager.CreateAsync(NewRole);
            return result;
        }
    }
}

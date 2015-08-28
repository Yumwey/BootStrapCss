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
    class UserRepository:IUserRepository
    {
        private readonly UserManager<IdentityUser> _manager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <returns></returns>
        public object Get()
        {
            IQueryable<IdentityUser> users = _manager.Users;
            var data = new { data = users.ToViewModels() };
            return data;
        }
    }
}

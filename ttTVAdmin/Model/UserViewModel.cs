using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public  class UserViewModel
    {
        //显示系统用户的信息
        public string Id{get;set;}
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Other { get; set; }
    }
    public static class UserExtension
    {
        public static UserViewModel ToViewModel(this IdentityUser user)
        {
            string Delete="<a class='btn btn-danger' href='javascript:void(0);'><i class='fa fa-remove'></i> 删除</a></td>";
            string Editor = "<a class='btn btn-success' href='javascript:void(0);'><i class='fa fa-cog'></i> 编辑</a>";
            string Add = "<a class='btn btn-primary' href='javascript:void(0);'><i class='fa fa-cog'></i> 分配角色</a>";
            UserViewModel userView = new UserViewModel()
            {
                Id=user.Id,
                Name=user.UserName,
                Email=user.Email,
                PhoneNumber=user.PhoneNumber,
                Other = string.Format(Delete + Editor + Add)
            };
            return userView;
        }
        public static List<UserViewModel> ToViewModels(this IQueryable<IdentityUser> model)
        {
            List<UserViewModel> models = new List<UserViewModel>();
            foreach (IdentityUser user in model)
            {
                models.Add(user.ToViewModel());
            }
            return models;
        }
    }
}

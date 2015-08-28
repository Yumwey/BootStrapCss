using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Other { get; set; }
    }
    public static class RoleExtension
    {
        public static RoleViewModel ToViewModel(this IdentityRole role)
        {
            string Editor = "<a class='btn btn-success' href='javascript:void(0);'><i class='fa fa-cog'></i> 编辑</a>";
            string Delete = "<a class='btn btn-danger' href='javascript:void(0);'><i class='fa fa-remove'></i> 删除</a></td>";
            RoleViewModel model = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name,
                Other = string.Format(Delete+Editor),
            };
            return model;
        }
        public static List<RoleViewModel> ToViewModels(this IQueryable<IdentityRole> role)
        {
            List<RoleViewModel> models = new List<RoleViewModel>();
            foreach (IdentityRole model in role)
            {
                models.Add(model.ToViewModel());
            }
            return models;
        }
    }
}

using SmartAdminMvc.App_Helpers;
using SmartAdminMvc.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ttTVAdmin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DAL;
using IDAL;
using System.Threading.Tasks;


namespace SmartAdminMvc.Controllers
{
    [UserAttribute]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> manager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
        private IRoleRepository IRole = DALFactory.CreateRole();
        public ActionResult RoleManager()
        {         
            return View();
        }
        //获取系统角色
        public JsonResult GetRoles()
        {
            var data = IRole.Get();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        public ActionResult NewRoles(IdentityRole role)
        {           
            var result = IRole.Add(role);
            // 添加失败的情况
            if (result.Exception!=null)
            {
                return View(role);
            }
            else
                return RedirectToAction("RoleManager", "Roles");
        }
        //新增删除的方法
        //新增编辑的方法
	}
}
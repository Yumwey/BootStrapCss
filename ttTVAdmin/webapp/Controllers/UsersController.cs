using Microsoft.AspNet.Identity.EntityFramework;
using SmartAdminMvc.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ttTVAdmin;
using IDAL;
using DAL;
namespace SmartAdminMvc.Controllers
{
    [UserAttribute]
    public class UsersController : Controller
    {        
        private static IUserRepository IUser = DALFactory.CreateUser();
        //public IdentityDbContext IdentityDb = new IdentityDbContext();      
        // GET: /Users/
        //[Authorize(Roles = "Admin")添加自定义角色验证
        //[AdminAttribute]
        public  ActionResult ManageUsers()
        {           
            return View();
        }
        //获取系统用户
        //[Authorize(Roles = "Admin")]
        public JsonResult GetUsers()
        {
            //使用Identity自带UserManager获取系统用户
            var data = IUser.Get();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //新增编辑的方法
        //新增删除的方法
	}
}
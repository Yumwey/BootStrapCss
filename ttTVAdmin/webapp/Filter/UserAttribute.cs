using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace SmartAdminMvc.Filter
{
    /// <summary>        
    ///过滤器：针对角色过滤Action的执行与否
    ///功能：过滤掉未登陆用户，当用户未登录时，无法进入其他页面，跳转到登陆页面
    /// </summary>
    /// <param name="filterContext"></param>
    public class UserAttribute : AuthorizeAttribute
    {
        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //}

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            
            string user = httpContext.User.Identity.Name;
            if (user == null || !httpContext.User.Identity.IsAuthenticated)
            {
                httpContext.Response.Write("<script>alert('请先登录！')</script>");
                return false;
            }
            else
                return base.AuthorizeCore(httpContext);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.HttpContext.Response.Write("<script>alert('请先登录！')</script>");
            //设置时间延迟
            //System.Threading.Thread.Sleep(2000);
            filterContext.Result = new RedirectResult("Account/login");
        }              
    }
}
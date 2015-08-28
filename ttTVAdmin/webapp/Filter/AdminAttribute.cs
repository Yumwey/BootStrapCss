using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmartAdminMvc.Filter
{
    /// <summary>
    /// 管理员过滤器，过滤非管理人员执行Action
    /// </summary>
    public class AdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        { //在Action执行前执行
            //此处获取用户角色：成功则执行，失败不执行
            //ErrorRedirect(filterContext); 
            if (HttpContext.Current.User.IsInRole("Admin"))
            {
                //可以执行
            }
            else
            {
                //否则失败
            }
            base.OnActionExecuting(filterContext);
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {           
            filterContext.HttpContext.Response.Write("<script>alert('你不是管理员！');</script>");
            filterContext.Result = new RedirectResult("index/home");
        }
        //错误处理方法
        //private void ErrorRedirect(ActionExecutingContext filterContext)
        //{
        //    //filterContext.HttpContext.Response.Write("<script>alert('你不是管理员！');</script>");
        //    filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Home", action = "index" }));
        //} // end ErrorRed
    }
}
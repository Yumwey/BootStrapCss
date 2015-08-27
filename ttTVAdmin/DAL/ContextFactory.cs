using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using Models;

namespace DAL
{
    public class ContextFactory
    {
        /// <summary>
        /// 获取当前数据库上下文:简单工厂获取当前DbContext,实现单个请求之间的DbContext单例
        /// </summary>
        /// <returns></returns>
        public static ServiceDeskContext GetCurrentContext()
        {
            ServiceDeskContext _context = CallContext.GetData("TicketsContext") as ServiceDeskContext;
            if (_context == null)
            {
                _context = new ServiceDeskContext();
                CallContext.SetData("TicketsContext", _context);
            }
            return _context;
        }
    }
}
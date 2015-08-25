using SmartAdminMvc.IDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SmartAdminMvc.DAL
{
    public class DALFactory
    {
        //获取到对应的具体实现方法
        public static string name = ConfigurationManager.AppSettings["First"].ToString();
        public static string path = ConfigurationManager.AppSettings["Second"].ToString();

        public static InterfaceTicketsRepository CreateTickets()
        {
            string className = name + ".Tickets" + path;
            return (InterfaceTicketsRepository)Assembly.Load(name).CreateInstance(className);
        }


    }
}
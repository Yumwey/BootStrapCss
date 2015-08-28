using IDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DAL
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
        public static InterfaceCommentsRepository CreateTicketsComments()
        {
            string classname = name + ".TicketsComments" + path;
            return (InterfaceCommentsRepository)Assembly.Load(name).CreateInstance(classname);
        }
        public static InterfaceTicketsAttachmentsRepository CreateAttachment()
        {
            string classname = name + ".TicketsAttachment" + path;
            return (InterfaceTicketsAttachmentsRepository)Assembly.Load(name).CreateInstance(classname);
        }
        public static IUserRepository CreateUser()
        {
            string classname = name + ".User" + path;
            return (IUserRepository)Assembly.Load(name).CreateInstance(classname);
        }
        public static IRoleRepository CreateRole()
        {
            string classname = name + ".Role" + path;
            return (IRoleRepository)Assembly.Load(name).CreateInstance(classname);
        }
        
    }
}
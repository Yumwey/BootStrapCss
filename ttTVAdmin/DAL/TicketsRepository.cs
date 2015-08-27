#region Using

using System.Linq;
using System.Web.Mvc;
//using System.Web.WebPages.Html;
using Models;
//using Models;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Web;
using System.IO;
using System.Collections;
using SelectListItem = System.Web.Mvc.SelectListItem;
using IDAL;
using System.Security.Principal;
using System.Data.Entity;
using Newtonsoft.Json;


//using System;
//using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
using System.Globalization;
//using System.Web.Mvc;
//using System.Web.Security;


#endregion

namespace DAL
{
    public class TicketsRepository : InterfaceTicketsRepository
    {
        System.Web.Mvc.ModelStateDictionary state = new System.Web.Mvc.ModelStateDictionary();
        //使用数据工厂，获取每次请求的数据单例
        private ServiceDeskContext DbTickets = ContextFactory.GetCurrentContext();
        /// <summary>
        /// 创建新的Tickets
        /// </summary>
        /// <param name="viewmodel"></param>
        /// <returns></returns>
        public int Create(TicketCreationModel viewmodel, string Name)
        {
            int returnValue = -1;
            Ticket ticket;
            //ticket.TicketId
            // Ensure we have a valid viewModel to work with
            if (state.IsValid)
            {
                DateTime now = DateTime.Now;
                //string user =this.User.Identity.Name;
                ticket = new Ticket()
                {
                    AffectsCustomer = viewmodel.AffectsCustomer,
                    Category = viewmodel.Category,
                    CreatedBy = Name,
                    CreatedDate = now,
                    CurrentStatus = "Active",
                    CurrentStatusDate = now,
                    CurrentStatusSetBy = Name,
                    Details = viewmodel.Details,
                    IsHtml = false,
                    LastUpdateBy = Name,
                    LastUpdateDate = now,
                    Priority = viewmodel.Priority,
                    PublishedToKb = false,
                    TagList = viewmodel.TagList,
                    Title = viewmodel.Title,
                    Type = viewmodel.Type,
                    Owner = viewmodel.OtherOwner ? viewmodel.Owner : Name
                };
                DbTickets.Tickets.Add(ticket);
                ServiceDeskContext context = new ServiceDeskContext();

                DbTickets.SaveChanges();
                returnValue = ticket.TicketId;
                //return View(viewModel);
            }
            return returnValue;
        }
        //获取总数
        public int Count()
        {
            int count = DbTickets.Tickets.Where(t => t.AssignedTo == null).Count();

            return count;
        }
        //获取所有Tickets
        //定义虚方法，运行时决定是否调用，实现动态绑定（多态）
        public virtual TicketCreationModel Get(int id)
        {
            TicketCreationModel model = new TicketCreationModel();
            //IQueryable<Ticket> ticket = DbTickets.Tickets;
            return model;
        }
        //获取未分配任务
        public object GetUnassigned()
        {
            var tickets = DbTickets.Tickets.Where(t => t.AssignedTo == null);
            var data = new { data = tickets.ToViewModels(true, true, true) };
            return data;
        }
        //获取已经分配任务
        public object GetAssigned(string user)
        {
            IQueryable<Ticket> tickets;
            if (string.IsNullOrEmpty(user))
                tickets = DbTickets.Tickets.Where(t => string.IsNullOrEmpty(t.AssignedTo) == false && t.CurrentStatus != "Closed");
            else
                tickets = DbTickets.Tickets.Where(t => string.IsNullOrEmpty(t.AssignedTo) == false && t.AssignedTo.Equals(user) && t.CurrentStatus != "Closed");

            var data = new { data = tickets.ToViewModels(true, true, true) };
            return data;
        }
        //获取任务状态：当前用户
        public object GetByStatus(string user, string status)
        {
            IQueryable<Ticket> tickets;
            if (string.IsNullOrEmpty(user))
            {
                if (string.IsNullOrEmpty(status))
                    tickets = DbTickets.Tickets;
                else
                    tickets = DbTickets.Tickets.Where(t => status.Equals(t.CurrentStatus));
            }
            else
                tickets = DbTickets.Tickets.Where(t =>
                    string.IsNullOrEmpty(t.AssignedTo) == false && t.AssignedTo.Equals(user) &&
                    (string.IsNullOrEmpty(status) || status.Equals(t.CurrentStatus))
                    );

            var data = new { data = tickets.ToViewModels(true, true, true) };
            return data;
        }
    }
}
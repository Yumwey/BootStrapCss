#region Using

using System.Linq;
using System.Web.Mvc;

using System.Web.WebPages.Html;
using ttTVAdmin.Models;
using ttTVMS.Models;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Web;
using System.IO;
using System.Collections;
using SelectListItem = System.Web.Mvc.SelectListItem;
using SmartAdminMvc.IDAL;
using System.Security.Principal;

#endregion

namespace SmartAdminMvc.DAL
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

                DbTickets.SaveChanges();

                returnValue = ticket.TicketId;
                //return View(viewModel);
            }
            return returnValue;
           
        }
    }
}
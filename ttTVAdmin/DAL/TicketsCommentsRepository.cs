using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using Models;
using System.Web.UI;

namespace DAL
{
    class TicketsCommentsRepository:InterfaceCommentsRepository
    {
        //获取当前数据库上下文的单例，一次任务对应一次数据
        private ServiceDeskContext db = ContextFactory.GetCurrentContext();
        //定义虚方法，需要时进行Override覆写
        public virtual int Count()
        {
            throw new NotImplementedException();
        }
        //获取当前任务的评论
        public IQueryable<TicketComment> Get(int id)
        {
            var comments = db.TicketComments.Where(c => c.TicketId == id).OrderByDescending(c => c.CommentId);
            return comments;
        }
        //添加新的评论
        public string Add(long id, string comment,string user)
        {
            string result = null;
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
                result = "Ticket not found.";
            else
            {
                TicketComment tcomment = new TicketComment();
                DateTime now = DateTime.Now;

                tcomment.CommentEvent = string.Format("added comment");
                tcomment.IsHtml = false;
                tcomment.CommentedBy = user;
                tcomment.CommentedDate = now;
                tcomment.Comment = System.Web.HttpUtility.HtmlEncode(comment).Trim(); //对数据进行编码，防止脚本注入式攻击
                tcomment.TicketId = ticket.TicketId;
                db.TicketComments.Add(tcomment);
                db.SaveChanges();
            }
            return result;
        }
    }
}

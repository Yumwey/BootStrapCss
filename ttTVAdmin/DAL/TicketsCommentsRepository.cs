using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using Models;

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
    }
}

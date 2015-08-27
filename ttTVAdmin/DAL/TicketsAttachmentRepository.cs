using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using Models;

namespace DAL
{
    class TicketsAttachmentRepository:InterfaceTicketsAttachmentsRepository
    {
        //创建数据上下文单线程
        private ServiceDeskContext db = ContextFactory.GetCurrentContext();

        //定义虚方法
        public virtual int Count()
        {
            throw new NotImplementedException();
        }
        //获取当前附件
        public IQueryable<TicketAttachment> Get(int id)
        {
            var files = db.TicketAttachments.Where(a => a.TicketId == id).OrderByDescending(c => c.FileId);
            return files;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using Models;
using System.Web.Mvc;

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
        //显示当前附件
        public TicketAttachment GetAttachment(int id, int? size)
        {
            TicketAttachment file = db.TicketAttachments.Find(id);
            return file;
        }
        //获取文件流
        public byte[] GetAttachmentStream(int id, int? size)
        {
            TicketAttachment file = GetAttachment(id, size);
            byte[] data = file.FileContents;

            if (size.HasValue)
            {
                switch (size)
                {
                    case 1:
                        data = ImageManager.ResizeImageFile(data, ImageManager.PhotoSize.Small);
                        break;
                    case 2:
                        data = ImageManager.ResizeImageFile(data, ImageManager.PhotoSize.Medium);
                        break;
                    case 3:
                        data = ImageManager.ResizeImageFile(data, ImageManager.PhotoSize.Full);
                        break;
                }
            }
            return data;
        }
        //上传文件
        //public 
    }  
}

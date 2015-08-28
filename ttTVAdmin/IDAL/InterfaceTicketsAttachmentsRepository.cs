using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Web.Mvc;

namespace IDAL
{
    public interface InterfaceTicketsAttachmentsRepository : InterfaceBaseRepository<IQueryable<TicketAttachment>>
    {
        /// <summary>
        /// 显示有附件的任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        TicketAttachment GetAttachment(int id, int? size);
        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="id"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        byte[] GetAttachmentStream(int id, int? size);
        /// <summary>
        /// 任务的附件上传
        /// </summary>
        /// <returns></returns>
        //ActionResult Upload();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Web.UI;

namespace IDAL
{
    public interface InterfaceCommentsRepository:InterfaceBaseRepository<IQueryable<TicketComment>>
    {
        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        string Add(long id, string comment,string user);
    }
}

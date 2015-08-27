using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
//using Models;

namespace IDAL
{
   public interface InterfaceTicketsRepository : InterfaceBaseRepository<TicketCreationModel>
    {
        //此接口定义了接口的仓库，属于基础接口
        /// <summary>
        /// 增加：返回为T的泛型类型，接受参数entity
        /// </summary>
        /// <returns></returns>
       int Create(TicketCreationModel entity, string Name);
       /// <summary>
       /// 获取未分配任务
       /// </summary>
       /// <returns></returns>
        object GetUnassigned();
       /// <summary>
       /// 用途：获取已经分配任务
       /// 参数：当前用户名称
       /// 返回：匿名类型或Object
       /// </summary>
       /// <returns></returns>
        object GetAssigned(string user);
       /// <summary>
       /// 用途：获取任务状态
       /// 参数：当前用户，任务状态
       /// 返回：Object
       /// </summary>
       /// <param name="user"></param>
       /// <param name="status"></param>
       /// <returns></returns>
        object GetByStatus(string user, string status);
    }
}

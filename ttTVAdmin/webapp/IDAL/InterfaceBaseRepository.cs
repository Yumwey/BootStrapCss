using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SmartAdminMvc.IDAL
{
    public interface InterfaceBaseRepository<T>
    {
        //此接口定义了接口的仓库，属于基础接口
        /// <summary>
        /// 增加：返回为T的泛型类型，接受参数entity
        /// </summary>
        /// <returns></returns>
         int Create(T entity,string Name);
        /// <summary>
        /// 更新：返回T泛型，接收具体参数entity
        /// </summary>
        /// <returns></returns>
        // T Updata(T entity);
        ///// <summary>
        ///// 删除：返回泛型T,接收具体参数entity
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        // T Delete(T entity);
        ///// <summary>
        ///// 查看详情：返回泛型T,接收具体参数entity
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        // T Detail(T entity);

    }
}

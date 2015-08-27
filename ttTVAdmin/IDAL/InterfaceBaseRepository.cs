using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDAL
{
    public interface InterfaceBaseRepository<T>
    {
        //<summary>
        //统计数量
        //</summary>
        //<returns></returns>
        int Count();
        //<summary>
        //显示所有指定内容       
        //</summary>
        //<returns></returns>
        T Get(int id);
    }
}

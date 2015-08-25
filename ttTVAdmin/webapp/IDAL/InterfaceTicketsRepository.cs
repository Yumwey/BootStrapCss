using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ttTVAdmin.Models;
using ttTVMS.Models;

namespace SmartAdminMvc.IDAL
{
   public interface InterfaceTicketsRepository : InterfaceBaseRepository<TicketCreationModel>
    {
        //重写创建方法
        //int Create(TicketCreationModel viewmodel, string Name);
    }
}

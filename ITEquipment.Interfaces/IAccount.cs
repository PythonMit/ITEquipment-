using ITEquipment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Interfaces
{
   public interface IAccount
    {
        Task<UserMasterViewModel> GetLoginUserDetail(string email, string password);
        Task<UserMasterViewModel> GetLoginUserDetailByEmail(string email);
        Task<UserListModel> GetAllUserList(int DisplayLength, int DisplayStart, string SortColumnName, string SortDirection, string SearchText, string UserSearch, string email, string role );
        Task<bool> AddUser(UserMasterDTOModel userMasterDTO);
    }
}

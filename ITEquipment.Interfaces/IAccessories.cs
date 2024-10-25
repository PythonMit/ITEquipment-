using ITEquipment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Interfaces
{
   public interface IAccessories
    {
        Task<AccessoryListModel> GetAllAccessories(int DisplayLength, int DisplayStart, string SortColumnName, string SortDirection, string SearchText, string AccessoryName , string ModelNumber, string SerialNumber);
        Task<bool> AddAccessories(AccessoriesViewModel accessories);
        Task<bool> UpdateAccessories(AccessoriesViewModel accessories);
        Task<bool> RemoveAccessories(int accessoriesId);
        Task<AccessoriesViewModel> GetAccessoriesById(int accessoriesId);
        Task<IEnumerable<AccessoriesViewModel>> SearchAccessory(string serchData);
    }
}

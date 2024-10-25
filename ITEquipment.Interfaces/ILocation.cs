using ITEquipment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Interfaces
{
   public interface ILocation
    {
        Task<LocationModelListModel> GetAllLocation(int DisplayLength, int DisplayStart, string SortColumnName, string SortDirection, string SearchText, string LoactionName);
        Task<bool> AddLocation(LocationModel locationModel);
        Task<bool> UpdateLocation(LocationModel locationModel);
        Task<bool> Removelocation(int locationId);
        Task<LocationModel> GetlocationById(int locationId);
    }
}

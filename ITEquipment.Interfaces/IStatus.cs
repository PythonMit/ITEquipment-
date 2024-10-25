using ITEquipment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;

namespace ITEquipment.Interfaces
{
   public interface IStatus
    {
      Task<StatusViewModel> GetStatus();
        Task<List<SelectListItem>> GetEquipmentStatusType(string selectedVal);
        Task<List<SelectListItem>> GetEquipmentMasterStatus(string selectedVal);
        Task<List<SelectListItem>> GetLocation(string selectedVal);
        Task<List<SelectListItem>> GetRoles(string selectedVal);
        Task<bool> UpdateUserRole(int UserId, int RoleId);
        Task<List<SelectListItem>> GetManufacturers(string selectedVal);
    }
}

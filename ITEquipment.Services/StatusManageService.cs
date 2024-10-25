using ITEquipment.Interfaces;
using ITEquipment.Models;
using ITEquipment.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ITEquipment.Services
{
    public class StatusManageService : IStatus
    {
        public string _Constring;
        private IConfiguration _configuration;
        public StatusManageService(IConfiguration configuration)
        {
            _Constring = configuration.GetConnectionString("DefaultConnection");
            _configuration = configuration;
        }

        public Task<StatusViewModel> GetStatus()
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                StatusViewModel objList = new StatusViewModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_STATUS, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        objList.statusLists = Util.DataReaderMapToList<StatusList>(dataReader);

                    }
                    cmd.Connection.Close();
                    return Task.FromResult(objList);
                }
            }
        }

        public async Task<List<SelectListItem>> GetEquipmentStatusType(string selectedVal = "0")
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            objList.Add(new SelectListItem
            {
                Value = "0",
                Text = "-- Select Status --",
                Selected = (selectedVal == "0")
            });
            using (SqlConnection sqlConnection = new SqlConnection(_Constring))
            {
                await sqlConnection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_STATUS, sqlConnection))
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        objList.Add(new SelectListItem
                        {
                            Value = rdr["Id"].ToString(),
                            Text = rdr["StatusType"].ToString(),
                            Selected = (selectedVal == rdr["Id"].ToString())
                        });
                    }
                }
                sqlConnection.Close();
            }
            return objList;
        }

        public async Task<List<SelectListItem>> GetEquipmentMasterStatus(string selectedVal = "0")
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            objList.Add(new SelectListItem
            {
                Value = "0",
                Text = "-- Select Status --",
                Selected = (selectedVal == "0")
            });
            using (SqlConnection sqlConnection = new SqlConnection(_Constring))
            {
                await sqlConnection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENTSTATUS, sqlConnection))
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        objList.Add(new SelectListItem
                        {
                            Value = rdr["Id"].ToString(),
                            Text = rdr["StatusType"].ToString(),
                            Selected = (selectedVal == rdr["Id"].ToString())
                        });
                    }
                }
                sqlConnection.Close();
            }
            return objList;
        }

        public async Task<List<SelectListItem>> GetLocation(string selectedVal = "0")
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            objList.Add(new SelectListItem
            {
                Value = "0",
                Text = "-- Select Location --",
                Selected = (selectedVal == "0")
            });
            using (SqlConnection sqlConnection = new SqlConnection(_Constring))
            {
                await sqlConnection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_LOCATIONS, sqlConnection))
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        objList.Add(new SelectListItem
                        {
                            Value = rdr["Id"].ToString(),
                            Text = rdr["LocationName"].ToString(),
                            Selected = (selectedVal == rdr["Id"].ToString())
                        });
                    }
                }
                sqlConnection.Close();
            }
            return objList;
        }
        public async Task<List<SelectListItem>> GetManufacturers(string selectedVal = "0")
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            objList.Add(new SelectListItem
            {
                Value = "0",
                Text = "-- Select Manufacturer --",
                Selected = (selectedVal == "0")
            });
            using (SqlConnection sqlConnection = new SqlConnection(_Constring))
            {
                await sqlConnection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANFACTURERS, sqlConnection))
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        objList.Add(new SelectListItem
                        {
                            Value = rdr["Id"].ToString(),
                            Text = rdr["ManufacturerName"].ToString(),
                            Selected = (selectedVal == rdr["Id"].ToString())
                        });
                    }
                }
                sqlConnection.Close();
            }
            return objList;
        }
        public async Task<List<SelectListItem>> GetRoles(string selectedVal = "0")
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            objList.Add(new SelectListItem
            {
                Value = "0",
                Text = "-- Select Role --",
                Selected = (selectedVal == "0")
            });
            using (SqlConnection sqlConnection = new SqlConnection(_Constring))
            {
                await sqlConnection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_ROLES, sqlConnection))
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        objList.Add(new SelectListItem
                        {
                            Value = rdr["Id"].ToString(),
                            Text = rdr["Name"].ToString(),
                            Selected = (selectedVal == rdr["Id"].ToString())
                        });
                    }
                }
                sqlConnection.Close();
            }
            return objList;
        }
        public Task<bool> UpdateUserRole(int UserId, int RoleId)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {

                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_UPDATEUSERROLE, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@RoleId", RoleId);
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }

        }
    }
}

using ITEquipment.Interfaces;
using ITEquipment.Models;
using ITEquipment.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Services
{
   public class AccessoriesManageService : IAccessories
    {
        public string _Constring;
        private IConfiguration _configuration;

        public AccessoriesManageService(IConfiguration configuration)
        {
            _configuration = configuration;
            _Constring = configuration.GetConnectionString("DefaultConnection");
        }
        public Task<bool> AddAccessories(AccessoriesViewModel accessories)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                AccessoriesViewModel objList = new AccessoriesViewModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGEACCESSORIES, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 50)).Value = "Add";
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50)).Value = accessories.Name;
                    cmd.Parameters.Add(new SqlParameter("@ModelNo", SqlDbType.NVarChar, 50)).Value = accessories.ModelNo;
                    cmd.Parameters.Add(new SqlParameter("@SerialNo", SqlDbType.NVarChar, 50)).Value = accessories.SerialNo;
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }
           
        }

        public async Task<AccessoriesViewModel> GetAccessoriesById(int accessoriesId)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                AccessoriesViewModel objList = new AccessoriesViewModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGEACCESSORIES, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", accessoriesId);
                    cmd.Parameters.AddWithValue("@Action", "AccessoriesById");
                    cmd.Connection.Open();
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    if (rdr.Read())
                    {
                        objList.Id = Convert.ToInt32(rdr["Id"]);
                        objList.Name = rdr["Name"].ToString();
                        objList.ModelNo = rdr["ModelNo"].ToString();
                        objList.SerialNo = rdr["SerialNo"].ToString();
                    }
                    con.Close();
                }

                return objList;
            }

            throw new NotImplementedException();
        }

        public async Task<AccessoryListModel> GetAllAccessories(int DisplayLength, int DisplayStart, string SortColumnName, string SortDirection, string SearchText, string AccessoryName, string ModelNumber, string SerialNumber)
        {
            try
            {
                if (SearchText == null)
                    SearchText = "";
                AccessoryListModel accessoriesList = new AccessoryListModel();
            
                using (SqlConnection con = new SqlConnection(_Constring))
                {
                    using (SqlCommand cmd = new SqlCommand(ModuleList.USP_GETACCESSORIES, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CurrentPage", DisplayStart);
                        cmd.Parameters.AddWithValue("@PageSize", DisplayLength);
                        cmd.Parameters.AddWithValue("@ColumnName", SortColumnName);
                        cmd.Parameters.AddWithValue("@SortOrder", SortDirection);
                        cmd.Parameters.AddWithValue("@Search", SearchText);
                        cmd.Parameters.AddWithValue("@AccessoryName", AccessoryName);
                        cmd.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                        cmd.Parameters.AddWithValue("@ModelNumber", ModelNumber);
                        SqlParameter paramOutPut = new SqlParameter("@noOfRecords", SqlDbType.Int);
                        paramOutPut.Size = 15;
                        paramOutPut.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(paramOutPut);
                        cmd.Connection.Open();

                        using (IDataReader dataReader = cmd.ExecuteReader())
                        {
                            accessoriesList.AccessoryList = Util.DataReaderMapToList<AccessoriesViewModel>(dataReader);
                            //accessoriesList.AccessoryList.ForEach(x => x.Id = x.Id.ToString().Encrypt());
                        }
                        if (cmd.Parameters["@noOfRecords"].Value != DBNull.Value)
                            accessoriesList.noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                        else
                            accessoriesList.noOfRecords = 0;
                        con.Close();
                        return accessoriesList;
                    }
                }
             
            }
            catch (Exception)
            {

                throw;
            }
           
           

        }

        public Task<bool> RemoveAccessories(int accessoriesId)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                AccessoriesViewModel objList = new AccessoriesViewModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGEACCESSORIES, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 50)).Value = "Delete";
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = accessoriesId;
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AccessoriesViewModel>> SearchAccessory(string serchData)
        {
            List<AccessoriesViewModel> accessoriesList = new List<AccessoriesViewModel>();
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGEACCESSORIES, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "List");
                    await con.OpenAsync();

                    using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                    {
                       
                        while (rdr.Read())
                        {
                            var Name = rdr["Name"].ToString();
                            var ModelNo = rdr["ModelNo"].ToString();
                            var SerialNo = rdr["SerialNo"].ToString();
                            if(string.IsNullOrEmpty(serchData) || Name.Contains(serchData)|| ModelNo.Contains(serchData) || SerialNo.Contains(serchData))
                            {
                                AccessoriesViewModel obList = new AccessoriesViewModel
                                {
                                    Id = Convert.ToInt32(rdr["Id"]),
                                    Name = rdr["Name"].ToString(),
                                    ModelNo = rdr["ModelNo"].ToString(),
                                    SerialNo = rdr["SerialNo"].ToString()
                                };
                                accessoriesList.Add(obList);
                            }
                        }
                    }
                }

                con.Close();
            }
            return accessoriesList;

            throw new NotImplementedException();
        }

        public Task<bool> UpdateAccessories(AccessoriesViewModel accessories)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                AccessoriesViewModel objList = new AccessoriesViewModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGEACCESSORIES, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 50)).Value = "Update";
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = accessories.Id;
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50)).Value = accessories.Name;
                    cmd.Parameters.Add(new SqlParameter("@ModelNo", SqlDbType.NVarChar, 50)).Value = accessories.ModelNo;
                    cmd.Parameters.Add(new SqlParameter("@SerialNo", SqlDbType.NVarChar, 50)).Value = accessories.SerialNo;
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }
            throw new NotImplementedException();
        }
    }
}

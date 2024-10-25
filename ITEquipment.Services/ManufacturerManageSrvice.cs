using ITEquipment.Interfaces;
using ITEquipment.Models;
using ITEquipment.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace ITEquipment.Services
{
    public class ManufacturerManageSrvice : IManufacturer
    {
        public string _Constring;
        private IConfiguration _configuration;

        public ManufacturerManageSrvice(IConfiguration configuration)
        {
            _configuration = configuration;
            _Constring = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<ManufacturerNameListModel> GetAllManufacturer(int DisplayLength, int DisplayStart, string SortColumnName, string SortDirection, string SearchText, string ManufacturerName)
        {
            try
            {
                if (SearchText == null)
                    SearchText = "";
                ManufacturerNameListModel ManufacturerList = new ManufacturerNameListModel();

                using (SqlConnection con = new SqlConnection(_Constring))
                {
                    using (SqlCommand cmd = new SqlCommand(ModuleList.USP_GETMANUFACTURER, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CurrentPage", DisplayStart);
                        cmd.Parameters.AddWithValue("@PageSize", DisplayLength);
                        cmd.Parameters.AddWithValue("@ColumnName", SortColumnName);
                        cmd.Parameters.AddWithValue("@SortOrder", SortDirection);
                        cmd.Parameters.AddWithValue("@Search", SearchText);
                        cmd.Parameters.AddWithValue("@ManufacturerName", ManufacturerName);

                        SqlParameter paramOutPut = new SqlParameter("@noOfRecords", SqlDbType.Int);
                        paramOutPut.Size = 15;
                        paramOutPut.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(paramOutPut);
                        cmd.Connection.Open();

                        using (IDataReader dataReader = cmd.ExecuteReader())
                        {
                            ManufacturerList.manufacturerModel = Util.DataReaderMapToList<ManufacturerModel>(dataReader);
                            //accessoriesList.AccessoryList.ForEach(x => x.Id = x.Id.ToString().Encrypt());
                        }
                        if (cmd.Parameters["@noOfRecords"].Value != DBNull.Value)
                            ManufacturerList.noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                        else
                            ManufacturerList.noOfRecords = 0;
                        con.Close();
                        return ManufacturerList;
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }



        }

        public Task<bool> AddManufacturer(ManufacturerModel manufacturerModel)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                AccessoriesViewModel objList = new AccessoriesViewModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGEMANUFACTURERS, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "Add");
                    cmd.Parameters.AddWithValue("@ManufacturerName", manufacturerModel.ManufacturerName);
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }

        }

        public Task<bool> UpdateManufacturer(ManufacturerModel manufacturerModel)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                ManufacturerModel objList = new ManufacturerModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGEMANUFACTURERS, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "Update");
                    cmd.Parameters.AddWithValue("@Id", manufacturerModel.Id);
                    cmd.Parameters.AddWithValue("@ManufacturerName", manufacturerModel.ManufacturerName);
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }
            throw new NotImplementedException();
        }

        public async Task<ManufacturerModel> GetManufacturerById(int manufacturerId)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                ManufacturerModel objList = new ManufacturerModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGEMANUFACTURERS, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", manufacturerId);
                    cmd.Parameters.AddWithValue("@Action", "ManufacturerById");
                    cmd.Connection.Open();
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    if (rdr.Read())
                    {
                        objList.Id = Convert.ToInt32(rdr["Id"]);
                        objList.ManufacturerName = rdr["ManufacturerName"].ToString();

                    }
                    con.Close();
                }

                return objList;
            }

            throw new NotImplementedException();
        }
        public Task<bool> RemoveManufacturer(int manufacturerId)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                ManufacturerModel objList = new ManufacturerModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGEMANUFACTURERS, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "Delete");
                    cmd.Parameters.AddWithValue("@Id", manufacturerId);
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

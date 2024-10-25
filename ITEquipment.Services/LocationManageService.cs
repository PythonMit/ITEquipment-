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

namespace ITEquipment.Services
{
    public class LocationManageService:ILocation
    {
        public string _Constring;
        private IConfiguration _configuration;

        public LocationManageService(IConfiguration configuration)
        {
            _configuration = configuration;
            _Constring = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<LocationModelListModel> GetAllLocation(int DisplayLength, int DisplayStart, string SortColumnName, string SortDirection, string SearchText, string LocationName)
        {
            try
            {
                if (SearchText == null)
                    SearchText = "";
                LocationModelListModel LocationList = new LocationModelListModel();

                using (SqlConnection con = new SqlConnection(_Constring))
                {
                    using (SqlCommand cmd = new SqlCommand(ModuleList.USP_GETLOCATION, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CurrentPage", DisplayStart);
                        cmd.Parameters.AddWithValue("@PageSize", DisplayLength);
                        cmd.Parameters.AddWithValue("@ColumnName", SortColumnName);
                        cmd.Parameters.AddWithValue("@SortOrder", SortDirection);
                        cmd.Parameters.AddWithValue("@Search", SearchText);
                        cmd.Parameters.AddWithValue("@LocationName", LocationName);

                        SqlParameter paramOutPut = new SqlParameter("@noOfRecords", SqlDbType.Int);
                        paramOutPut.Size = 15;
                        paramOutPut.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(paramOutPut);
                        cmd.Connection.Open();

                        using (IDataReader dataReader = cmd.ExecuteReader())
                        {
                            LocationList.locationModels = Util.DataReaderMapToList<LocationModel>(dataReader);
                            //accessoriesList.AccessoryList.ForEach(x => x.Id = x.Id.ToString().Encrypt());
                        }
                        if (cmd.Parameters["@noOfRecords"].Value != DBNull.Value)
                            LocationList.noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                        else
                            LocationList.noOfRecords = 0;
                        con.Close();
                        return LocationList;
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }



        }

        public Task<bool> AddLocation(LocationModel locationModel)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                LocationModel objList = new LocationModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGELOCATIONS, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "Add");
                    cmd.Parameters.AddWithValue("@LocationName", locationModel.LocationName);
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }

        }

        public Task<bool> UpdateLocation(LocationModel locationModel)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                LocationModel objList = new LocationModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGELOCATIONS, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "Update");
                    cmd.Parameters.AddWithValue("@Id", locationModel.Id);
                    cmd.Parameters.AddWithValue("@LocationName", locationModel.LocationName);
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }
            throw new NotImplementedException();
        }

        public async Task<LocationModel> GetlocationById(int locationId)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                LocationModel objList = new LocationModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGELOCATIONS, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", locationId);
                    cmd.Parameters.AddWithValue("@Action", "LocationById");
                    cmd.Connection.Open();
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    if (rdr.Read())
                    {
                        objList.Id = Convert.ToInt32(rdr["Id"]);
                        objList.LocationName = rdr["LocationName"].ToString();

                    }
                    con.Close();
                }

                return objList;
            }

            throw new NotImplementedException();
        }
        public Task<bool> Removelocation(int locationId)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                ManufacturerModel objList = new ManufacturerModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGELOCATIONS, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "Delete");
                    cmd.Parameters.AddWithValue("@Id", locationId);
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

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

namespace ITEquipment.Services
{
    public class AuthManageService : IAccount
    {
        public string _Constring;
        private IConfiguration _configuration;
        public AuthManageService(IConfiguration configuration)
        {
            _Constring = configuration.GetConnectionString("DefaultConnection");
            _configuration = configuration;
        }

        public Task<UserMasterViewModel> GetLoginUserDetail(string email, string password)
        {

            UserMasterViewModel loginViewModel = new UserMasterViewModel();
            using (SqlConnection sqlConnection = new SqlConnection(_Constring))
            {
                sqlConnection.Open();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_GLOBAL_GETUSERBYPARAMETERS, sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        loginViewModel = Util.ConvertTOEntity<UserMasterViewModel>(dataReader);
                    }
                }
                sqlConnection.Close();
            }
            return Task.FromResult(loginViewModel);
            //throw new NotImplementedException();
        }

        public Task<UserMasterViewModel> GetLoginUserDetailByEmail(string email)
        {

            UserMasterViewModel loginViewModel = new UserMasterViewModel();
            using (SqlConnection sqlConnection = new SqlConnection(_Constring))
            {
                sqlConnection.Open();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_GLOBAL_GETUSERBYEMAIL, sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        loginViewModel = Util.ConvertTOEntity<UserMasterViewModel>(dataReader);
                    }
                }
                sqlConnection.Close();
            }
            return Task.FromResult(loginViewModel);
            //throw new NotImplementedException();
        }
        public async Task<UserListModel> GetAllUserList(int DisplayLength, int DisplayStart, string SortColumnName, string SortDirection, string SearchText, string UserSearch, string email, string role)
        {
            try
            {
                if (SearchText == null)
                    SearchText = "";
                UserListModel userListModel = new UserListModel();

                using (SqlConnection con = new SqlConnection(_Constring))
                {
                    using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGEUSER, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CurrentPage", DisplayStart);
                        cmd.Parameters.AddWithValue("@PageSize", DisplayLength);
                        cmd.Parameters.AddWithValue("@ColumnName", SortColumnName);
                        cmd.Parameters.AddWithValue("@SortOrder", SortDirection);
                        cmd.Parameters.AddWithValue("@Search", SearchText);
                        cmd.Parameters.AddWithValue("@UserSearch", UserSearch);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Role", role);
                        SqlParameter paramOutPut = new SqlParameter("@noOfRecords", SqlDbType.Int);
                        paramOutPut.Size = 15;
                        paramOutPut.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(paramOutPut);
                        cmd.Connection.Open();

                        using (IDataReader dataReader = cmd.ExecuteReader())
                        {
                            userListModel.userMasters = Util.DataReaderMapToList<UserMasterDTOModel>(dataReader);
                            //accessoriesList.AccessoryList.ForEach(x => x.Id = x.Id.ToString().Encrypt());
                        }
                        if (cmd.Parameters["@noOfRecords"].Value != DBNull.Value)
                            userListModel.noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                        else
                            userListModel.noOfRecords = 0;
                        con.Close();
                        return userListModel;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }



        }


        public Task<bool> AddUser(UserMasterDTOModel userMasterDTO)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                UserMasterDTOModel objList = new UserMasterDTOModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_UPDATEUSERDETAILS, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FirstName", userMasterDTO.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", userMasterDTO.LastName);
                    cmd.Parameters.AddWithValue("@Email", userMasterDTO.Email);
                    cmd.Parameters.AddWithValue("@PhoneNo", userMasterDTO.PhoneNo);
                    cmd.Parameters.AddWithValue("@RoleId", userMasterDTO.RoleId);
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }

        }
    }
}

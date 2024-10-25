using ITEquipment.Interfaces;
using ITEquipment.Models;
using ITEquipment.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static ITEquipment.Models.Consts.EnumConst;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ITEquipment.Services
{
    public class EquipmentManageService : IEquipment
    {
        public string _Constring;
        private IConfiguration _configuration;

        public EquipmentManageService(IConfiguration configuration)
        {
            _Constring = configuration.GetConnectionString("DefaultConnection");
            _configuration = configuration;

        }

        public Task<bool> AddOrUpdateEquipment(EquipmentViewModel equipmentViewModel)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                EquipmentViewModel objList = new EquipmentViewModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_ADDORUPDATE, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = equipmentViewModel.Id > 0 ? equipmentViewModel.Id : 0;
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50)).Value = equipmentViewModel.Name;
                    cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 50)).Value = equipmentViewModel.Description;
                    cmd.Parameters.Add(new SqlParameter("@SerialNo", SqlDbType.NVarChar, 50)).Value = equipmentViewModel.SerialNo;
                    cmd.Parameters.Add(new SqlParameter("@ModelNo", SqlDbType.NVarChar, 50)).Value = equipmentViewModel.ModelNo;
                    cmd.Parameters.Add(new SqlParameter("@Image", SqlDbType.NVarChar, 4000)).Value = equipmentViewModel.Image;
                    cmd.Parameters.Add(new SqlParameter("@LocationId", SqlDbType.Int)).Value = equipmentViewModel.LocationId;
                    cmd.Parameters.Add(new SqlParameter("@VideoUrl", SqlDbType.NVarChar, 4000)).Value = equipmentViewModel.VideoUrl;
                    cmd.Parameters.Add(new SqlParameter("@StatusId", SqlDbType.Int)).Value = equipmentViewModel.StatusId;
                    cmd.Parameters.Add(new SqlParameter("@DocumentUrl", SqlDbType.NVarChar, 4000)).Value = string.IsNullOrEmpty(equipmentViewModel.DocumentUrl) ? null : equipmentViewModel.DocumentUrl;
                    cmd.Parameters.Add(new SqlParameter("@Manufacturer", SqlDbType.NVarChar, 4000)).Value = string.IsNullOrEmpty(equipmentViewModel.Manufacturer) ? null : equipmentViewModel.Manufacturer;
                    cmd.Parameters.Add(new SqlParameter("@AssetsTag", SqlDbType.NVarChar, 4000)).Value = string.IsNullOrEmpty(equipmentViewModel.AssetsTag) ? null : equipmentViewModel.AssetsTag;
                    cmd.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.Int)).Value = equipmentViewModel.CreatedBy;
                    cmd.Parameters.Add(new SqlParameter("@CreatedDateTime", SqlDbType.DateTime)).Value = equipmentViewModel.CreatedDateTime;
                    cmd.Parameters.Add(new SqlParameter("@ModifiedBy", SqlDbType.Int)).Value = equipmentViewModel.ModifiedBy;
                    cmd.Parameters.Add(new SqlParameter("@ModifiedDateTime", SqlDbType.DateTime)).Value = equipmentViewModel.ModifiedDateTime;
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }

            throw new NotImplementedException();
        }

        public Task<int> Bookingcancel(int userId, int bookingId)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {

                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_BOOKINGCANCEL, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = rescheduleViewModel.Id > 0 ? rescheduleViewModel.Id : 0;
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int)).Value = userId;
                    cmd.Parameters.Add(new SqlParameter("@BookingId", SqlDbType.Int)).Value = bookingId;
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteScalar();
                    cmd.Connection.Close();
                    return Task.FromResult(Convert.ToInt32(resultvalue));
                }

            }
            throw new NotImplementedException();
        }

        public Task<int> BookingEquipment(BookingEquipmentViewModel bookingEquipmentViewModel)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                BookingEquipmentViewModel objList = new BookingEquipmentViewModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_EQUIPMENTBOOKING, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int)).Value = bookingEquipmentViewModel.UserId > 0 ? bookingEquipmentViewModel.UserId : 0;
                    cmd.Parameters.Add(new SqlParameter("@EquipmentId", SqlDbType.Int)).Value = bookingEquipmentViewModel.EquipmentId;
                    //cmd.Parameters.Add(new SqlParameter("@AccessoriesId", SqlDbType.Int)).Value = bookingEquipmentViewModel.AccessoriesId;
                    cmd.Parameters.Add(new SqlParameter("@Startdate", SqlDbType.DateTime)).Value = bookingEquipmentViewModel.StartDate;
                    cmd.Parameters.Add(new SqlParameter("@Enddate", SqlDbType.DateTime)).Value = bookingEquipmentViewModel.EndDate;
                    cmd.Parameters.Add(new SqlParameter("@StatusId", SqlDbType.Int)).Value = StatusType.Requested;
                    cmd.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.Int)).Value = bookingEquipmentViewModel.CreatedBy;
                    //cmd.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime)).Value = bookingEquipmentViewModel.CreatedDate;
                    //cmd.Parameters.Add(new SqlParameter("@ModifyBy", SqlDbType.Int)).Value = bookingEquipmentViewModel.ModifyBy;
                    //cmd.Parameters.Add(new SqlParameter("@ModifiedDate", SqlDbType.DateTime)).Value = bookingEquipmentViewModel.ModifiedDate;
                    //  objList.bookingAccessoriesViewModels.ForEach(x => x.Id = bookingEquipmentViewModel.Id);
                    DataTable BA = Util.ToDataTable(bookingEquipmentViewModel.bookingAccessoriesViewModels);
                    cmd.Parameters.Add("@BookingAccessoriesTableType", SqlDbType.Structured).Value = BA;
                    SqlParameter paramOutPut = new SqlParameter("@BookingId", SqlDbType.Int);
                    paramOutPut.Size = 15;
                    paramOutPut.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramOutPut);
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteScalar();
                    cmd.Connection.Close();
                    return Task.FromResult(Convert.ToInt32(resultvalue));
                }
            }

            throw new NotImplementedException();
        }

        public Task<int> BookingExtend(ExtendBookingViewModel extendBookingViewModel)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                ExtendBookingViewModel objList = new ExtendBookingViewModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_BOOKINgEXTEND, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = rescheduleViewModel.Id > 0 ? rescheduleViewModel.Id : 0;
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int)).Value = extendBookingViewModel.UserId;
                    cmd.Parameters.Add(new SqlParameter("@BookingId", SqlDbType.Int)).Value = extendBookingViewModel.BookingId;
                    cmd.Parameters.Add(new SqlParameter("@Enddate", SqlDbType.DateTime)).Value = extendBookingViewModel.Enddate;
                    cmd.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.Int)).Value = extendBookingViewModel.CreatedBy;
                    cmd.Parameters.Add(new SqlParameter("@BookingType", SqlDbType.NVarChar)).Value = extendBookingViewModel.BookingType;
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteScalar();
                    cmd.Connection.Close();
                    return Task.FromResult(Convert.ToInt32(resultvalue));
                }

            }

            throw new NotImplementedException();
        }

        public Task<int> BookingReschedule(RescheduleViewModel rescheduleViewModel)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                RescheduleViewModel objList = new RescheduleViewModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_BOOKINGRESCHEDULE, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = rescheduleViewModel.Id > 0 ? rescheduleViewModel.Id : 0;
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int)).Value = rescheduleViewModel.UserId;
                    cmd.Parameters.Add(new SqlParameter("@BookingId", SqlDbType.Int)).Value = rescheduleViewModel.BookingId;
                    cmd.Parameters.Add(new SqlParameter("@startdate", SqlDbType.DateTime)).Value = rescheduleViewModel.startdate;
                    cmd.Parameters.Add(new SqlParameter("@Enddate", SqlDbType.DateTime)).Value = rescheduleViewModel.Enddate;
                    cmd.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.Int)).Value = rescheduleViewModel.CreatedBy;
                    cmd.Parameters.Add(new SqlParameter("@BookingType", SqlDbType.NVarChar)).Value = rescheduleViewModel.BookingType;
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteScalar();
                    cmd.Connection.Close();
                    return Task.FromResult(Convert.ToInt32(resultvalue));
                }

            }

            throw new NotImplementedException();
        }

        public async Task<AccessoriesList> GetAllAccessoriesByEquipmentId(int equipmentId)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                AccessoriesList objList = new AccessoriesList();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_GETALLACCESSORIESBYEQUIPMENTID, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (equipmentId > 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = equipmentId;
                        cmd.Connection.Open();
                        using (IDataReader dataReader = cmd.ExecuteReader())
                        {
                            objList.accessoriesViewModels = Util.DataReaderMapToList<AccessoriesViewModel>(dataReader);
                        }
                        cmd.Connection.Close();
                        return await Task.FromResult(objList);
                    }
                    return null;
                }
            }
            throw new NotImplementedException();
        }

        public async Task<BookingList> GetAllBookingListById(int userId)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                BookingList objList = new BookingList();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_GETALLBOOKINGBYUSERID, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = userId;
                    cmd.Connection.Open();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        objList.bookingviewmodels = Util.DataReaderMapToList<Bookingviewmodel>(dataReader);

                    }
                    cmd.Connection.Close();
                    return await Task.FromResult(objList);
                }
            }
            throw new NotImplementedException();
        }

        public async Task<EquipmentList> GetAllEquipment()
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                EquipmentList objList = new EquipmentList();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_GETALLEQUIPMENT, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        objList.equipmentViewModels = Util.DataReaderMapToList<EquipmentViewModel>(dataReader);

                    }
                    cmd.Connection.Close();
                    return await Task.FromResult(objList);
                }
            }
        }

        public async Task<UserMasterViewModel> GetUserDetailsById(int userId)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                UserMasterViewModel objList = new UserMasterViewModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_GETUSERDETAILSBYID, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", userId);
                    cmd.Connection.Open();
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    if (rdr.Read())
                    {
                        objList.Id = Convert.ToInt32(rdr["Id"]);
                        objList.FirstName = rdr["FirstName"].ToString();
                        objList.LastName = rdr["LastName"].ToString();
                        objList.AppVersion = rdr["AppVersion"].ToString();
                        objList.PhoneNo = rdr["PhoneNo"].ToString();
                        objList.FCMToken = rdr["FCMToken"].ToString();
                        objList.DeviceId = rdr["DeviceId"].ToString();
                        objList.Os = rdr["Os"].ToString();
                        objList.Email = rdr["Email"].ToString();
                        objList.RoleId = Convert.ToInt32(rdr["RoleId"]);
                    }
                    con.Close();
                }

                return objList;
            }

            throw new NotImplementedException();
        }

        public Task<bool> RemoveEquipment(int equipmentId)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                EquipmentViewModel objList = new EquipmentViewModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_DELETE, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = equipmentId;

                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }
            throw new NotImplementedException();
        }
        public Task<bool> RemoveEquipmentFile(int equipmentId, string FileType)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                EquipmentViewModel objList = new EquipmentViewModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_DELETEEQUIPMENTFILE, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = equipmentId;
                    cmd.Parameters.AddWithValue("@FileType", FileType);
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserDetails(UserMasterViewModel userMasterViewModel)
        {


            using (SqlConnection con = new SqlConnection(_Constring))
            {
                UserMasterViewModel objList = new UserMasterViewModel();

                //var result = GetUserDetailsById(userMasterViewModel.Id);
                // if (result.Id ==userMasterViewModel.Id)
                //{
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_UPDATEUSERDETAILS, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = userMasterViewModel.Id > 0 ? userMasterViewModel.Id : 0;
                    cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 50)).Value = userMasterViewModel.FirstName;
                    cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 50)).Value = userMasterViewModel.LastName;
                    cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50)).Value = userMasterViewModel.Email;
                    cmd.Parameters.Add(new SqlParameter("@PhoneNo", SqlDbType.NVarChar, 50)).Value = userMasterViewModel.PhoneNo;
                    cmd.Parameters.Add(new SqlParameter("@FCMToken", SqlDbType.NVarChar, 50)).Value = userMasterViewModel.FCMToken;
                    //cmd.Parameters.Add(new SqlParameter("@BookingId", SqlDbType.Int)).Value = userMasterViewModel.BookingId;
                    cmd.Parameters.Add(new SqlParameter("@DeviceId", SqlDbType.NVarChar, 50)).Value = userMasterViewModel.DeviceId;
                    cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 50)).Value = userMasterViewModel.Password;
                    cmd.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.Int)).Value = userMasterViewModel.CreatedBy;
                    cmd.Parameters.Add(new SqlParameter("@CreatedDateTime", SqlDbType.DateTime)).Value = userMasterViewModel.CreatedDateTime;
                    cmd.Parameters.Add(new SqlParameter("@ModifiedBy", SqlDbType.Int)).Value = userMasterViewModel.ModifiedBy;
                    cmd.Parameters.Add(new SqlParameter("@ModifiedDateTime", SqlDbType.DateTime)).Value = userMasterViewModel.ModifiedDateTime;
                    cmd.Parameters.Add(new SqlParameter("@AppVersion", SqlDbType.NVarChar, 50)).Value = userMasterViewModel.AppVersion;
                    cmd.Parameters.Add(new SqlParameter("@Os", SqlDbType.NVarChar, 50)).Value = userMasterViewModel.Os;
                    cmd.Parameters.Add(new SqlParameter("@RoleId", SqlDbType.Int)).Value = 1; //userMasterViewModel.RoleId;
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
                //}

            }


            throw new NotImplementedException();
        }
        public async Task<GetSettingViewModel> GetSetting()
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                GetSettingViewModel objList = new GetSettingViewModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_GETSETTING, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    if (rdr.Read())
                    {
                        objList.Id = Convert.ToInt32(rdr["Id"]);
                        objList.SettingName = rdr["SettingName"].ToString();
                        objList.SettingValue = rdr["SettingValue"].ToString();

                    }
                    con.Close();
                }

                return objList;
            }

            throw new NotImplementedException();
        }

        #region web project
        public async Task<EquipmentListModel> GetAllEquipmentList(int DisplayLength, int DisplayStart, string SortColumnName, string SortDirection, string SearchText, string EquipmentName, string AssetsTag, string Location, string Status)
        {
            try
            {
                if (SearchText == null)
                    SearchText = "";
                EquipmentListModel equipmentList = new EquipmentListModel();

                using (SqlConnection con = new SqlConnection(_Constring))
                {
                    using (SqlCommand cmd = new SqlCommand(ModuleList.USP_GETEQUIPMENT, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CurrentPage", DisplayStart);
                        cmd.Parameters.AddWithValue("@PageSize", DisplayLength);
                        cmd.Parameters.AddWithValue("@ColumnName", SortColumnName);
                        cmd.Parameters.AddWithValue("@SortOrder", SortDirection);
                        cmd.Parameters.AddWithValue("@Search", SearchText);
                        cmd.Parameters.AddWithValue("@EquipmentName", EquipmentName);
                        cmd.Parameters.AddWithValue("@AssetsTag", AssetsTag);
                        cmd.Parameters.AddWithValue("@Location", Location);
                        cmd.Parameters.AddWithValue("@Status", Status);
                        SqlParameter paramOutPut = new SqlParameter("@noOfRecords", SqlDbType.Int);
                        paramOutPut.Size = 15;
                        paramOutPut.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(paramOutPut);
                        cmd.Connection.Open();

                        using (IDataReader dataReader = cmd.ExecuteReader())
                        {
                            equipmentList.EquipmentList = Util.DataReaderMapToList<EquipmentModel>(dataReader);
                            //accessoriesList.AccessoryList.ForEach(x => x.Id = x.Id.ToString().Encrypt());
                        }
                        if (cmd.Parameters["@noOfRecords"].Value != DBNull.Value)
                            equipmentList.noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                        else
                            equipmentList.noOfRecords = 0;
                        con.Close();
                        return equipmentList;
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public Task<bool> AddEquipment(EquipmentDTOModel equipment, string AccessoryListJson)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                List<AccessoryListDTO> accessoryLists = new List<AccessoryListDTO>();
                if (AccessoryListJson != null)
                {

                    List<string> accessoriesNames = JsonConvert.DeserializeObject<List<string>>(AccessoryListJson);

                    foreach (var accessory in accessoriesNames)
                    {
                        accessoryLists.Add(new AccessoryListDTO { AccessoryName = accessory });
                    }
                }

                //var accessories = JsonConvert.DeserializeObject<List<string>>(AccessoryListJson);
                EquipmentModel objList = new EquipmentModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGEEQUIPMENT, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 50)).Value = "Add";
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50)).Value = equipment.Name;
                    cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 50)).Value = equipment.Description;
                    cmd.Parameters.Add(new SqlParameter("@SerialNo", SqlDbType.NVarChar, 50)).Value = equipment.SerialNo;
                    cmd.Parameters.Add(new SqlParameter("@ModelNo", SqlDbType.NVarChar, 50)).Value = equipment.ModelNo;
                    cmd.Parameters.Add(new SqlParameter("@Image", SqlDbType.NVarChar, 4000)).Value = equipment.ImagePath;
                    cmd.Parameters.Add(new SqlParameter("@LocationId", SqlDbType.Int)).Value = equipment.LocationId;
                    cmd.Parameters.Add(new SqlParameter("@VideoUrl", SqlDbType.NVarChar, 4000)).Value = equipment.VideoPath;
                    cmd.Parameters.Add(new SqlParameter("@StatusId", SqlDbType.Int)).Value = equipment.StatusId;
                    cmd.Parameters.Add(new SqlParameter("@DocumentUrl", SqlDbType.NVarChar, 4000)).Value = equipment.DocumentPath;
                    cmd.Parameters.Add(new SqlParameter("@ManufacturerId", SqlDbType.Int)).Value = equipment.ManufacturerId;
                    cmd.Parameters.Add(new SqlParameter("@AssetsTag", SqlDbType.NVarChar, 4000)).Value = equipment.AssetsTag;
                    DataTable BA = Util.ToDataTable(accessoryLists);
                    cmd.Parameters.Add("@EquipmentAccessoriesTableType", SqlDbType.Structured).Value = BA;
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }

        }
        public Task<bool> UpdateEquipment(EquipmentDTOModel equipment, string AccessoryListJson)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                List<AccessoryListDTO> accessoryLists = new List<AccessoryListDTO>();
                if (AccessoryListJson != null)
                {

                    List<string> accessoriesNames = JsonConvert.DeserializeObject<List<string>>(AccessoryListJson);

                    foreach (var accessory in accessoriesNames)
                    {
                        accessoryLists.Add(new AccessoryListDTO { AccessoryName = accessory });
                    }
                }
                EquipmentModel objList = new EquipmentModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGEEQUIPMENT, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 50)).Value = "Update";
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = equipment.Id;
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50)).Value = equipment.Name;
                    cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 50)).Value = equipment.Description;
                    cmd.Parameters.Add(new SqlParameter("@SerialNo", SqlDbType.NVarChar, 50)).Value = equipment.SerialNo;
                    cmd.Parameters.Add(new SqlParameter("@ModelNo", SqlDbType.NVarChar, 50)).Value = equipment.ModelNo;
                    cmd.Parameters.Add(new SqlParameter("@Image", SqlDbType.NVarChar, 4000)).Value = equipment.ImagePath;
                    cmd.Parameters.Add(new SqlParameter("@LocationId", SqlDbType.NVarChar, 50)).Value = equipment.LocationId;
                    cmd.Parameters.Add(new SqlParameter("@VideoUrl", SqlDbType.NVarChar, 4000)).Value = equipment.VideoPath;
                    cmd.Parameters.Add(new SqlParameter("@StatusId", SqlDbType.Int)).Value = equipment.StatusId;
                    cmd.Parameters.Add(new SqlParameter("@DocumentUrl", SqlDbType.NVarChar, 4000)).Value = equipment.DocumentPath;
                    cmd.Parameters.Add(new SqlParameter("@ManufacturerId", SqlDbType.Int)).Value = equipment.ManufacturerId;
                    cmd.Parameters.Add(new SqlParameter("@AssetsTag", SqlDbType.NVarChar, 4000)).Value = equipment.AssetsTag;
                    DataTable BA = Util.ToDataTable(accessoryLists);
                    cmd.Parameters.Add("@EquipmentAccessoriesTableType", SqlDbType.Structured).Value = BA;
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }

        }
        public async Task<EquipmentDTOModel> GetEquipmentById(int equipmentId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_Constring))
                {
                    EquipmentDTOModel objModel = new EquipmentDTOModel();
                    using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGEEQUIPMENT, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", equipmentId);
                        cmd.Parameters.AddWithValue("@Action", "EquipmentById");
                        cmd.Connection.Open();
                        SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                        if (rdr.Read())
                        {
                            objModel.Id = Convert.ToInt32(rdr["Id"]);
                            objModel.Name = rdr["Name"].ToString();
                            objModel.Description = rdr["Description"].ToString();
                            objModel.ModelNo = rdr["ModelNo"].ToString();
                            objModel.SerialNo = rdr["SerialNo"].ToString();
                            objModel.ImagePath = rdr["Image"].ToString();
                            objModel.LocationId = Convert.ToInt32(rdr["LocationId"]);
                            objModel.VideoPath = rdr["VideoUrl"].ToString();
                            objModel.StatusId = Convert.ToInt32(rdr["StatusId"]);
                            objModel.DocumentPath = rdr["DocumentUrl"].ToString();
                            objModel.StatusType = rdr["StatusType"].ToString();
                            objModel.ManufacturerId = Convert.ToInt32(rdr["ManufacturerId"]);
                            objModel.AssetsTag = rdr["AssetsTag"].ToString();
                            objModel.LocationName = rdr["LocationName"].ToString();
                            objModel.ManufacturerName = rdr["ManufacturerName"].ToString();
                        }
                        rdr.NextResult();
                        objModel.AssetsList = Util.DataReaderMapToList<AccessoryListViewModel>(rdr);

                        con.Close();
                    }

                    return objModel;
                }
            }
            catch (Exception ex)
            {

                throw;
            }



        }
        public async Task<DashboardCountList> GetDashboardcountList()
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                DashboardCountList objList = new DashboardCountList();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_COUNTLIST, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    if (rdr.Read())
                    {
                        objList.EquipmentsList = Convert.ToInt32(rdr["EquipmentsList"]); //BigInteger.Parse(rdr["EquipmentsList"].ToString()); 
                        objList.AsseccoryList = Convert.ToInt32(rdr["AsseccoryList"]);
                        objList.UsersList = Convert.ToInt32(rdr["UsersList"]);
                        objList.BookingList = Convert.ToInt32(rdr["BookingList"]);
                    }
                    con.Close();
                }

                return objList;
            }


        }
        public async Task<BookingListDTOModel> GetAllBookingList(int DisplayLength, int DisplayStart, string SortColumnName, string SortDirection, string SearchText, string UserSearch, string EquipmentSearch, string StatusSearch, string StartDateSearch, string EndDateSearch)
        {
            try
            {
                if (SearchText == null)
                    SearchText = "";
                BookingListDTOModel BookingList = new BookingListDTOModel();

                using (SqlConnection con = new SqlConnection(_Constring))
                {
                    using (SqlCommand cmd = new SqlCommand(ModuleList.USP_MANAGEBOOKING, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CurrentPage", DisplayStart);
                        cmd.Parameters.AddWithValue("@PageSize", DisplayLength);
                        cmd.Parameters.AddWithValue("@ColumnName", SortColumnName);
                        cmd.Parameters.AddWithValue("@SortOrder", SortDirection);
                        cmd.Parameters.AddWithValue("@Search", SearchText);
                        cmd.Parameters.AddWithValue("@UserSearch", UserSearch);
                        cmd.Parameters.AddWithValue("@EquipmentSearch", EquipmentSearch);
                        cmd.Parameters.AddWithValue("@StatusSearch", StatusSearch);
                        cmd.Parameters.AddWithValue("@StartDateSearch", StartDateSearch);
                        cmd.Parameters.AddWithValue("@EndDateSearch", EndDateSearch);
                        SqlParameter paramOutPut = new SqlParameter("@noOfRecords", SqlDbType.Int);
                        paramOutPut.Size = 15;
                        paramOutPut.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(paramOutPut);
                        cmd.Connection.Open();

                        using (IDataReader dataReader = cmd.ExecuteReader())
                        {
                            BookingList.BookingList = Util.DataReaderMapToList<BookingDTOViewModel>(dataReader);
                            //accessoriesList.AccessoryList.ForEach(x => x.Id = x.Id.ToString().Encrypt());
                        }
                        if (cmd.Parameters["@noOfRecords"].Value != DBNull.Value)
                            BookingList.noOfRecords = Convert.ToInt32(cmd.Parameters["@noOfRecords"].Value);
                        else
                            BookingList.noOfRecords = 0;
                        con.Close();
                        return BookingList;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }



        }

        public async Task<UserMasterDTOModel> ViewUserDetailsById(int userId)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                UserMasterDTOModel objList = new UserMasterDTOModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_GETUSERDETAILSBYID, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", userId);
                    cmd.Connection.Open();
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    if (rdr.Read())
                    {
                        objList.Id = Convert.ToInt32(rdr["Id"]);
                        objList.FirstName = rdr["FirstName"].ToString();
                        objList.LastName = rdr["LastName"].ToString();
                        objList.AppVersion = rdr["AppVersion"].ToString();
                        objList.PhoneNo = rdr["PhoneNo"].ToString();
                        objList.FCMToken = rdr["FCMToken"].ToString();
                        objList.DeviceId = rdr["DeviceId"].ToString();
                        objList.Os = rdr["Os"].ToString();
                        objList.Email = rdr["Email"].ToString();
                        objList.RoleId = Convert.ToInt32(rdr["RoleId"]);
                        objList.Name = rdr["Name"].ToString();
                    }
                    con.Close();
                }

                return objList;
            }

            throw new NotImplementedException();
        }
        public Task<bool> UpdateBookingStatus(int bookingId, int statusId, string Status = "", string Comment = "")
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {

                //using (SqlCommand cmd = new SqlCommand(ModuleList.USP_UPDATEBOOKINGSTATUS, con))
                using (SqlCommand cmd = new SqlCommand(ModuleList.usp_UpdateBookingStatus, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookingId", bookingId);
                    cmd.Parameters.AddWithValue("@StatusId", statusId);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    cmd.Parameters.AddWithValue("@Comment", Comment);
                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }

        }

        public Task<bool> UpdateEquipmentStatus(int equipmentId, string StatusType)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {

                //using (SqlCommand cmd = new SqlCommand(ModuleList.USP_UPDATEBOOKINGSTATUS, con))
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_UPDATEEQUIPMENTSTATUS, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EquipmentId", equipmentId);
                    cmd.Parameters.AddWithValue("@StatusType", StatusType);

                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }

        }

        public async Task<List<EquipmentStatuswiseCountList>> GetEquipmentStatuswiseCountList()
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                List<EquipmentStatuswiseCountList> objList = new List<EquipmentStatuswiseCountList>();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_GETEQUIPMENTSTATUSWISECOUNTLIST, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = equipmentId;
                    cmd.Connection.Open();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        objList = Util.DataReaderMapToList<EquipmentStatuswiseCountList>(dataReader);
                    }
                    cmd.Connection.Close();
                    return await Task.FromResult(objList);

                }
            }
            throw new NotImplementedException();
        }

        public async Task<List<TopUsedEquipmentCountModal>> GetTopUsedEquipmentCountList()
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                List<TopUsedEquipmentCountModal> objList = new List<TopUsedEquipmentCountModal>();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_EQUIPMENT_GETTOPUSEDEQUIPMENTCOUNTLIST, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = equipmentId;
                    cmd.Connection.Open();
                    using (IDataReader dataReader = cmd.ExecuteReader())
                    {
                        objList = Util.DataReaderMapToList<TopUsedEquipmentCountModal>(dataReader);
                    }
                    cmd.Connection.Close();
                    return await Task.FromResult(objList);

                }
            }
            throw new NotImplementedException();
        }
        public Task<bool> RemoveAccessoryMapping(int Id)
        {
            using (SqlConnection con = new SqlConnection(_Constring))
            {
                EquipmentViewModel objList = new EquipmentViewModel();
                using (SqlCommand cmd = new SqlCommand(ModuleList.USP_ACCESSORYMAPPING_DELETE, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = Id;

                    cmd.Connection.Open();
                    var resultvalue = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return Task.FromResult(true);
                }
            }
            throw new NotImplementedException();
        }


        public async Task<BookingHistoryList> GetBookingHistory(int bookingId,int equipmentId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_Constring))
                {
                    BookingHistoryList objModel = new BookingHistoryList();
                    using (SqlCommand cmd = new SqlCommand(ModuleList.USP_BOOKINGHISTORY, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BookingId", bookingId);
                        cmd.Parameters.AddWithValue("@EquipmentID", equipmentId);
                        cmd.Connection.Open();
                        List<BookingHistoryModel> bookingHistoryModels = new List<BookingHistoryModel>();
                        SqlDataReader rdr = await cmd.ExecuteReaderAsync();

                        //while (rdr.Read())
                        //{
                            objModel.bookingHistoryModels = Util.DataReaderMapToList<BookingHistoryModel>(rdr);

                        //}
                        con.Close();
                    }

                    return objModel;
                }
            }
            catch (Exception ex)
            {

                throw;
            }



        }
        #endregion



    }

}

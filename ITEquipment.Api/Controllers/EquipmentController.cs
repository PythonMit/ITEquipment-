using ITEquipment.Interfaces;
using ITEquipment.Models;
using ITEquipment.Services;
using ITEquipment.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;

namespace ITEquipment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IStatus _status;
        private readonly IEquipment _equipment;
        private readonly ICommonMethods _commonMethods;

        public EquipmentController(IStatus status, IEquipment equipment,ICommonMethods commonMethods)
        {
            _status = status;
            _equipment = equipment;
            _commonMethods = commonMethods;
        }
        //[Authorize]
        [HttpGet("EquipmentStausTypes")]
        public async Task<IActionResult> GetEquipmentStatus()
        {

            try
            {
                StatusViewModel objList = await _status.GetStatus();
                return Ok(objList.statusLists);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost("AddOrUpdate_Equipment")]
        public async Task<IActionResult> AddOrUpdateEquipment([FromBody] EquipmentViewModel equipmentViewModel)
        {
            try
            {

                var result = await _equipment.AddOrUpdateEquipment(equipmentViewModel);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> DeleteEquipment(int Id)
        {
            try
            {
                var result = await _equipment.RemoveEquipment(Id);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet("GetAllEquipment")]
        public async Task<IActionResult> GetAllEquipment()
        {
            try
            {
                EquipmentList objList = await _equipment.GetAllEquipment();
                return Ok(objList.equipmentViewModels);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet("GetAllAccessories/{equipmentId}")]
        public async Task<IActionResult> GetAllAccessoriesByEquipmentId(int equipmentId)
        {
            try
            {
                AccessoriesList objList = await _equipment.GetAllAccessoriesByEquipmentId(equipmentId);
                return Ok(objList.accessoriesViewModels);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPut("UpdateUserDetails")]
        public async Task<IActionResult> UpdateUserDetails([FromBody] UserMasterViewModel userMasterViewModel)
        {
            var resultModel = new ResponseResult<bool>();
            try
            {

                //if (userMasterViewModel.Id > 0)
                //{
                    var result = await _equipment.UpdateUserDetails(userMasterViewModel);
                    if (result)
                    {
                        resultModel.Success = true;
                        resultModel.Value = true;
                        resultModel.ResultMessage = "FCM token uploaded successfully!";
                    }
                    else
                    {
                        resultModel.Success = false;
                        resultModel.Value = false;
                        resultModel.ResultMessage = "Something went wrong.";
                    }

                //}
                //else
                //{
                //    resultModel.Success = false;
                //    resultModel.Value = false;
                //    resultModel.ResultMessage = "User not found.";
                //}
            }
            catch (Exception ex)
            {
                resultModel.Success = false;
                resultModel.ResultMessage = ex.Message;
            }
            return Ok(resultModel);
        }
        [HttpPost("BookingEquipment")]
        public async Task<IActionResult> BookingEquipment([FromBody] BookingEquipmentViewModel bookingEquipmentViewModel)
        {
            var resultModel = new ResponseResult<int>();
            try
            {

                var result = await _equipment.BookingEquipment(bookingEquipmentViewModel);//1;
                    if (result>0)
                {
                     await _equipment.UpdateBookingStatus(result, 0, "Requested", "Equipment Booking");
                    var equipmentData = await _equipment.GetEquipmentById(bookingEquipmentViewModel.EquipmentId);
                    var userData= await _equipment.ViewUserDetailsById(bookingEquipmentViewModel.UserId);
                    UserMasterViewModel objList = await _equipment.GetUserDetailsById(bookingEquipmentViewModel.UserId);
                        if (!string.IsNullOrEmpty(objList.Email))
                        {
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplate", "BookingConfirmation.html");
                            string emailContent = CommonMethodsService.ReadFileAsString(filePath);
                            EmailFormModel objEmail = new EmailFormModel();
                            objEmail.Subject = EmailSubjectConstants.USER_EQUIPMENTBOOKING;
                            objEmail.Message = emailContent;
                        objEmail.ToAddress = objList.Email; //"mitesh.n.gandhi@tretainfotech.com"; 
                        objEmail.StartDate = bookingEquipmentViewModel.StartDate.ToString("d");
                        objEmail.EndDate = bookingEquipmentViewModel.EndDate.ToString("d");
                        objEmail.EquName = equipmentData.Name;
                        objEmail.UserName = userData.FirstName + ' ' + userData.LastName;
                        objEmail.CcAddress = "mitesh.n.gandhi@tretainfotech.com";
                        int mailSent = _commonMethods.SendEmailInThread(objEmail);
                            //int mailSent = _commonMethods.EmailSend(objEmail);

                            bool IsMailSent = (mailSent > 0) ? true : false;
                            if (IsMailSent)
                            {
                                resultModel.Success = true;
                                resultModel.Value = result;
                                resultModel.ResultMessage = "Booked equipment successfully!";
                            }
                            

                        }
                        
                    }
                    else
                    {
                        resultModel.Success = false;
                        resultModel.Value = 0;
                        resultModel.ResultMessage = "Something went wrong.";
                    }
                return Ok(resultModel);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("GetAllBookingById/{userId}")]
        public async Task<IActionResult> GetAllBookingListById(int userId)
        {
            try
            {
                BookingList objList = await _equipment.GetAllBookingListById(userId);
                return Ok(objList.bookingviewmodels);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost("BookingReschedule")]
        public async Task<IActionResult> BookingReschedule([FromBody] RescheduleViewModel rescheduleViewModel)
        {
            var resultModel = new ResponseResult<int>();
            try
            {

                var result = await _equipment.BookingReschedule(rescheduleViewModel);
                if (result>0)
                {
                    resultModel.Success = true;
                    resultModel.Value = result;
                    resultModel.ResultMessage = "Reschedule equipment successfully!";
                }
                else
                {
                    resultModel.Success = false;
                    resultModel.Value = 0;
                    resultModel.ResultMessage = "Something went wrong.";
                }
                return Ok(resultModel);
               
               
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost("BookingExtend")]
        public async Task<IActionResult> BookingExtend( [FromBody] ExtendBookingViewModel extendBookingViewModel)
        {
            var resultModel = new ResponseResult<int>();
            try
            {

                var result = await _equipment.BookingExtend(extendBookingViewModel);
                if (result>0)
                {
                    resultModel.Success = true;
                    resultModel.Value =result;
                    resultModel.ResultMessage = "Extend equipment successfully!";
                }
                else
                {
                    resultModel.Success = false;
                    resultModel.Value = 0;
                    resultModel.ResultMessage = "Something went wrong.";
                }
                return Ok(resultModel);


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost("BookingCancel/{userId}/{bookingId}")]
        public async Task<IActionResult> Bookingcancel(int userId,int bookingId)
        {
            var resultModel = new ResponseResult<int>();
            try
            {

                var result = await _equipment.Bookingcancel(userId, bookingId);
                if (result > 0)
                {
                    resultModel.Success = true;
                    resultModel.Value = result;
                    resultModel.ResultMessage = "Cancelled equipment successfully!";
                }
                else
                {
                    resultModel.Success = false;
                    resultModel.Value = 0;
                    resultModel.ResultMessage = "Something went wrong.";
                }
                return Ok(resultModel);


            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet("GetUserDetailsById/{userId}")]
        public async Task<IActionResult> GetUserDetailsById(int userId)
        {
           // var resultModel = new ResponseResult<int>();
            try
            {
                if (userId > 0)
                {
                    UserMasterViewModel objList = await _equipment.GetUserDetailsById(userId);
                  if(objList.Id>0)
                    {
                        return Ok(objList);
                    }
                }
                return Ok("user not found");
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        [HttpGet("Agreement")]
        public async Task<IActionResult> AcceptAgreement( )
        {
            var resultModel = new ResponseResult<string>();
            try
            {
                var result = await _equipment.GetSetting();
                resultModel.Success = true;
               resultModel.Value = result.SettingValue ;
                resultModel.ResultMessage = "Agreement accept successfully!";

                return Ok(resultModel);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
      
    }
}

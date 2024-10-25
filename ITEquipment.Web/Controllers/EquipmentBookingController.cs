using ITEquipment.Interfaces;
using ITEquipment.Models;
using ITEquipment.Services;
using ITEquipment.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace ITEquipment.Web.Controllers
{
    [Authorize]
    public class EquipmentBookingController : Controller
    {


        private readonly IEquipment _equipment;
        private readonly IStatus _status;
        private readonly ICommonMethods _commonMethods;
        public EquipmentBookingController(IEquipment equipment, IStatus status, ICommonMethods commonMethods)
        {
            _equipment = equipment;
            _status = status;
            _commonMethods = commonMethods;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {

                    foreach (var claim in User.Claims)
                    {
                        if (claim.Type == "name")
                        {
                            ViewData["UserName"] = claim.Value;
                        }
                    }
                    var email = User.Identity.Name;
                    ViewData["UserEmail"] = email;
                    ViewBag.URole = HttpContext.Session.GetString("UserRoles");
                }
                ViewBag.Status = await _status.GetEquipmentStatusType("0");
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        public async Task<IActionResult> GetEquipmentBookingList(string sEcho = "", int iDisplayStart = 0, int iDisplayLength = 0, int iSortCol_0 = 0, string sSortDir_0 = "", string sColumns = "", string searchdata = "", string UserSearch = "", string EquipmentSearch = "", string StatusSearch = "", string StartDateSearch = "", string EndDateSearch = "")
        {
            JQueryDataTableParamModel parm = new JQueryDataTableParamModel();
            //DateOnly EndDateSearch = default(DateOnly)
            parm.sEcho = sEcho;
            parm.iDisplayStart = iDisplayStart;
            parm.iDisplayLength = iDisplayLength;
            parm.sSortDir_0 = sSortDir_0;
            parm.searchdata = searchdata;
            parm.UserSearch = UserSearch;
            parm.EquipmentSearch = EquipmentSearch;
            parm.StatusSearch = StatusSearch;
            parm.StartDateSearch = StartDateSearch;
            parm.EndDateSearch = EndDateSearch;
            parm.sColumns = sColumns;
            var sColumnsArray = sColumns.Split(',');
            string sortColumn = "";
            if (sColumnsArray.Length > 0 && sColumnsArray.Length >= iSortCol_0)
                sortColumn = Convert.ToString(sColumnsArray[iSortCol_0]);
            try
            {
                if (!string.IsNullOrEmpty(parm.searchdata) || !string.IsNullOrEmpty(parm.UserSearch) || !string.IsNullOrEmpty(parm.EquipmentSearch) || !string.IsNullOrEmpty(parm.StatusSearch) || !string.IsNullOrEmpty(parm.StartDateSearch) || !string.IsNullOrEmpty(parm.EndDateSearch))
                {
                    parm.searchdata = parm.searchdata.Replace("'", "''");
                    parm.UserSearch = parm.searchdata.Replace("'", "''");
                    parm.EquipmentSearch = parm.searchdata.Replace("'", "''");
                    parm.StatusSearch = parm.searchdata.Replace("'", "''");
                    parm.StartDateSearch = parm.searchdata.Replace("'", "''");
                    parm.EndDateSearch = parm.searchdata.Replace("'", "''");
                    //parm.iDisplayLength = 10;
                    //parm.sSortDir_0 = "desc";
                    //sortColumn = "Id";


                }
                BookingListDTOModel objList = await _equipment.GetAllBookingList(parm.iDisplayLength, parm.iDisplayStart, sortColumn, parm.sSortDir_0, searchdata, UserSearch, EquipmentSearch, StatusSearch, StartDateSearch, EndDateSearch);
                return Ok(new
                {
                    sEcho = parm.sEcho,
                    iTotalRecords = objList.BookingList.Count,
                    iTotalDisplayRecords = objList.noOfRecords,
                    aaData = objList.BookingList
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> ViewUserById(int Id)
        {
            try
            {
                var result = await _equipment.ViewUserDetailsById(Id);
                ViewBag.FirstName = result.FirstName;
                return PartialView("_UserListView", result);
            }
            catch (Exception)
            {

                throw;
            }

           
        }

        public async Task<IActionResult> ViewEquipmentById(int Id)
        {
            try
            {
                var result = await _equipment.GetEquipmentById(Id);
                ViewBag.ImageFilePath = result.ImagePath;
                ViewBag.VideoPath = result.VideoPath;
                ViewBag.DocumentPath = result.DocumentPath;
                return PartialView("_EquipmentView", result);
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public async Task<IActionResult> UpdateEquipmentStatus(int BookingId, int StatusId)
        {

            MessageModel objModel = new MessageModel();
            try
            {
                var result = await _equipment.UpdateBookingStatus(BookingId, StatusId);
                if (result)
                {

                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "Booking Status update successfully!";
                }
                else
                {
                    objModel.Success = true;
                    objModel.MessageType = "failed";
                    objModel.ResultMessage = "Something went wrong! Booking Status not update!!";
                }
            }
            catch (Exception)
            {
                objModel.Success = false;
                objModel.MessageType = "Error";
                objModel.ResultMessage = "Something went wrong!";
                throw;
            }
            return Json(objModel);

        }
        public async Task<IActionResult> ActionEquipmentRequest(int BookingId, int StatusId, string ReqType, string UserEmail, string Comment, string EquName, string StartDate, string EndDate)
        {

            MessageModel objModel = new MessageModel();
            EmailFormModel objEmail = new EmailFormModel();
            try
            {
                var result = await _equipment.UpdateBookingStatus(BookingId, StatusId, ReqType, Comment);

                var filePath = string.Empty;
                if (!string.IsNullOrEmpty(UserEmail))
                {
                    if (ReqType == "Approved")
                    {
                        filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplate", "Approval_Notification.html");
                        objEmail.Subject = EmailSubjectConstants.USER_EQUIPMENTBOOKINGAPPROVED;
                    }
                    else
                    {
                        filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplate", "Rejected_Notification.html");
                        objEmail.Subject = EmailSubjectConstants.USER_EQUIPMENTBOOKINGREJECTED;
                    }
                    string emailContent = CommonMethodsService.ReadFileAsString(filePath);
                    objEmail.Message = emailContent;
                    objEmail.Comment = Comment;
                    objEmail.EquName = EquName;
                    objEmail.StartDate = StartDate;
                    objEmail.EndDate = EndDate;
                    objEmail.ToAddress = UserEmail; //"mitesh.n.gandhi@tretainfotech.com"; 
                    objEmail.ReqType = ReqType;
                    //int mailSent = _commonMethods.SendEmailInThread(objEmail);
                    int mailSent = _commonMethods.EmailSend(objEmail);

                    bool IsMailSent = (mailSent > 0) ? true : false;
                    if (IsMailSent)
                    {
                        objModel.Success = true;
                        objModel.MessageType = "Success";
                        objModel.ResultMessage = "Booking Status update successfully!";

                    }
                    else
                    {
                        objModel.Success = false;
                        objModel.MessageType = "failed";
                        objModel.ResultMessage = "Something went wrong! Booking Status not update!!";
                    }
                }
            }
            catch (Exception)
            {
                objModel.Success = false;
                objModel.MessageType = "Error";
                objModel.ResultMessage = "Something went wrong!";
                throw;
            }
            return Json(objModel);

        }
        public async Task<IActionResult> SendActionEmail(EmailFormModel objEmail)
        {
            try
            {
                var resultModel = new ResponseResult<bool>();
                int mailSent = _commonMethods.SendEmailInThread(objEmail);
                bool IsMailSent = (mailSent > 0) ? true : false;
                //ViewBag.Status = await _status.GetEquipmentStatusType("0");
                //return View(nameof(Index));

                resultModel.Success = true;
                resultModel.Value = true;
                resultModel.ResultMessage = "Booked equipment successfully!";
                return Json(resultModel);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        [HttpGet]
        public IActionResult GetOverDueEmailTemplate()
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EmailTemplate/Overdue_Alert.html");

                if (System.IO.File.Exists(filePath))
                {
                    // Return the file as a string result
                    var fileContent = System.IO.File.ReadAllText(filePath);
                    return Content(fileContent, "text/html");
                }

                // If file not found, return 404
                return NotFound("File not found");
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<IActionResult> GetEquipmentById(int Id)
        {
            try
            {
                var result = await _equipment.GetEquipmentById(Id);

                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<IActionResult> BookingHistoryById(int bookingId,int equipmentId)
        {
            try
            {
                var result = await _equipment.GetBookingHistory(bookingId, equipmentId);
               
                return PartialView("_BookingHistory", result);
            }
            catch (Exception ex)
            {

                throw;
            }


        }
    }
}

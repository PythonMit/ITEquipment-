using ITEquipment.Interfaces;
using ITEquipment.Models;
using ITEquipment.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using static ITEquipment.Models.Consts.EnumConst;
using ITEquipment.Utilities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;


namespace ITEquipment.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccount _account;
        private readonly IEquipment _equipment;
        private readonly IStatus _status;
        public HomeController(ILogger<HomeController> logger, IAccount account, IEquipment equipment, IStatus status)
        {
            _logger = logger;
            _account = account;
            _equipment = equipment;
            _status = status;
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
                    // Accessing Claims
                    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var name = User.Identity.Name;
                    var type = User.Identity.AuthenticationType;
                    ViewData["UserEmail"] = name;
                    UserMasterViewModel loginViewModel = new UserMasterViewModel();
                    loginViewModel = await _account.GetLoginUserDetailByEmail(name);
                    DashboardCountList dashboardCountList = new DashboardCountList();
                    dashboardCountList = await _equipment.GetDashboardcountList();
                    ViewBag.EquipmentsList = dashboardCountList.EquipmentsList;
                    ViewBag.AsseccoriesList = dashboardCountList.AsseccoryList;
                    ViewBag.UserList = dashboardCountList.UsersList;
                    ViewBag.BookingList = dashboardCountList.BookingList;
                    HttpContext.Session.SetString("UserEmail", User.Identity.Name);
                    //List<EquipmentStatuswiseCountList> equipmentCountList = await _equipment.GetEquipmentStatuswiseCountList();
                    if (loginViewModel != null)
                    {
                        Role role = new Role();
                        role.roleId = (RoleType)loginViewModel.RoleId;
                        HttpContext.Session.SetString("UserRoles", role.roleId.ToString());
                        ViewBag.URole = HttpContext.Session.GetString("UserRoles");
                        if (role.roleId == RoleType.Admin || role.roleId == RoleType.Readonly)
                        {
                            return View();
                        }

                    }
                }
                return RedirectToAction(nameof(Unauthorized));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Unauthorized()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetUserList(string sEcho = "", int iDisplayStart = 0, int iDisplayLength = 0, int iSortCol_0 = 0, string sSortDir_0 = "", string sColumns = "", string searchdata = "", string UserSearch = "", string email = "", string role = "")
        {
            JQueryDataTableParamModel parm = new JQueryDataTableParamModel();

            parm.sEcho = sEcho;
            parm.iDisplayStart = iDisplayStart;
            parm.iDisplayLength = iDisplayLength;
            parm.sSortDir_0 = sSortDir_0;
            parm.searchdata = searchdata;
            parm.UserSearch = UserSearch;
            parm.Email = email;
            parm.Role = role;
            var sColumnsArray = sColumns.Split(',');
            string sortColumn = "";
            if (sColumnsArray.Length > 0 && sColumnsArray.Length >= iSortCol_0)
                sortColumn = Convert.ToString(sColumnsArray[iSortCol_0]);
            try
            {
                if (!string.IsNullOrEmpty(parm.searchdata) || !string.IsNullOrEmpty(parm.UserSearch) || !string.IsNullOrEmpty(parm.Email) || !string.IsNullOrEmpty(parm.Role))
                {
                    parm.searchdata = parm.searchdata.Replace("'", "''");
                    parm.UserSearch = parm.searchdata.Replace("'", "''");
                    parm.Email = parm.searchdata.Replace("'", "''");
                    parm.Role = parm.searchdata.Replace("'", "''");
                }
                UserListModel objList = await _account.GetAllUserList(parm.iDisplayLength, parm.iDisplayStart, sortColumn, parm.sSortDir_0, searchdata, UserSearch, email, role);
                return Ok(new
                {
                    sEcho = parm.sEcho,
                    iTotalRecords = objList.userMasters.Count,
                    iTotalDisplayRecords = objList.noOfRecords,
                    aaData = objList.userMasters
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> UserList()
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
                    ViewBag.Roles = await _status.GetRoles("0");
                    ViewBag.URole = HttpContext.Session.GetString("UserRoles");
                    ViewBag.email = HttpContext.Session.GetString("UserEmail");
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }
          
        }
        public async Task<IActionResult> UpdateUserRole(int UserId, int RoleId)
        {

            MessageModel objModel = new MessageModel();
            try
            {
                var result = await _status.UpdateUserRole(UserId, RoleId);
                if (result)
                {

                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "User Role update successfully!";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(UserMasterDTOModel UserMasterDTOModel)
        {

            MessageModel objModel = new MessageModel();
            try
            {

                UserMasterViewModel loginViewModel = new UserMasterViewModel();
                loginViewModel = await _account.GetLoginUserDetailByEmail(UserMasterDTOModel.Email);
                if (loginViewModel == null)
                {
                    var result = await _account.AddUser(UserMasterDTOModel);
                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "User added successfully!";
                }
                else
                {
                    objModel.Success = false;
                    objModel.MessageType = "Error";
                    objModel.ResultMessage = "This email is already Exist!";
                }
            }
            catch (Exception)
            {
                objModel.Success = false;
                objModel.MessageType = "Error";
                objModel.ResultMessage = "Something went wrong!";
                throw;
            }

            // return Ok(objModel);

            return Json(objModel);

            //return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetStatusCounts()
        {
            try
            {
                List<EquipmentStatuswiseCountList> equipmentCountList = await _equipment.GetEquipmentStatuswiseCountList();
                return Ok(equipmentCountList);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<IActionResult> GetTopUsedEquipmentCountList()
        {
            try
            {
                List<TopUsedEquipmentCountModal> countList = await _equipment.GetTopUsedEquipmentCountList();
                return Ok(countList);
            }
            catch (Exception)
            {

                throw;
            }
         
        }
    }
}

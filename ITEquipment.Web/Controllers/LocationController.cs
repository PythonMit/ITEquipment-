using ITEquipment.Interfaces;
using ITEquipment.Models;
using ITEquipment.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITEquipment.Web.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        private readonly IAccount _account;
        private readonly ILocation _location;
        public LocationController(IAccount account, ILocation location)
        {
            _account = account;
            _location = location;
        }
        public IActionResult Index()
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
                return View();
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public async Task<IActionResult> GetLocationList(string sEcho = "", int iDisplayStart = 0, int iDisplayLength = 0, int iSortCol_0 = 0, string sSortDir_0 = "", string sColumns = "", string searchdata = "", string LocationName = "")
        {
            JQueryDataTableParamModel parm = new JQueryDataTableParamModel();

            parm.sEcho = sEcho;
            parm.iDisplayStart = iDisplayStart;
            parm.iDisplayLength = iDisplayLength;
            parm.sSortDir_0 = sSortDir_0;
            parm.searchdata = searchdata;
            parm.LocationName = LocationName;

            var sColumnsArray = sColumns.Split(',');
            string sortColumn = "";
            if (sColumnsArray.Length > 0 && sColumnsArray.Length >= iSortCol_0)
                sortColumn = Convert.ToString(sColumnsArray[iSortCol_0]);
            try
            {
                if (!string.IsNullOrEmpty(parm.searchdata) || !string.IsNullOrEmpty(parm.LocationName))
                {
                    parm.searchdata = parm.searchdata.Replace("'", "''");
                    parm.LocationName = parm.searchdata.Replace("'", "''");

                }
                LocationModelListModel objList = await _location.GetAllLocation(parm.iDisplayLength, parm.iDisplayStart, sortColumn, parm.sSortDir_0, searchdata, LocationName);
                return Ok(new
                {
                    sEcho = parm.sEcho,
                    iTotalRecords = objList.locationModels.Count,
                    iTotalDisplayRecords = objList.noOfRecords,
                    aaData = objList.locationModels
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditLocation(LocationModel locationModel)
        {

            MessageModel objModel = new MessageModel();
            try
            {
                // if (ModelState.IsValid)
                //{

                if (locationModel.Id > 0)
                {
                    var result = await _location.UpdateLocation(locationModel);
                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "Location updated successfully!";
                }
                else
                {
                    var result = await _location.AddLocation(locationModel);
                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "Location added successfully!";
                }
                //}
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
        public IActionResult AddLocation()
        {
            return View("_AddEditManufacturer", new ManufacturerModel());
        }
        public async Task<IActionResult> EditManufacturer(int Id)
        {
            var result = await _location.GetlocationById(Id);

            return PartialView("_AddEditLocation", result);
        }
        public async Task<IActionResult> DeleteLocation(int Id)
        {
            MessageModel objModel = new MessageModel();
            try
            {
                var result = await _location.GetlocationById(Id);
                if (result != null && result.Id == Id)
                {
                    await _location.Removelocation(Id);
                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "Location deleted successfully!";
                }
                else
                {
                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "Something went wrong!";
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
    }
}

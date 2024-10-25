using ITEquipment.Interfaces;
using ITEquipment.Models;
using ITEquipment.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITEquipment.Web.Controllers
{
    [Authorize]
    public class ManufacturerController : Controller
    {
        private readonly IAccount _account;
        private readonly IManufacturer _manufacturer;
        public ManufacturerController(IAccount account, IManufacturer manufacturer)
        {
            _account = account;
            _manufacturer = manufacturer;
        }

        public IActionResult Index()
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
        public async Task<IActionResult> GetManufacturerList(string sEcho = "", int iDisplayStart = 0, int iDisplayLength = 0, int iSortCol_0 = 0, string sSortDir_0 = "", string sColumns = "", string searchdata = "", string ManufacturerName = "")
        {
            JQueryDataTableParamModel parm = new JQueryDataTableParamModel();

            parm.sEcho = sEcho;
            parm.iDisplayStart = iDisplayStart;
            parm.iDisplayLength = iDisplayLength;
            parm.sSortDir_0 = sSortDir_0;
            parm.searchdata = searchdata;
            parm.ManufacturerName = ManufacturerName;

            var sColumnsArray = sColumns.Split(',');
            string sortColumn = "";
            if (sColumnsArray.Length > 0 && sColumnsArray.Length >= iSortCol_0)
                sortColumn = Convert.ToString(sColumnsArray[iSortCol_0]);
            try
            {
                if (!string.IsNullOrEmpty(parm.searchdata) || !string.IsNullOrEmpty(parm.ManufacturerName))
                {
                    parm.searchdata = parm.searchdata.Replace("'", "''");
                    parm.ManufacturerName = parm.searchdata.Replace("'", "''");

                }
                ManufacturerNameListModel objList = await _manufacturer.GetAllManufacturer(parm.iDisplayLength, parm.iDisplayStart, sortColumn, parm.sSortDir_0, searchdata, ManufacturerName);
                return Ok(new
                {
                    sEcho = parm.sEcho,
                    iTotalRecords = objList.manufacturerModel.Count,
                    iTotalDisplayRecords = objList.noOfRecords,
                    aaData = objList.manufacturerModel
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditManufacturer(ManufacturerModel manufacturerModel)
        {

            MessageModel objModel = new MessageModel();
            try
            {
               
                if (manufacturerModel.Id > 0)
                {
                    var result = await _manufacturer.UpdateManufacturer(manufacturerModel);
                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "Manufacturer updated successfully!";
                }
                else
                {
                    var result = await _manufacturer.AddManufacturer(manufacturerModel);
                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "Manufacturer added successfully!";
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
        public IActionResult AddManufacturer()
        {
            return View("_AddEditManufacturer", new ManufacturerModel());
        }
        public async Task<IActionResult> EditManufacturer(int Id)
        {
            var result = await _manufacturer.GetManufacturerById(Id);

            return PartialView("_AddEditManufacturer", result);
        }
        public async Task<IActionResult> DeleteManufacturer(int Id)
        {
            MessageModel objModel = new MessageModel();
            try
            {
                var result = await _manufacturer.GetManufacturerById(Id);
                if (result != null && result.Id == Id)
                {
                    await _manufacturer.RemoveManufacturer(Id);
                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "Manufacturer deleted successfully!";
                }
                else
                {
                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "Something went wrong! Category not deleted!!";
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

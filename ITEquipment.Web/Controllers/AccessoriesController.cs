using ITEquipment.Interfaces;
using ITEquipment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ITEquipment.Models.Consts.EnumConst;
using System.Security.Claims;
using ITEquipment.Utilities;
using System.Reflection.Metadata.Ecma335;

namespace ITEquipment.Web.Controllers
{
    [Authorize]
    public class AccessoriesController : Controller
    {
        private readonly IAccessories _accessories;
        private readonly IAccount _account;
        public AccessoriesController(IAccessories accessories, IAccount account)
        {
            _accessories = accessories;
            _account = account;
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
            catch (Exception ex)
            {

                throw;
            }
           
        }
        public async Task<IActionResult> GetAccessoryList(string sEcho = "", int iDisplayStart = 0, int iDisplayLength = 0, int iSortCol_0 = 0, string sSortDir_0 = "", string sColumns = "", string searchdata = "",string AccessoryName="",string ModelNumber="", string SerialNumber="")
        {
            JQueryDataTableParamModel parm = new JQueryDataTableParamModel();

            parm.sEcho = sEcho;
            parm.iDisplayStart = iDisplayStart;
            parm.iDisplayLength = iDisplayLength;
            parm.sSortDir_0 = sSortDir_0;
            parm.searchdata = searchdata;
            parm.AccessoryName = AccessoryName;
            parm.SerialNumber = SerialNumber;
            parm.ModelNumber = ModelNumber;
            var sColumnsArray = sColumns.Split(',');
            string sortColumn = "";
            if (sColumnsArray.Length > 0 && sColumnsArray.Length >= iSortCol_0)
                sortColumn = Convert.ToString(sColumnsArray[iSortCol_0]);
            try
            {
                if (!string.IsNullOrEmpty(parm.searchdata) || !string.IsNullOrEmpty(parm.AccessoryName) || !string.IsNullOrEmpty(parm.SerialNumber) || !string.IsNullOrEmpty(parm.ModelNumber))
                {
                    parm.searchdata = parm.searchdata.Replace("'", "''");
                    parm.AccessoryName = parm.searchdata.Replace("'", "''");
                    parm.SerialNumber = parm.searchdata.Replace("'", "''");
                    parm.ModelNumber = parm.searchdata.Replace("'", "''");
                }
                AccessoryListModel objList = await _accessories.GetAllAccessories(parm.iDisplayLength, parm.iDisplayStart, sortColumn, parm.sSortDir_0, searchdata, AccessoryName, ModelNumber, SerialNumber);
                return Ok(new
                {
                    sEcho = parm.sEcho,
                    iTotalRecords = objList.AccessoryList.Count,
                    iTotalDisplayRecords = objList.noOfRecords,
                    aaData = objList.AccessoryList
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditAccessory(AccessoriesViewModel accessoriesViewModel)
        {

            MessageModel objModel = new MessageModel();
            try
            {
                // if (ModelState.IsValid)
                //{

                if (accessoriesViewModel.Id > 0)
                {
                    var result = await _accessories.UpdateAccessories(accessoriesViewModel);
                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "Accessory updated successfully!";
                }
                else
                {
                    var result = await _accessories.AddAccessories(accessoriesViewModel);
                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "Accessory added successfully!";
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

           // return Ok(objModel);

              return Json(objModel);

            //return RedirectToAction(nameof(Index));
        }
        public IActionResult AddAccessory()
        {
            return View("AddEditAccessories", new AccessoriesViewModel());
        }
        public async Task<IActionResult> EditAccessory(int Id)
        {
            try
            {
                var result = await _accessories.GetAccessoriesById(Id);

                return PartialView("_AddEditAccessories", result);
            }
            catch (Exception)
            {

                throw;
            }

           
        }
        public async Task<IActionResult> DeleteAccessory(int Id)
        {
            MessageModel objModel = new MessageModel();
            try
            {
                var result = await _accessories.GetAccessoriesById(Id);
                if (result != null && result.Id == Id)
                {
                    await _accessories.RemoveAccessories(Id);
                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "Accessory deleted successfully!";
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
            // return RedirectToAction(nameof(Index));
        }

    }
}

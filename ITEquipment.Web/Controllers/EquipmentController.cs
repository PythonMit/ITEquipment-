using ITEquipment.Interfaces;
using ITEquipment.Models;
using ITEquipment.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace ITEquipment.Web.Controllers
{
    [Authorize]
    public class EquipmentController : Controller
    {

        private readonly IStatus _status;
        private readonly IEquipment _equipment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private static readonly string[] AllowedVideoExtensions = { ".mp4", ".avi", ".mov", ".mkv" };
        private static readonly string[] AllowedDocumentExtensions = { ".pdf" };
        private readonly long _maxFileSize;
        private readonly IConfiguration _configuration;
        public EquipmentController(IStatus status, IEquipment equipment, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _status = status;
            _equipment = equipment;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _maxFileSize = configuration.GetValue<long>("FileUpload:MaxFileSize");
        }
        [Authorize]
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
                // ViewBag.Status = await _status.GetEquipmentStatusType("0");
                ViewBag.Location = await _status.GetLocation("0");
                ViewBag.Manufacturer = await _status.GetManufacturers("0");
                ViewBag.Status = await _status.GetEquipmentMasterStatus("0");
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<IActionResult> GetEquipmentList(string sEcho = "", int iDisplayStart = 0, int iDisplayLength = 0, int iSortCol_0 = 0, string sSortDir_0 = "", string sColumns = "", string searchdata = "", string EquipmentName = "", string AssetsTag = "", string Location = "", string Status = "")
        {
            JQueryDataTableParamModel parm = new JQueryDataTableParamModel();

            parm.sEcho = sEcho;
            parm.iDisplayStart = iDisplayStart;
            parm.iDisplayLength = iDisplayLength;
            parm.sSortDir_0 = sSortDir_0;
            parm.searchdata = searchdata;
            parm.EquipmentName = EquipmentName;
            parm.AssetsTag = AssetsTag;
            parm.Location = Location;
            parm.Status = Status;
            var sColumnsArray = sColumns.Split(',');
            string sortColumn = "";
            if (sColumnsArray.Length > 0 && sColumnsArray.Length >= iSortCol_0)
                sortColumn = Convert.ToString(sColumnsArray[iSortCol_0]);
            try
            {
                if (!string.IsNullOrEmpty(parm.searchdata) || !string.IsNullOrEmpty(parm.EquipmentName) || !string.IsNullOrEmpty(parm.Location) || !string.IsNullOrEmpty(parm.AssetsTag) || !string.IsNullOrEmpty(parm.Status))
                {
                    parm.searchdata = parm.searchdata.Replace("'", "''");
                    parm.EquipmentName = parm.searchdata.Replace("'", "''");
                    parm.AssetsTag = parm.searchdata.Replace("'", "''");
                    parm.Location = parm.searchdata.Replace("'", "''");
                    parm.Status = parm.searchdata.Replace("'", "''");
                }
                EquipmentListModel objList = await _equipment.GetAllEquipmentList(parm.iDisplayLength, parm.iDisplayStart, sortColumn, parm.sSortDir_0, searchdata, EquipmentName, AssetsTag, Location, Status);
                return Ok(new
                {
                    sEcho = parm.sEcho,
                    iTotalRecords = objList.EquipmentList.Count,
                    iTotalDisplayRecords = objList.noOfRecords,
                    aaData = objList.EquipmentList
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AddEditEquipment(EquipmentDTOModel equipmentModel, string AccessoryListJson)
        {

            MessageModel objModel = new MessageModel();
            try
            {
                //if (ModelState.IsValid)
                //{
                if (equipmentModel.Image != null)
                {

                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string uploadsFolderPath = Path.Combine(wwwRootPath, "EquipmentsImages");
                    if (!Directory.Exists(uploadsFolderPath))
                    {
                        Directory.CreateDirectory(uploadsFolderPath);
                    }
                    string fileName = Path.GetFileNameWithoutExtension(equipmentModel.Image.FileName);
                    string extension = Path.GetExtension(equipmentModel.Image.FileName);
                    if (!AllowedExtensions.Contains(extension))
                    {
                        return BadRequest($"Invalid file type. Only the following image formats are allowed: {string.Join(", ", AllowedExtensions)}");
                    }
                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string uniqueFileName = fileName + "_" + Path.GetRandomFileName() + "_" + timestamp + extension;
                    string filePath = Path.Combine(uploadsFolderPath, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await equipmentModel.Image.CopyToAsync(stream);
                    }
                    equipmentModel.ImagePath = "/EquipmentsImages/" + uniqueFileName.ToString();



                }
                if (equipmentModel.Video != null)
                {

                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string uploadsVideoFolderPath = Path.Combine(wwwRootPath, "EquipmentsVideo");
                    if (!Directory.Exists(uploadsVideoFolderPath))
                    {
                        Directory.CreateDirectory(uploadsVideoFolderPath);
                    }
                    string fileName = Path.GetFileNameWithoutExtension(equipmentModel.Video.FileName);
                    string extension = Path.GetExtension(equipmentModel.Video.FileName);
                    if (!AllowedVideoExtensions.Contains(extension))
                    {
                        return BadRequest($"Invalid file type. Only the following image formats are allowed: {string.Join(", ", AllowedExtensions)}");
                    }
                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string uniqueFileName = fileName + "_" + Path.GetRandomFileName() + "_" + timestamp + extension;
                    string filePath = Path.Combine(uploadsVideoFolderPath, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await equipmentModel.Video.CopyToAsync(stream);
                    }
                    equipmentModel.VideoPath = "/EquipmentsVideo/" + uniqueFileName.ToString();


                }
                if (equipmentModel.Document != null)
                {

                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string uploadsDocumentFolderPath = Path.Combine(wwwRootPath, "EquipmentsDocument");
                    if (!Directory.Exists(uploadsDocumentFolderPath))
                    {
                        Directory.CreateDirectory(uploadsDocumentFolderPath);
                    }
                    string fileName = Path.GetFileNameWithoutExtension(equipmentModel.Document.FileName);
                    string extension = Path.GetExtension(equipmentModel.Document.FileName);
                    if (!AllowedDocumentExtensions.Contains(extension))
                    {
                        return BadRequest($"Invalid file type. Only the following image formats are allowed: {string.Join(", ", AllowedExtensions)}");
                    }
                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string uniqueFileName = fileName + "_" + Path.GetRandomFileName() + "_" + timestamp + extension;
                    string filePath = Path.Combine(uploadsDocumentFolderPath, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await equipmentModel.Document.CopyToAsync(stream);
                    }
                    equipmentModel.DocumentPath = "/EquipmentsDocument/" + uniqueFileName.ToString();


                }

                if (equipmentModel.Image != null && equipmentModel.Image.Length > _maxFileSize)
                {

                    objModel.Success = false;
                    objModel.MessageType = "error";
                    objModel.ResultMessage = "Image file size exceeds " + _maxFileSize / 1024 + " KB";
                }
                else if (equipmentModel.Video != null && equipmentModel.Video.Length > _maxFileSize)
                {
                    objModel.Success = false;
                    objModel.MessageType = "error";
                    objModel.ResultMessage = "Video file size exceeds " + _maxFileSize / 1024 + " KB";
                }
                else if (equipmentModel.Document != null && equipmentModel.Document.Length > _maxFileSize)
                {
                    objModel.Success = false;
                    objModel.MessageType = "error";
                    objModel.ResultMessage = "Document file size exceeds " + _maxFileSize / 1024 + " KB";
                }
                else
                {
                    if (equipmentModel.Id > 0)
                    {
                        var result = await _equipment.UpdateEquipment(equipmentModel, AccessoryListJson);
                        objModel.Success = true;
                        objModel.MessageType = "success";
                        objModel.ResultMessage = "Equipment updated successfully!";
                    }
                    else
                    {
                        var result = await _equipment.AddEquipment(equipmentModel, AccessoryListJson);
                        objModel.Success = true;
                        objModel.MessageType = "success";
                        objModel.ResultMessage = "Equipment added successfully!";
                    }
                }





                //  }

            }
            catch (Exception ex)
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

        public async Task<IActionResult> EditEquipment(int Id)
        {
            try
            {
                var result = await _equipment.GetEquipmentById(Id);
                ViewBag.Status = await _status.GetEquipmentMasterStatus("0");
                ViewBag.Location = await _status.GetLocation("0");
                ViewBag.ImageFilePath = result.ImagePath;
                ViewBag.VideoPath = result.VideoPath;
                ViewBag.DocumentPath = result.DocumentPath;
                ViewBag.Manufacturer = await _status.GetManufacturers("0");
                if (Id <= 0)
                {
                    return PartialView("_AddEditEquipment");
                }
                return PartialView("_AddEditEquipment", result);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<IActionResult> DeleteEquipment(int Id)
        {
            MessageModel objModel = new MessageModel();
            try
            {
                var result = await _equipment.GetEquipmentById(Id);

                if (result != null && result.Id == Id)
                {
                    await _equipment.RemoveEquipment(Id);
                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "Equipment deleted successfully!";
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
        public async Task<IActionResult> DeleteEquipmentFile(int Id, string FileType)
        {
            MessageModel objModel = new MessageModel();
            try
            {
                var result = await _equipment.GetEquipmentById(Id);
                if (result != null && result.Id == Id)
                {
                    await _equipment.RemoveEquipmentFile(Id, FileType);
                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "Equipment File deleted successfully!";
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
        [HttpPost]
        public async Task<IActionResult> DeleteEquipmentFileFromRoot(int Id, string FileType, string FullPath)
        {
            MessageModel objModel = new MessageModel();
            try
            {
                string fileName = Path.GetFileName(FullPath);
                var filePath = string.Empty;
                switch (FileType)
                {
                    case "ImageFile":
                        filePath = Path.Combine(_webHostEnvironment.WebRootPath, "EquipmentsImages", fileName);
                        break;
                    case "VideoFile":
                        filePath = Path.Combine(_webHostEnvironment.WebRootPath, "EquipmentsVideo", fileName);
                        break;
                    case "DocumentFile":
                        filePath = Path.Combine(_webHostEnvironment.WebRootPath, "EquipmentsDocument", fileName);
                        break;
                }
                if (System.IO.File.Exists(filePath))
                {
                    // Delete the file
                    System.IO.File.Delete(filePath);
                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "Equipment File deleted successfully!";
                }
                else
                {
                    objModel.Success = false;
                    objModel.MessageType = "error";
                    objModel.ResultMessage = "Something went wrong! Equipment file not deleted!!";
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


        public async Task<IActionResult> DeleteAccessoryMapping(int Id)
        {
            MessageModel objModel = new MessageModel();
            try
            {
                var result = await _equipment.RemoveAccessoryMapping(Id);

                if (result)
                {
                    objModel.Success = true;
                    objModel.MessageType = "Success";
                    objModel.ResultMessage = "Associated Accessory deleted successfully!";
                }
                else
                {
                    objModel.Success = false;
                    objModel.MessageType = "Error";
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
        [HttpPost]
        public async Task<IActionResult> UpdateEquipmentStatus(int EquipmentId, string StatusName)
        {

            MessageModel objModel = new MessageModel();
            try
            {
                if (EquipmentId > 0 && !string.IsNullOrEmpty(StatusName))
                {
                    var result = await _equipment.UpdateEquipmentStatus(EquipmentId, StatusName);
                    if (result)
                    {
                        objModel.Success = true;
                        objModel.MessageType = "Success";
                        objModel.ResultMessage = "Equipment Status update successfully!";
                    }
                }
                else
                {
                    objModel.Success = false;
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

    }

}

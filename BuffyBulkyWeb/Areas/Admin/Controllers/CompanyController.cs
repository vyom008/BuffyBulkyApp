using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BuffyBulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {
            
            if(id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }

        }

        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (ModelState.IsValid)
            {

                if(CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);

                    //*****I updated this myself, if incase have to go back to original - Delete or comment out below*****
                    _unitOfWork.Save();
                    TempData["success"] = "Company created successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);

                    //*****I updated this myself, if incase have to go back to original - Delete or comment out below*****
                    _unitOfWork.Save();
                    TempData["success"] = "Company updated successfully";
                    return RedirectToAction("Index");
                }

                //*****I commented this myself, if incase have to go back to original uncomment below and comment out above*****
                //_unitOfWork.Save();
                //TempData["success"] = "Company created successfully";
                //return RedirectToAction("Index");
            }
            else
            {
                return View(CompanyObj);
            }

        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if(CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }


            _unitOfWork.Company.Remove(CompanyToBeDeleted); 
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Succesful" });
        }

        #endregion
    }
}
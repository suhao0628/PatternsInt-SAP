using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAP.NET6.Services.Catalogue;
using SAP.NET6.ViewModels.Catalogue;
using SAP.NET6.ViewModels.Catalogue.Admin;

namespace SAP.NET6.Controllers.Admin
{
    [Authorize]
    [Route("admin/catalogue")]
    public class CatalogueAdminController : Controller
    {
        private ICatalogueAdministration CatalogueAdministration { get; }

        private ICatalogueDataProvider CatalogueDataProvider { get; }

        public CatalogueAdminController(ICatalogueAdministration catalogueAdministration,
            ICatalogueDataProvider catalogueDataProvider)
        {
            CatalogueAdministration = catalogueAdministration;
            CatalogueDataProvider = catalogueDataProvider;
        }

        [Route("")]
        public async Task<ActionResult> Index()
        {
            var catalogue = await CatalogueDataProvider.GetCatalogueAsync();
            return View(catalogue);
        }

        [Route("node/{id}")]
        public async Task<ActionResult> Node(Guid id)
        {
            var catalogue = await CatalogueDataProvider.GetCatalogueAsync(id);
            return View("Index", catalogue);
        }

        [Route("create_category")]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [Route("create_category")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCategory(CreateCategoryViewModel category)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View();
                }

                await CatalogueAdministration.CreateCategoryAsync(category);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                return View();
            }
        }

        [Route("create_item")]
        public ActionResult CreateItem()
        {
            return View();
        }

        [HttpPost]
        [Route("create_item")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateItem(CreateItemViewModel item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                await CatalogueAdministration.CreateItemAsync(item);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                return View();
            }
        }

        // Never implement delete of something by GET request!
        [Route("delete_item/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await CatalogueAdministration.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        [Route("upload_file")]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        [Route("upload_file")]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            if (file == null)
            {
                // It will be good to show error message for user
                return RedirectToAction("Index");
            }

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(path, file.FileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Here you can process uploaded file

            return RedirectToAction("Index");
        }
    }
}
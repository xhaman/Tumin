using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tumin.Data;
using Tumin.Models;
using Tumin.Services;

namespace Tumin.Controllers
{
    public class UserInformationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserInformationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserInformations
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserInformation.ToListAsync());
        }

        // GET: UserInformations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInformation = await _context.UserInformation
                .SingleOrDefaultAsync(m => m.UserId == id);
            if (userInformation == null)
            {
                return NotFound();
            }

            var avatar = await _context.AvatarImage.FirstOrDefaultAsync(a => a.UserId == id);
            if(avatar!=null)
                ViewBag.avatarUrl = avatar.Url ?? "";
            else
                ViewBag.avatarUrl = "";

            return View(userInformation);
        }

        // GET: UserInformations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FistName,LastName,MothersLastName,Curp,Address,State,ZipCode,rfc,Latitude,Longitude")] UserInformation userInformation)
        {
            if (ModelState.IsValid)
            {
                userInformation.UserId = Guid.NewGuid();
                _context.Add(userInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userInformation);
        }

        // GET: UserInformations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInformation = await _context.UserInformation.SingleOrDefaultAsync(m => m.UserId == id);
            if (userInformation == null)
            {
                return NotFound();
            }
            return View(userInformation);
        }

        // POST: UserInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserId,FistName,LastName,MothersLastName,Curp,Address,State,ZipCode,rfc,Latitude,Longitude")] UserInformation userInformation)
        {
            if (id != userInformation.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInformationExists(userInformation.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details),new {id=userInformation.UserId });
            }
            return View(userInformation);
        }

        // GET: UserInformations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInformation = await _context.UserInformation
                .SingleOrDefaultAsync(m => m.UserId == id);
            if (userInformation == null)
            {
                return NotFound();
            }

            return View(userInformation);
        }

        // POST: UserInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userInformation = await _context.UserInformation.SingleOrDefaultAsync(m => m.UserId == id);
            _context.UserInformation.Remove(userInformation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult BusinessLocation(Guid id)
        {
            var userInfo = _context.UserInformation.Find(id);
            return View(userInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> BusinessLocation(UserInformation userInfo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInformationExists(userInfo.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Administration");
                }


                return RedirectToAction("AvatarUpload", new {id = userInfo.UserId});
                //switch (userInfo.UserType)
                //{
                //    case (int)UserTypes.Business:
                //        return RedirectToAction("AllUsers");
                //    case (int)UserTypes.Artist:
                //        return RedirectToAction("AvatarUpload");
                //    default:
                //        break;
                //}
            }
            return View(userInfo);
        }

        [HttpPost]
        public ActionResult SearchByLocation(float latitude, float longitude, int tipo_comercio)
        {

            List<UserInformation> BusinessLocation;
            try
            {
                using (var db = _context)
                {
                    BusinessLocation = db.UserInformation.FromSql("SELECT * FROM UserInformations WHERE dbo.DistanceBetween({0}, {1}, Latitude, Longitude) < 5", latitude, longitude).ToList();
                    // return BusinessLocation.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            var jsonComercios = from comercio in BusinessLocation.AsEnumerable()
                                select JsonComercioFromComercio(comercio);



            return Json(jsonComercios.ToList());
            //return View();
        }

        private JsonComercio JsonComercioFromComercio(UserInformation comercio)
        {
            return new JsonComercio
            {
                ComercioId = comercio.UserId,
                Nombre = comercio.FistName,
                Latitud = comercio.Latitude,
                Longitud = comercio.Longitude,
                Direccion = comercio.Address,
                Url = comercio.UserId.ToString()

            };
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AvatarUpload(Guid id)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AvatarUpload(Guid Id, List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var AzureStorage = new AzureStorageService();
            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                        var imageUrl = await AzureStorage.UploadFileToContainer(formFile, ".jpg");

                        var imageAvatar = new AvatarImage()
                        {
                            Id = Guid.NewGuid(),
                            UserId = Id,
                            Url = imageUrl,
                            UploadDate = DateTime.Now,
                            IsCurrentAvatar = true

                        };

                        _context.AvatarImage.Add(imageAvatar);
                        _context.SaveChanges();

                    }
                }
            }


            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return RedirectToAction("Details", "UserInformation", new { id = Id });
        }


        private bool UserInformationExists(Guid id)
        {
            return _context.UserInformation.Any(e => e.UserId == id);
        }
    }

    public class JsonComercio
    {
        public Guid ComercioId { get; set; }
        public string Nombre { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public string Direccion { get; set; }
        public string Url { get; set; }

    }
}

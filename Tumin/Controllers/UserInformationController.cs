using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tumin.Data;
using Tumin.Models;

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
                return RedirectToAction(nameof(Index));
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

        private bool UserInformationExists(Guid id)
        {
            return _context.UserInformation.Any(e => e.UserId == id);
        }
    }
}

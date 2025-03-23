using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FastFood.Data;
using FastFood.Models;
using Microsoft.AspNetCore.Http;

namespace FastFood.Controllers
{
    public class DoAnNhanhController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DoAnNhanhController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: DoAnNhanh
        public async Task<IActionResult> Index(string? tuKhoa = null)
        {
            var danhSachDoAnNhanh = _context.DoAnNhanhs.Include(d => d.DanhMuc).AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                danhSachDoAnNhanh = danhSachDoAnNhanh.Where(d =>
                    d.MonAn.Contains(tuKhoa) || d.MoTa.Contains(tuKhoa));
            }

            ViewBag.TuKhoa = tuKhoa; // Trả về từ khóa cho View
            return View(await danhSachDoAnNhanh.ToListAsync());
        }

        // GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doAnNhanh = await _context.DoAnNhanhs
                .Include(d => d.DanhMuc)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (doAnNhanh == null)
            {
                return NotFound();
            }

            return View(doAnNhanh);
        }

        // GET: Create
        public IActionResult Create()
        {
            ViewData["DanhMucID"] = new SelectList(_context.DanhMucs, "DanhMucID", "TenDanhMuc");
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MonAn,MoTa,Gia,ImageFile,DanhMucID")] DoAnNhanh doAnNhanh)
        {
            if (ModelState.IsValid)
            {
                if (doAnNhanh.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + doAnNhanh.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await doAnNhanh.ImageFile.CopyToAsync(fileStream);
                    }

                    doAnNhanh.HinhAnhUrl = "/images/" + uniqueFileName;
                }

                _context.Add(doAnNhanh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DanhMucID"] = new SelectList(_context.DanhMucs, "DanhMucID", "TenDanhMuc", doAnNhanh.DanhMucID);
            return View(doAnNhanh);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doAnNhanh = await _context.DoAnNhanhs.FindAsync(id);
            if (doAnNhanh == null)
            {
                return NotFound();
            }

            ViewData["DanhMucID"] = new SelectList(_context.DanhMucs, "DanhMucID", "TenDanhMuc", doAnNhanh.DanhMucID);
            return View(doAnNhanh);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,MonAn,MoTa,Gia,HinhAnhUrl,DanhMucID")] DoAnNhanh doAnNhanh)
        {
            if (id != doAnNhanh.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doAnNhanh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoAnNhanhExists(doAnNhanh.ID))
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

            ViewData["DanhMucID"] = new SelectList(_context.DanhMucs, "DanhMucID", "TenDanhMuc", doAnNhanh.DanhMucID);
            return View(doAnNhanh);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doAnNhanh = await _context.DoAnNhanhs
                .Include(d => d.DanhMuc)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (doAnNhanh == null)
            {
                return NotFound();
            }

            return View(doAnNhanh);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doAnNhanh = await _context.DoAnNhanhs.FindAsync(id);
            if (doAnNhanh != null)
            {
                _context.DoAnNhanhs.Remove(doAnNhanh);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DoAnNhanhExists(int id)
        {
            return _context.DoAnNhanhs.Any(e => e.ID == id);
        }
    }
}

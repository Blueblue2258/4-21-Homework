#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _4._21Homework.Models;

namespace _4._21Homework.Controllers
{
    public class FoodsController : Controller
    {
        private readonly HomeworkContext _context;

        public FoodsController(HomeworkContext context)
        {
            _context = context;
        }

        // GET: Foods
        public async Task<IActionResult> Index()
        {
            return View(await _context.Foods.ToListAsync());
        }

        // GET: Foods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Foods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // GET: Foods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Style,Stars,Price,Comment")] Food food)
        {
            if (ModelState.IsValid)
            {
                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(food);
        }

        // GET: Foods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Style,Stars,Price,Comment")] Food food)
        {
            if (id != food.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(food);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodExists(food.Id))
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
            return View(food);
        }

        // GET: Foods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Foods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Search()
        {
            ViewData["Message"] = "英雄搜尋 GET => 取得表單";
            return View(new BlueSearchViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search([Bind("Name,MinStare,MaxStare")] FoodSearchParams searchParams)
        {

            var viewModel = new BlueSearchViewModel(); // 清出記憶體空間 => 打掃房間
            if (ModelState.IsValid)
            {
                var searchResult = _context.Foods.ToList(); // 取出所有資料
                if (!string.IsNullOrEmpty(searchParams.Name)) // 如果有輸入名字，就用名字當條件搜尋
                {
                    searchResult = searchResult
                        .Where(x => x.Name == searchParams.Name)
                        .ToList();
                }

                // 如果攻擊力的範圍合邏輯，就用加攻擊力條件再多篩一次
                if (searchParams.MinStars >= 0
                    && searchParams.MaxStars > 0
                    && searchParams.MinStars < searchParams.MaxStars)
                {
                    // 最小 ATK = 10；最大 ATK = 20
                    // ==> 那些英雄攻擊歷介在 10~20 之間
                    searchResult = searchResult
                        .Where(x => x.Stars >= searchParams.MinStars && x.Stars <= searchParams.MaxStars)
                        .ToList();
                }
                else // 如果攻擊力的範圍不合邏輯，就顯示忽略攻擊力條件字樣
                {
                    ViewData["Message"] = $"（忽略星星搜尋條件）";
                }

                // 用 ViewData 讓 Controller 與 View 共享資料
                ViewData["Message"] += $"搜尋到 {searchResult.Count} 個星星";

                // 把搜尋條件與搜尋結果賦值到 ViewModel 的 property
                // MVC 才有辦法幫我們把資料打包進 Search.cshtml
                viewModel.SearchParams = searchParams;
                viewModel.Foods = searchResult;
            }
            // 回傳 View + ViewModel 進行打包作業
            return View(viewModel);
        }
        private bool FoodExists(int id)
        {
            return _context.Foods.Any(e => e.Id == id);
        }
    }
}

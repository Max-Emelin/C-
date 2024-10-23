using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;

        public ClubController(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _clubRepository.GetAll());
        }

        public async Task<IActionResult> Detail(int id)
        {
            return View(await _clubRepository.GetByIdAsync(id));
        }

        public IActionResult Create()
        {
            ViewBag.Action = "create";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Club club)
        {
            if (ModelState.IsValid)
            {
                _clubRepository.Add(club);

                return RedirectToAction(nameof(Index));
            }

            return View(club);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Action = "edit";

            return View(await _clubRepository.GetByIdAsync(id.HasValue ? id.Value : 0));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Club club)
        {
            if (ModelState.IsValid)
            {
                _clubRepository.Update(club);

                return RedirectToAction(nameof(Index));
            }

            return View(club);
        }
    }
}
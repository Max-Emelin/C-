using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repository;

namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;

        public RaceController(IRaceRepository raceRepository)
        {
            _raceRepository = raceRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _raceRepository.GetAll());
        }

        public async Task<IActionResult> Detail(int id)
        {
            return View(await _raceRepository.GetByIdAsync(id));
        }

        public IActionResult Create()
        {
            ViewBag.Action = "create";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Race race)
        {
            if (ModelState.IsValid)
            {
                _raceRepository.Add(race);

                return RedirectToAction(nameof(Index));
            }

            return View(race);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Action = "edit";

            return View(await _raceRepository.GetByIdAsync(id.HasValue ? id.Value : 0));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Race race)
        {
            if (ModelState.IsValid)
            {
                _raceRepository.Update(race);

                return RedirectToAction(nameof(Index));
            }

            return View(race);
        }
    }
}
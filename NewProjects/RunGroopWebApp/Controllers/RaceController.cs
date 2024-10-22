using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;

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
    }
}
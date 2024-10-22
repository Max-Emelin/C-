using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;

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
    }
}
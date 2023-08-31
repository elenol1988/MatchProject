using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Client.Client.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NToastNotify;
using Shared;
using System;


namespace Client.Controllers
{
    [Authorize]
    public class MatchesController : BaseController
    {
        private readonly ILogger<MatchesController> _logger;
        private readonly IMatchClient _matchClient;
        private readonly IToastNotification _toastNotification;
        public MatchesController(ILogger<MatchesController> logger, IMatchClient matchClient,IToastNotification toastNotification)
        {
            _logger = logger;
            _matchClient = matchClient;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var matches = await _matchClient.GetMatchesAsync(GetToken);
                return View(matches);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(new NotFoundResult());
            }
          
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            try
            {
                var match = await _matchClient.GetMatchByIdAsync(Id,GetToken);
                return View(match);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(new NotFoundResult());
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Match match)
        {
            try
            {
                var matches = await _matchClient.PutMatchAsync(match,GetToken);
                _toastNotification.AddSuccessToastMessage("Success update");
                return RedirectToAction("Index", "Matches");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(new NotFoundResult());
            }

        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
               
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Match match)
        {
            try
            {
                var matches = await _matchClient.PostMatchAsync(match, GetToken);
                _toastNotification.AddSuccessToastMessage("Success");
                return RedirectToAction("Index", "Matches");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(new NotFoundResult());
            }

        }
        [HttpPost]
        
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var matches = await _matchClient.DeleteMatchAsync(id, GetToken);
                return RedirectToAction("Index", "Matches");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(new NotFoundResult());
            }

        }
    }
}

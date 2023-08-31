using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Api.Repositories.Interface;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Shared.Account;
using Shared;
using System;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ILogger<MatchesController> _logger;
        public MatchesController(IMatchRepository matchRepository, ILogger<MatchesController> logger)
        {
            _matchRepository =matchRepository;
            _logger = logger;
        }

        // GET: api/Matches
        [HttpGet]
        [Route("getmatches")]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatch()
        {
            try
            {
                return Ok(await _matchRepository.GetMatch());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
           
        }

        // GET: api/Matches/5
        [HttpGet]
        [Route("getmatch/{id}")]
        public async Task<ActionResult<Match>> GetMatch(int id)
        {
            try
            {
                var match = await _matchRepository.GetMatchById(id);

                if (match == null)
                {
                    return NotFound();
                }

                return match;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            
        }

        // PUT: api/Matches/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Route("putmatch")]
        public async Task<IActionResult> PutMatch(Match match)
        {
            try
            {
                await _matchRepository.PutMatch( match);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await MatchExists(match.Id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex.Message);
                    return BadRequest(ex.Message);
                }
            }

            return NoContent();
        }

        // POST: api/Matches
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("postmatch")]
        public async Task<ActionResult<Match>> PostMatch(Match match)
        {
            try
            {
                await _matchRepository.PostMatch(match);
              
                return CreatedAtAction("GetMatch", new { id = match.Id }, match);
            }
            catch(Exception ex)
            {
                _logger?.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            
        }

        // DELETE: api/Matches/5
        [HttpDelete]
        [Route("deletematch/{id}")]
        public async Task<ActionResult<Match>> DeleteMatch(int id)
        {
            try
            {
                var match = await _matchRepository.DeleteMatch(id);
                if (match == null)
                {
                    return NotFound();
                }

                return match;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
           
        }

        private async Task<bool> MatchExists(int id)
        {
            return await _matchRepository.MatchExists(id);
        }
    }
}

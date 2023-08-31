using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Api.Repositories.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Models;
using System.Linq;
using Shared;




namespace Api.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly ILogger<MatchRepository> _logger;
        private readonly ApplicationDbContext _context;

        public MatchRepository(ApplicationDbContext context, ILogger<MatchRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<Match>> GetMatch()
        {
           return  await _context.Match.Include(m => m.MatchOdds).ToListAsync();
        }

        public async  Task<Match> GetMatchById(int id)
        {
            return  await _context.Match.Include(m => m.MatchOdds).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task PostMatch(Match match)
        {
            _context.Match.Add(match);
           
            await _context.SaveChangesAsync();
            var matchOdds = new MatchOdds
            {
                MatchId = match.Id,
                Specifier = match.MatchOdds.Specifier,
                Odd = match.MatchOdds.Odd,
                Match = match
                
            };
            _context.MatchOdds.Add(matchOdds);

            await _context.SaveChangesAsync();
        }

        public async Task PutMatch(Match match)
        {
            _context.Entry(match).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _context.Entry(match.MatchOdds).State = EntityState.Modified;
            await _context.SaveChangesAsync();
           
        }
        public async Task<Match> DeleteMatch(int id)
        {
            var match = await _context.Match.FirstOrDefaultAsync(s => s.Id == id);
            if (match == null)
            {
                return null;
            }

            _context.Match.Remove(match);
            await _context.SaveChangesAsync();
            return match;

        }
        public async Task<bool> MatchExists(int id)
        {
            return _context.Match.Any(e => e.Id == id); ;

        }

       
    }
}

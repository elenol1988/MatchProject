using System.Collections.Generic;
using System.Threading.Tasks;
using Shared;

namespace Client.Client.Interfaces
{
    public interface IMatchClient
    {
        Task<IEnumerable<Match>> GetMatchesAsync(string token);
        Task<Match> GetMatchByIdAsync (int id, string token);
        Task<Match> PutMatchAsync(Match match,string token);
        Task<Match> PostMatchAsync(Match match, string token);
        Task<Match> DeleteMatchAsync(int id, string token);
    }
}

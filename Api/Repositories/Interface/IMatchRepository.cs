using System.Collections.Generic;
using System.Threading.Tasks;
using Shared;

namespace Api.Repositories.Interface
{
    public interface IMatchRepository
    {
        Task<IEnumerable<Match>> GetMatch();
        Task<Match> GetMatchById(int id);
        Task PutMatch(Match match);
        Task PostMatch(Match match);
        Task<Match> DeleteMatch(int id);
        Task<bool> MatchExists(int id);
    }
}

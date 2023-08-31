using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Client.Client.Interfaces;
using System.Threading.Tasks;
using ClientProvider;

using Shared;
using System;


namespace Client.Client
{
    public class MatchClient : IMatchClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<MatchClient> _logger;
        private readonly IClientProvider _clientProvider;
        public MatchClient(IHttpContextAccessor httpContextAccessor, ILogger<MatchClient> logger, IClientProvider clientProvider)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _clientProvider = clientProvider;

        }        
        public async Task<IEnumerable<Match>> GetMatchesAsync( string token)
        {
            try
            {
                string url = String.Format(Startup.ApiUrl + "/matches/getmatches");
                return await _clientProvider.GetAsync<IEnumerable<Match>>(url,token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return default(IEnumerable<Match>);
            }
        }
        public async Task<Match> GetMatchByIdAsync(int id,string token)
        {
            try
            {
                string url = String.Format(Startup.ApiUrl + "/matches/getmatch/{0}", id);
                return await _clientProvider.GetAsync<Match>(url, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return default(Match);
            }
        }
        public async Task<Match> PutMatchAsync(Match match, string token)
        {
            try
            {
                string url = String.Format(Startup.ApiUrl + "/matches/putmatch");
                return await _clientProvider.PutAsync<Match, Match>(url, token, match);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return default(Match);
            }
        }

        public async Task<Match> PostMatchAsync(Match match, string token)
        {
                try
                {
                    string url = String.Format(Startup.ApiUrl + "/matches/postmatch");
                    return await _clientProvider.PostAsync<Match, Match>(url, token, match);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return default(Match);
                }
            }
        public async Task<Match> DeleteMatchAsync(int id, string token)
        {
            try
            {
                string url = String.Format(Startup.ApiUrl + "/matches/deletematch/{0}", id);
                return await _clientProvider.DeleteAsync<Match>(url, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return default(Match);
            }
        }
    }
}

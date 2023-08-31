using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientProvider
{
    public interface IClientProvider
    {
        Task<T> PostAsync<T, TK>(string apiUrl, string token, TK postModel);
        Task<T> PostNoAuthAsync<T, TK>(string apiUrl, TK postModel);
        Task<T> PutAsync<T, TK>(string apiUrl, string token, TK postModel);
        Task<T> DeleteAsync<T>(string apiUrl, string token);
        Task<T> GetAsync<T>(string apiUrl, string token);
    }
}

using Newtonsoft.Json;
using Serilog;
using Shared.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientProvider
{
    public class ClientProvider : IClientProvider
    {
        static HttpClient client = new HttpClient();

        public async Task<T> GetAsync<T>(string apiUrl, string token)
        {
            T result = default(T);
            try
            {

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestTimeout = Timeout.InfiniteTimeSpan;
                var httpTimeout = Timeout.InfiniteTimeSpan;
                using (var tokenSource = new CancellationTokenSource(requestTimeout))
                {

                    var response = await client.GetAsync(apiUrl, tokenSource.Token).ConfigureAwait(false);
                    await HandleResponse(response);
                    string responseData = await response.Content.ReadAsStringAsync();
                    result = await Task.Run(() => JsonConvert.DeserializeObject<T>(responseData));
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return result;
            }
        }

        public async Task<T> PostAsync<T, TK>(string apiUrl, string token, TK postModel)
        {
            T result = default(T);
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var requestTimeout = Timeout.InfiniteTimeSpan;
                var httpTimeout = Timeout.InfiniteTimeSpan;
                using (var tokenSource = new CancellationTokenSource(requestTimeout))
                {
                    var postcontent = JsonConvert.SerializeObject(postModel);
                    var response = await client.PostAsync(apiUrl, new StringContent(postcontent, Encoding.UTF8, "text/json"), tokenSource.Token).ConfigureAwait(false);
                    await HandleResponse(response);
                    string responseData = await response.Content.ReadAsStringAsync();
                    result = await Task.Run(() => JsonConvert.DeserializeObject<T>(responseData));
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return result;
            }
        }

        public async Task<T> PostNoAuthAsync<T, TK>(string apiUrl, TK postModel)
        {
            T result = default(T);
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();

                var requestTimeout = Timeout.InfiniteTimeSpan;
                var httpTimeout = Timeout.InfiniteTimeSpan;
                using (var tokenSource = new CancellationTokenSource(requestTimeout))
                {
                    var postcontent = JsonConvert.SerializeObject(postModel);
                    var response = await client.PostAsync(apiUrl, new StringContent(postcontent, Encoding.UTF8, "text/json"), tokenSource.Token).ConfigureAwait(false);
                    await HandleResponse(response);
                    string responseData = await response.Content.ReadAsStringAsync();
                    result = await Task.Run(() => JsonConvert.DeserializeObject<T>(responseData));
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return result;
            }
        }
        public async Task<T> PutAsync<T, TK>(string apiUrl, string token, TK postModel)
        {
            T result = default(T);
            try
            {

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestTimeout = Timeout.InfiniteTimeSpan;
                var httpTimeout = Timeout.InfiniteTimeSpan;
                using (var tokenSource = new CancellationTokenSource(requestTimeout))
                {
                    var postcontent = JsonConvert.SerializeObject(postModel);
                    var response = await client.PutAsync(apiUrl, new StringContent(postcontent, Encoding.UTF8, "text/json"), tokenSource.Token).ConfigureAwait(false);
                    await HandleResponse(response);
                    string responseData = await response.Content.ReadAsStringAsync();
                    result = await Task.Run(() => JsonConvert.DeserializeObject<T>(responseData));
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return result;
            }
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var exception = await Task.Run(() => JsonConvert.DeserializeObject<ApiError>(content));
                }
                Log.Error(content);

                throw new HttpRequestException(content);
            }
        }

        public async Task<T> DeleteAsync<T>(string apiUrl, string token)
        {
            T result = default(T);
            try
            {

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var requestTimeout = Timeout.InfiniteTimeSpan;
                var httpTimeout = Timeout.InfiniteTimeSpan;
                using (var tokenSource = new CancellationTokenSource(requestTimeout))
                {

                    var response = await client.DeleteAsync(apiUrl, tokenSource.Token).ConfigureAwait(false);
                    await HandleResponse(response);
                    string responseData = await response.Content.ReadAsStringAsync();
                    result = await Task.Run(() => JsonConvert.DeserializeObject<T>(responseData));
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return result;
            }
        }
    }
}


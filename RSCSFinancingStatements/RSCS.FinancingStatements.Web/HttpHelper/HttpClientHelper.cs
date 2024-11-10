using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;

namespace RSCS.FinancingStatements.Web.HttpHelper
{
    public interface IHttpClientHelper
    {
        Task<Res> GetSingleItemRequest<Res>(string apiUrl, CancellationToken token = default(CancellationToken));
        Task<Res[]> GetMultipleItemsRequest<Res>(string apiUrl, CancellationToken token = default(CancellationToken));
        Task<Res> PostRequest<Req, Res>(string apiUrl, Req postObject, CancellationToken token = default(CancellationToken));
        Task<Res> PostRequest<Req, Res>(string apiUrl, Req postObject, CancellationToken token = default(CancellationToken), Dictionary<string, string> headers = null);
        Task<Res> PostStreamDataRequest<Req, Res>(string apiUrl, byte[] postObject, CancellationToken token = default(CancellationToken));
        Task<Res> PostFormDataRequest<Req, Res>(string apiUrl, Req postObject, CancellationToken token = default(CancellationToken));
        Task<Res> PutRequest<Req, Res>(string apiUrl, Req putObject, CancellationToken token = default(CancellationToken));
        Task DeleteRequest(string apiUrl, CancellationToken token = default(CancellationToken));
        void SetHeaders(Dictionary<string, string> headers);
    }

    public class HttpClientHelper : IHttpClientHelper
    {
        private readonly HttpClient Client;
        public HttpClientHelper()
        {
            Client = new HttpClient();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Res"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Res> GetSingleItemRequest<Res>(string apiUrl, CancellationToken cancellationToken)
        {
            var result = default(Res);
            var response = await Client.GetAsync(apiUrl, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith(x =>
                {
                    if (typeof(Res).Namespace != "System")
                    {
                        result = JsonConvert.DeserializeObject<Res>(x?.Result);
                    }
                    else result = (Res)Convert.ChangeType(x?.Result, typeof(Res));
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Res"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Res[]> GetMultipleItemsRequest<Res>(string apiUrl, CancellationToken cancellationToken)
        {
            Res[] result = null;
            int statuscode;
            string message;
            JsonObject apierror;

            var response = await Client.GetAsync(apiUrl, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    //result = JsonConvert.DeserializeObject<Res[]>(x.Result);
                    JsonObject obj = JsonNode.Parse(x.Result).AsObject();
                    statuscode = (int)obj["statusCode"];

                    if (statuscode == 200)
                    {
                        JsonArray jsonArray = (JsonArray)obj["result"];
                        message = (string)obj["message"];
                        apierror = (JsonObject)obj["responseException"];
                        result = JsonConvert.DeserializeObject<Res[]>(jsonArray.ToJsonString());
                    }

                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Req"></typeparam>
        /// <typeparam name="Res"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="postObject"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Res> PostRequest<Req, Res>(string apiUrl, Req postObject, CancellationToken cancellationToken)
        {
            Res result = default(Res);
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(postObject), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(apiUrl, httpContent, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    result = JsonConvert.DeserializeObject<Res>(x.Result);
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }


        public async Task<Res> PostRequest<Req, Res>(string apiUrl, Req postObject, CancellationToken cancellationToken, Dictionary<string, string> headers = null)
        {
            Res result = default(Res);
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(postObject), Encoding.UTF8, "application/json");

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl))
            {
                requestMessage.Content = httpContent;

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        requestMessage.Headers.Add(header.Key, header.Value);
                    }
                }

                var response = await Client.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Res>(content);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"{response.StatusCode}:{content}");
                }
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Req"></typeparam>
        /// <typeparam name="Res"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="postObject"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Res> PostStreamDataRequest<Req, Res>(string apiUrl, byte[] postObject, CancellationToken cancellationToken)
        {
            Res result = default(Res);
            MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(postObject);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = $"\"\"" };
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            multipartFormDataContent.Add(fileContent);

            var response = await Client.PostAsync(apiUrl, multipartFormDataContent, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    result = JsonConvert.DeserializeObject<Res>(x.Result);
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Req"></typeparam>
        /// <typeparam name="Res"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="postObject"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Res> PostFormDataRequest<Req, Res>(string apiUrl, Req postObject, CancellationToken cancellationToken)
        {
            Res result = default(Res);
            FormUrlEncodedContent httpContent = new FormUrlEncodedContent(postObject.ToDictionary<string>().AsEnumerable());
            var response = await Client.PostAsync(apiUrl, httpContent, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    result = JsonConvert.DeserializeObject<Res>(x.Result);
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Req"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="putObject"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //public async Task<Res> PutRequest<Req, Res>(string apiUrl, Req putObject, CancellationToken cancellationToken)
        //{
        //	Res result = default(Res);
        //	var httpContent = new StringContent(JsonConvert.SerializeObject(putObject), Encoding.UTF8, "application/json");
        //	var response = await Client.PutAsync(apiUrl, httpContent, cancellationToken).ConfigureAwait(false);
        //	if (response.IsSuccessStatusCode)
        //	{
        //		await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
        //		{
        //			result = JsonConvert.DeserializeObject<Res>(x.Result);
        //		}, cancellationToken);
        //	}
        //	else
        //	{
        //		var content = await response.Content.ReadAsStringAsync();
        //		response.Content?.Dispose();
        //		throw new HttpRequestException($"{response.StatusCode}:{content}");
        //	}
        //	return result;
        //}
        public async Task<Res> PutRequest<Req, Res>(string apiUrl, Req putObject, CancellationToken cancellationToken)
        {
            Res result = default(Res);
            var httpContent = new StringContent(JsonConvert.SerializeObject(putObject), Encoding.UTF8, "application/json");
            var response = await Client.PutAsync(apiUrl, httpContent, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    //result = JsonConvert.DeserializeObject<Res>(x.Result);
                    JsonObject obj = JsonNode.Parse(x.Result).AsObject();
                    int statuscode = (int)obj["statusCode"];

                    if (statuscode == 200)
                    {
                        int jsonArray = (int)obj["result"];
                        result = JsonConvert.DeserializeObject<Res>(jsonArray.ToString());
                    }
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DeleteRequest(string apiUrl, CancellationToken cancellationToken)
        {
            var response = await Client.DeleteAsync(apiUrl, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="headers"></param>
        public void SetHeaders(Dictionary<string, string> headers)
        {
            Client.DefaultRequestHeaders.Clear();
            foreach (var item in headers)
            {
                Client.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
            }
        }
    }
}

﻿using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FaceAnalysis
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private static readonly HttpClient httpClient;
        static HttpClientWrapper()
        {
            httpClient = new HttpClient();
        }

        public async Task<string> Post(string url, MultipartFormDataContent httpContent)
        {
            return await PostRequest(url, httpContent);
        }

        private async Task<string> PostRequest(string url, MultipartFormDataContent httpContent, bool repeatedRequest = false)
        {
            try
            {

                using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(url))
                {
                    Version = HttpVersion.Version10,
                    Content = httpContent
                })
                using (var response = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false))
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        return responseString;
                    else
                        throw new HttpRequestException(response.ToString());
                }
            }

            catch (HttpRequestException e)
            {
                if (e.Message == "Error while copying content to a stream." && repeatedRequest == false)
                {
                    Debug.WriteLine("Bad response, attempting to send request again");
                    return await PostRequest(url, httpContent, true);
                }                  
                else
                {
                    Debug.WriteLine(e);
                    return null;
                }                   
            }
        }
    }
}
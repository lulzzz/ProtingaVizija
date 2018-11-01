﻿using System.Net.Http;
using System.Threading.Tasks;

namespace HttpHelpers
{
    public interface IHttpClientWrapper
    {
        Task<string> Post(string url, MultipartFormDataContent httpContent, bool repeatedRequest = false);
        Task<string> Get(string url);
    }
}
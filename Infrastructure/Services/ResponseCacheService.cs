﻿using Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;

        public ResponseCacheService(IConnectionMultiplexer redis)
        {
           _database= redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string cacheKey, object respnse ,TimeSpan timeToLive)
        {
            if (respnse is null)
                return;
            var options= new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var cachedResponse=JsonSerializer.Serialize(respnse,options);
            await _database.StringSetAsync(cacheKey,cachedResponse,timeToLive);
        }

        public async Task<string> GetCachedResponse(string cacheKey)
        {
            var cachedResponse = await _database.StringGetAsync(cacheKey);
            if (cachedResponse.IsNullOrEmpty)
                return null;
            return cachedResponse;
        }
    }
}

using BB.CR.Providers.Bases;
using BB.CR.Providers.Extensions;
using BB.CR.Providers.Messages;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Net;

namespace BB.CR.Rest.Bases
{
    public class BaseHandler
    {
        public static async Task<ReturnResponse<T>> ExecuteAsync<T>(Func<Task<ReturnResponse<T>>> action, ILogger logger)
        {
            var response = new ReturnResponse<T>();

            try
            {
                response = await action().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, BaseVariable.ERROR_MESSAGE, ex.InnerMessage());
                response.Error(HttpStatusCode.InternalServerError, CommonResources.InternalServerError);
            }

            return response;
        }

        public static async Task<ReturnResponse<T>> CacheAsync<T>(Func<Task<ReturnResponse<T>>> action, ILogger logger, IDistributedCache distributedCache, string cacheName)
        {
            var response = new ReturnResponse<T>();

            // Try to get from cache first
            try
            {
                var cacheProvinces = await distributedCache.GetStringAsync(cacheName).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(cacheProvinces))
                {
                    response = JsonConvert.DeserializeObject<ReturnResponse<T>>(cacheProvinces);
                    if (response != null)
                        return response;
                }
            }
            catch (Exception ex)
            {
                // Redis connection error - log and continue without cache
                logger.Log(LogLevel.Warning, "Redis cache unavailable, falling back to database: {Message}", ex.InnerMessage());
            }

            // Cache miss or Redis error - get from database
            try
            {
                response = await action().ConfigureAwait(false);
                
                // Try to cache the result (ignore errors if Redis is still unavailable)
                try
                {
                    var cachedOption = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(2)
                    };
                    var json = JsonConvert.SerializeObject(response);
                    await distributedCache.SetStringAsync(cacheName, json, cachedOption).ConfigureAwait(false);
                }
                catch (Exception cacheEx)
                {
                    // Log but don't fail the request if caching fails
                    logger.Log(LogLevel.Warning, "Failed to cache result: {Message}", cacheEx.InnerMessage());
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, BaseVariable.ERROR_MESSAGE, ex.InnerMessage());
#pragma warning disable CS8604 // Possible null reference argument.
                response.Error(HttpStatusCode.InternalServerError, CommonResources.InternalServerError);
#pragma warning restore CS8604 // Possible null reference argument.
            }

#pragma warning disable CS8603 // Possible null reference return.
            return response;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public static async Task<ReturnResponse<List<KeyValuePair<T, string>>>?> GetEnumAsync<T>(ILogger logger, IDistributedCache distributedCache, string cacheName) where T : Enum
        {
            var response = new ReturnResponse<List<KeyValuePair<T, string>>>();
            
            // Try to get from cache first
            try
            {
                var cacheProvinces = await distributedCache.GetStringAsync(cacheName).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(cacheProvinces))
                {
                    response = JsonConvert.DeserializeObject<ReturnResponse<List<KeyValuePair<T, string>>>>(cacheProvinces);
                    if (response != null)
                        return response;
                }
            }
            catch (Exception ex)
            {
                // Redis connection error - log and continue without cache
                logger.Log(LogLevel.Warning, "Redis cache unavailable, falling back to enum generation: {Message}", ex.InnerMessage());
            }

            // Cache miss or Redis error - generate enum list
            try
            {
                var enums = new List<KeyValuePair<T, string>>();
                await Task.Run(() => enums = EnumExtension.ToList<T>()).ConfigureAwait(false);
                response.Success(enums, CommonResources.Ok);
                
                // Try to cache the result (ignore errors if Redis is still unavailable)
                try
                {
                    var json = JsonConvert.SerializeObject(response);
                    await distributedCache.SetStringAsync(cacheName, json).ConfigureAwait(false);
                }
                catch (Exception cacheEx)
                {
                    // Log but don't fail the request if caching fails
                    logger.Log(LogLevel.Warning, "Failed to cache enum result: {Message}", cacheEx.InnerMessage());
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, BaseVariable.ERROR_MESSAGE, ex.InnerMessage());
                response?.Error(HttpStatusCode.InternalServerError, CommonResources.InternalServerError);
            }
            
            return response;
        }

        public static async Task<T?> CacheAsync<T>(Func<Task<T>> action, ILogger logger, IDistributedCache distributedCache, string cacheName)
        {
            var response = default(T);

            try
            {
                response = await action().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, BaseVariable.ERROR_MESSAGE, ex.InnerMessage());
            }

            return response;
        }

        public static async Task<T?> ExecuteAsync<T>(Func<Task<T>> action, ILogger logger)
        {
            var response = default(T);

            try
            {
                response = await action().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, BaseVariable.ERROR_MESSAGE, ex.InnerMessage());
            }

            return response;
        }
    }
}

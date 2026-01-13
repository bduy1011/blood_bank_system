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

            try
            {
                var cacheProvinces = await distributedCache.GetStringAsync(cacheName).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(cacheProvinces))
                {
                    response = JsonConvert.DeserializeObject<ReturnResponse<T>>(cacheProvinces);
                }
                else
                {
                    response = await action().ConfigureAwait(false);
                    #region Xử lý expire để recache lấy dữ liệu mới.
                    var cachedOption = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(2)
                    };
                    var json = JsonConvert.SerializeObject(response);
                    await distributedCache.SetStringAsync(cacheName, json, cachedOption).ConfigureAwait(false);
                    #endregion
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
            try
            {
                var cacheProvinces = await distributedCache.GetStringAsync(cacheName).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(cacheProvinces))
                {
                    response = JsonConvert.DeserializeObject<ReturnResponse<List<KeyValuePair<T, string>>>>(cacheProvinces);
                }
                else
                {
                    var enums = new List<KeyValuePair<T, string>>();
                    await Task.Run(() => enums = EnumExtension.ToList<T>()).ConfigureAwait(false);
                    response.Success(enums, CommonResources.Ok);
                    var json = JsonConvert.SerializeObject(response);
                    await distributedCache.SetStringAsync(cacheName, json).ConfigureAwait(false);
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

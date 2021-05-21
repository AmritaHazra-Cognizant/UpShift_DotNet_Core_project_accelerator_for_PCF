using DotNetCore.Framework.Interception;
using DotNetCore.Framework.Interception.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore.Framework.Caching.CoreCaching;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace DotNetCore.API.Caching.MemoryCache
{
    public class MemoryCacheInterceptor : IMethodInterceptor
    {
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly ICacheConfigFactory _cacheConfigFactory;
        private MemoryCacheAttribute _memoryCacheAttribute;
        public MemoryCacheInterceptor(IMemoryCacheService memoryCacheService, ICacheConfigFactory cacheConfigFactory)
        {
            _memoryCacheService = memoryCacheService;
            _cacheConfigFactory = cacheConfigFactory;
        }
        public string CacheKey { get; set; }
        public void AfterInvoke(InvocationContext invocationContext, object methodResult, Exception ex)
        {
            if (ex == null)
                StoreInCache(methodResult, invocationContext);
            else
                invocationContext.TempData = CreateCacheExceptionInfoMessage(CacheKey, ex);
            //var returnType = invocationContext.GetMethodReturnType();
            //var isAsyncMethod = returnType != null && returnType.IsGenericType &&
            //           returnType.GetGenericTypeDefinition() == typeof(Task<>);
            //if (isAsyncMethod)
            //{
            //    //InterceptAsync((dynamic)invocationContext.Invocation.ReturnValue, invocationContext);
            //}
            //else
            //{
            //    StoreInCache(methodResult, invocationContext);
            //}
        }

        public void BeforeInvoke(InvocationContext invocationContext)
        {
            _memoryCacheAttribute = invocationContext.GetAttributeFromMethod<MemoryCacheAttribute>();
            var cacheKey = _memoryCacheAttribute.CacheKey;
            var cacheConfig = _cacheConfigFactory.Config[cacheKey];
            if (cacheConfig.Enabled)
            {
                if (!string.IsNullOrWhiteSpace(_memoryCacheAttribute.CacheKeyParameterSubstitutions))
                {
                    List<object> paramValues = new List<object>();
                    string[] paramNames = _memoryCacheAttribute.CacheKeyParameterSubstitutions.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    foreach (string paramName in paramNames)
                    {
                        paramValues.Add(invocationContext.GetParameterValue(paramName.Trim()));

                    }
                    cacheKey = string.Format(cacheConfig.CacheKey.ToString(), paramValues.ToArray());

                }
                CacheKey = cacheKey;

                if (_memoryCacheAttribute.RefreshCache)
                {
                    _memoryCacheService.Remove(CacheKey);
                }

                var cacheReturnObj = RetrieveCacheEntry(invocationContext, CacheKey);
                if (cacheReturnObj != null && cacheReturnObj.CacheEntry != null)
                {
                    var returnType = invocationContext.GetMethodReturnType();
                    var isAsyncMethod = returnType != null && returnType.IsGenericType &&
                        returnType.GetGenericTypeDefinition() == typeof(Task<>);
                    if (isAsyncMethod)
                    {
                        invocationContext.OverrideAsyncMethodReturnValue(cacheReturnObj.CacheEntry);
                    }
                    else
                    {
                        invocationContext.OverrideMethodReturnValue(cacheReturnObj.CacheEntry);
                    }

                    invocationContext.TempData[CacheConstants.LOGGING_CACHE_INVOCATIONCONETXT] = string.Format(
                        "{0}{1}{2}", CacheConstants.LOGGING_CACHE_HIT, CacheConstants.LOGGING_CACHE_DELIMITER, cacheKey);
                    invocationContext.BypassInvocation();
                }
                else
                {
                    invocationContext.TempData[CacheConstants.LOGGING_CACHE_INVOCATIONCONETXT] = string.Format(
                           "{0}{1}{2}", CacheConstants.LOGGING_CACHE_MISS, CacheConstants.LOGGING_CACHE_DELIMITER, cacheKey);
                }

            }
        }

        private CacheReturnModel RetrieveCacheEntry(InvocationContext invocationContext, string key)
        {
            var cacheReturnObj = new CacheReturnModel();
            try
            {
                var returnType = invocationContext.GetMethodReturnType();
                cacheReturnObj.CacheEntry = IsReturnTypeGenericCollection(returnType) ?
                    RetrieveCollectionFromCache(key, returnType) :
                    _memoryCacheService.Get(key);
            }
            catch (Exception ex)
            {
                cacheReturnObj.CacheContext = CreateCacheExceptionInfoMessage(key, ex);
            }
            return cacheReturnObj;
        }

        private object RetrieveCollectionFromCache(string key, Type returnType)
        {
            var genericType = returnType.GetGenericArguments()[0];
            return _memoryCacheService.GetCollection(key, returnType);
        }
        private bool IsReturnTypeGenericCollection(Type type)
        {
            return type.GetInterfaces().Any(i => i.IsGenericType &&
            i.GetGenericTypeDefinition() == typeof(ICollection));

        }

        private T InterceptAsync<T>(Task<T> task, InvocationContext invocationContext)
        {


            T result = default(T);
            Task<T> callTask = Task.Run(() => task);
            //callTask.Wait(120000);
            result = callTask.Result;
            return result;
        }

        private void StoreInCache(object result, InvocationContext invocationContext)
        {
            if (result != null &&
                (!IsReturnTypeGenericCollection(result.GetType()) || ((IList)result).Count > 0))
            {
                var cacheResturnobj = new CacheReturnModel();
                cacheResturnobj.CacheEntry = result;
                try
                {
                    var obj = (ICacheable)result;
                    _memoryCacheService.Put(CacheKey, obj);
                }
                catch (Exception ex)
                {
                    invocationContext.TempData = CreateCacheExceptionInfoMessage(CacheKey, ex);
                }
            }
        }

        private ConcurrentDictionary<string, object> CreateCacheExceptionInfoMessage(object cacheKey, Exception ex)
        {
            var methodContext = new ConcurrentDictionary<string, object>();
            methodContext[CacheConstants.LOGGING_CACHE_INVOCATIONCONETXT_EXCEPTION] = ex;
            methodContext[CacheConstants.LOGGING_CACHE_INVOCATIONCONETXT] = string.Format(
                "{0}{1}{2}", CacheConstants.LOGGING_CACHE_FAILED, CacheConstants.LOGGING_CACHE_DELIMITER, cacheKey);
            return methodContext;
        }
    }
}

namespace DotNetCore.Framework.Interception.Internal
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Castle.DynamicProxy;
    using Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using DotNetCore.Framework.Interception.Configuration;
    using DotNetCore.Framework.Interception;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    /// <summary>
    /// The Master Interceptor Class Wraps a proxified object and handles all of its interceptions
    /// </summary>
    internal class CoreInterceptor : IAsyncInterceptor
    {
        /// <summary>
        /// Gets the <see cref="proxyConfiguration"/>
        /// </summary>
        private readonly SimpleProxyConfiguration proxyConfiguration;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/>
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="CoreInterceptor"/> class
        /// </summary>
        /// <param name="serviceProvider">Service Provider</param>
        /// <param name="proxyConfiguration">Proxy Configuration</param>
        public CoreInterceptor(IServiceProvider serviceProvider, SimpleProxyConfiguration proxyConfiguration)
        {
            this.serviceProvider = serviceProvider;
            this.proxyConfiguration = proxyConfiguration;
        }

        /// <summary>
        /// Intercepts a synchronous method <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method Invocation.</param>
        public void InterceptSynchronous(IInvocation invocation)
        {
            // Map the configured interceptors to this type based on its attributes
            var invocationMetaDataCollection = InvocationExtensions.GetInterceptorMetadataForMethod(invocation, this.serviceProvider, this.proxyConfiguration);

            //If there are no configured interceptors leave now
            if (invocationMetaDataCollection == null || !invocationMetaDataCollection.Any())
            {
                invocation.Proceed();
                return;
            }

            // Get the Ordering Strategy for Interceptors
            var orderingStrategy = this.proxyConfiguration.OrderingStrategy;

            // Process the BEFORE Interceptions
            foreach (var invocationContext in orderingStrategy.OrderBeforeInterception(invocationMetaDataCollection))
            {
                invocationContext.Interceptor.BeforeInvoke(invocationContext);
            }

            Exception exToThrow = null;
            // Execute the Real method
            if (!invocationMetaDataCollection.Any(p => p.InvocationIsBypassed))
            {
                try
                {
                    invocation.Proceed();
                    exToThrow = null;
                }
                catch (Exception ex)
                {
                    exToThrow = ex;
                }
            }
            // Process the AFTER Interceptions
            ConcurrentDictionary<string, object> contextTempData = null;
            foreach (var invocationContext in orderingStrategy.OrderAfterInterception(invocationMetaDataCollection))
            {
                //Copy stores data which can be passed between interceptors
                if (contextTempData != null && contextTempData.Count > 0)
                    invocationContext.CopyTempData(contextTempData);
                if (!invocationContext.InvocationIsBypassed)
                {
                    invocationContext.Interceptor.AfterInvoke(invocationContext, invocationContext.GetMethodReturnValue(), exToThrow);
                    contextTempData = invocationContext.TempData;
                    exToThrow = invocationContext.MethodException;
                }
            }

            if (exToThrow != null)
                throw exToThrow;
        }

        /// <summary>
        /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task"/>.
        /// </summary>
        /// <param name="invocation">The method Invocation.</param>
        public void InterceptAsynchronous(IInvocation invocation)
        {
            // Map the configured interceptors to this type based on its attributes
            var invocationMetaDataCollection = InvocationExtensions.GetInterceptorMetadataForMethod(invocation, this.serviceProvider, this.proxyConfiguration);

            //If there are no configured interceptors leave now
            if (invocationMetaDataCollection == null || !invocationMetaDataCollection.Any())
            {
                invocation.Proceed();
                return;
            }
            invocation.ReturnValue = InterceptAsynchronous(invocation, invocationMetaDataCollection);
        }

        /// <summary>
        /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task{T}"/>.
        /// </summary>
        /// <typeparam name="TResult">The Type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
        /// <param name="invocation"> The method invocation.</param>
        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            // Map the configured interceptors to this type based on its attributes
            var invocationMetaDataCollection = InvocationExtensions.GetInterceptorMetadataForMethod(invocation, this.serviceProvider, this.proxyConfiguration);

            // If there are no configured interceptors leave now
            if (invocationMetaDataCollection == null || !invocationMetaDataCollection.Any())
            {
                invocation.Proceed();
                return;
            }
            invocation.ReturnValue = InterceptAsynchronous<TResult>(invocation, invocationMetaDataCollection);
        }

        private async Task InterceptAsynchronous(IInvocation invocation, List<InvocationContext> invocationMetaDataCollection)
        {
            // Get the ordering Strategy for Interceptors
            var orderingStrategy = this.proxyConfiguration.OrderingStrategy;

            // Process the BEFORE Interceptions
            foreach (var invocationContext in orderingStrategy.OrderBeforeInterception(invocationMetaDataCollection))
            {
                invocationContext.Interceptor.BeforeInvoke(invocationContext);
            }

            Exception exToThrow = null;
            // Execute the Real method
            if (!invocationMetaDataCollection.Any(p => p.InvocationIsBypassed))
            {
                try
                {
                    invocation.Proceed();
                    var task = (Task)invocation.ReturnValue;
                    await task.ConfigureAwait(false);
                    exToThrow = null;
                }
                catch (Exception ex)
                {
                    exToThrow = ex;
                }
            }
            // Process the AFTER Interceptions
            ConcurrentDictionary<string, object> contextTempData = null;
            foreach (var invocationContext in orderingStrategy.OrderAfterInterception(invocationMetaDataCollection))
            {
                //Copy stores data which can be passed between interceptors
                if (contextTempData != null && contextTempData.Count > 0)
                    invocationContext.CopyTempData(contextTempData);
                if (!invocationContext.InvocationIsBypassed)
                {
                    invocationContext.Interceptor.AfterInvoke(invocationContext, invocationContext.GetMethodReturnValue(), exToThrow);
                    contextTempData = invocationContext.TempData;
                    exToThrow = invocationContext.MethodException;
                }
            }

            if (exToThrow != null)
                throw exToThrow;
        }
     
        private async Task<TResult> InterceptAsynchronous<TResult>(IInvocation invocation, List<InvocationContext> invocationMetaDataCollection)
        {
            TResult result = default(TResult);

            // Get the ordering Strategy for Interceptors
            var orderingStrategy = this.proxyConfiguration.OrderingStrategy;

            // Process the BEFORE Interceptions
            foreach (var invocationContext in orderingStrategy.OrderBeforeInterception(invocationMetaDataCollection))
            {
                invocationContext.Interceptor.BeforeInvoke(invocationContext);
            }

            Exception exToThrow = null;
            // Execute the Real method
            if (!invocationMetaDataCollection.Any(p => p.InvocationIsBypassed))
            {
                try
                {
                    invocation.Proceed();
                    var task = (Task<TResult>)invocation.ReturnValue;
                    result = await task.ConfigureAwait(false);
                    exToThrow = null;
                }
                catch (Exception ex)
                {
                    exToThrow = ex;
                }
            }
            else
            {
                result= await (Task<dynamic>)invocation.ReturnValue;
            
            }
            // Process the AFTER Interceptions
            ConcurrentDictionary<string, object> contextTempData = null;
            foreach (var invocationContext in orderingStrategy.OrderAfterInterception(invocationMetaDataCollection))
            {
                //Copy stores data which can be passed between interceptors
                contextTempData = invocationContext.TempData;
                if (contextTempData != null && contextTempData.Count > 0)
                    invocationContext.CopyTempData(contextTempData);
                if (!invocationContext.InvocationIsBypassed)
                {
                    try
                    {
                        //invocationContext.Interceptor.AfterInvoke(invocationContext, invocationContext.GetMethodReturnValue(), exToThrow);
                        invocationContext.Interceptor.AfterInvoke(invocationContext, result, exToThrow);
                        contextTempData = invocationContext.TempData;
                        exToThrow = invocationContext.MethodException;
                    }
                    catch (Exception ex)
                    {
                        contextTempData = invocationContext.TempData;
                        exToThrow = invocationContext.MethodException =
                            ex.InnerException != null ? ex.InnerException : ex;
                    }
                }
            }


            if (exToThrow != null)
                throw exToThrow;
            return result;
        }


    }
}
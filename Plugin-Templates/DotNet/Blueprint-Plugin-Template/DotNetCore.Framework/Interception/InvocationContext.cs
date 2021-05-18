namespace DotNetCore.Framework.Interception
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Attributes;
    using Castle.DynamicProxy;
    using Interfaces;

    /// <Summary>
    /// Metadata Class that describes the Current Invocation
    /// </Summary>
    public class InvocationContext : IHideBaseTypes
    {
        #region Internal Properties

        /// <Summary>
        /// Gets or Sets the Invocation Context
        /// </Summary>
        public IInvocation Invocation { get; set; }

        /// <Summary>
        /// Gets or Sets the Attribute that triggered the interceptor
        /// </Summary>
        public MethodInterceptionAttribute Attribute { get; set; }

        /// <Summary>
        /// Gets or Sets the Attribute that triggered for this method
        /// </Summary>
        public IMethodInterceptor Interceptor { get; set; }

        /// <Summary>
        /// Stores data which can be passed between interceptors 
        /// </Summary>
        public ConcurrentDictionary<string, object> TempData { get; set; }

        /// <Summary>
        /// Gets or Sets the <see cref="IServiceProvider"/>
        /// </Summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <Summary>
        /// Gets or Sets the Order Priority of this Invocation
        /// </Summary>
        public int Order { get; set; }

        /// <Summary>
        /// Gets or Sets a value that indicates whether invocation of the underlying method is bypassed
        /// </Summary>
        public bool InvocationIsBypassed { get; set; }

        /// <Summary>
        /// Gets or Sets the Exception occured in called method
        /// </Summary>
        public Exception MethodException { get; set; }
        #endregion

        /// <Summary>
        /// Initialises a new instance of the <see cref="InvocationContext"/>
        /// </Summary>
        public InvocationContext()
        {
            this.TempData = new ConcurrentDictionary<string, object>();
        }

        /// <Summary>
        /// Gets the attribute that initiated the interception
        /// </Summary>
        /// <returns><see cref="MethodInterceptionAttribute"/></returns>
        public MethodInterceptionAttribute GetOwningAttribute()
        {
            return this.Attribute;
        }

        /// <Summary>
        /// Gets the type that owns the executing method
        /// </Summary>
        /// <returns><see cref="Type"/></returns>
        public Type GetOwningType()
        {
            return this.Invocation.Method.DeclaringType;
        }

        /// <Summary>
        /// Gets the Service Provider for resolving dependencies
        /// </Summary>
        /// <returns><see cref="IServiceProvider"/></returns>
        public IServiceProvider GetServiceProvider()
        {
            return this.ServiceProvider;
        }

        /// <Summary>
        /// Gets the Executing Method Full Name
        /// </Summary>
        /// <returns>The Name of the executing method with Namespace</returns>
        public string GetExecutingMethodFullName()
        {
            return this.Invocation.InvocationTarget.ToString() + " " + this.Invocation.Method.Name;
        }

        /// <Summary>
        /// Gets the Position Of the specified Type in the in the methods parameter List
        /// </Summary>
        /// <typeparam name = "T"> Type of value to get </typeparam>
        /// <param name="parameterPosition">Parameter Position</param>
        /// <returns> Returns the value of the parameter at the given position at {T}</returns>
        public T GetParameterValue<T>(int parameterPosition)
        {
            return (T)this.Invocation.GetArgumentValue(parameterPosition);
        }

        public object GetParameterValue(string parameterName)
        {
            var param = Invocation.GetConcreteMethod().GetParameters().First(p => p.Name == parameterName);
            return this.Invocation.GetArgumentValue(param.Position);
        }

        public void CopyTempData(ConcurrentDictionary<string, object> source)
        {
            foreach (var item in source)
            {
                if (this.TempData == null)
                    this.TempData = new ConcurrentDictionary<string, object>();
                this.TempData.TryAdd(item.Key, item.Value);
            }
        }

        /// <Summary>
        /// Gets the Position Of the specified Type in the in the methods parameter List
        /// </Summary>
        /// <param name="parameterPosition">Parameter Position</param>
        /// <returns> Returns the value of the parameter at the given position</returns>
        public object GetParameterValue(int parameterPosition)
        {
            return this.Invocation.GetArgumentValue(parameterPosition);
        }

        /// <Summary>
        /// Sets the Value Of the parameter at the specified Location
        /// </Summary>
        /// <param name="parameterPosition">Parameter Position</param>
        /// <param name="newValue">New Value</param>
        public void SetParameterValue(int parameterPosition, object newValue)
        {
            this.Invocation.SetArgumentValue(parameterPosition, newValue);
        }

        /// <Summary>
        /// Adds temporary data to the context
        /// </Summary>
        /// <param name="name">Name to identify the data</param>
        /// <param name="value">Data Value</param>
        public void SetTemporaryData(string name, object value)
        {
            this.TempData.TryAdd(name, value);
        }

        /// <Summary>
        /// Gets temporary data from the context
        /// </Summary>
        /// <param name="name">Name to identify the data</param>
        /// <returns></returns>
        public object GetTemporaryData(string name)
        {
            return this.TempData.TryGetValue(name, out var value) ? value : default(object);
            // return this.TempData.GetValueOrDefault(name);
        }

        /// <Summary>
        /// Gets the return value from the method that was called
        /// </Summary>
        /// <returns>Method return Value</returns>
        public object GetMethodReturnValue()
        {
            return this.Invocation.ReturnValue;
        }

        /// <Summary>
        /// Overrides the return value for the method being called . Usually called with [BypassInvocation] to shortcut interception
        /// </Summary>
        /// <returns>Method return Value</returns>
        public void OverrideMethodReturnValue(object returnValue)
        {
            this.Invocation.ReturnValue = returnValue;
        }

        /// <Summary>
        /// Overrides the return value for the method being called . Usually called with [BypassInvocation] to shortcut interception
        /// </Summary>
        /// <returns>Method return Value</returns>
        public void OverrideAsyncMethodReturnValue(object returnValue)
        {
            this.Invocation.ReturnValue = Task.FromResult(returnValue);
        }

        /// <Summary>
        /// Sets the Invocation of the underlying method to be bypassed
        /// </Summary>
        public void BypassInvocation()
        {
            this.InvocationIsBypassed = true;
        }

        /// <Summary>
        /// Gets the executing Method Info
        /// </Summary>
        /// <returns>Returns the Position of the type in the method parameters. Returns -1 if not found.</returns>
        public MethodInfo GetExecutingMethodInfo()
        {
            return this.Invocation.Method;
        }

        /// <Summary>
        /// Gets the executing Method Name
        /// </Summary>
        /// <returns>The Name of the executing method.</returns>
        public string GetExecutingMethodName()
        {
            return this.Invocation.Method.Name;
        }

        public Type GetMethodReturnType()
        {
            return this.Invocation.Method.ReturnType;
        }

        /// <Summary>
        /// Method to try and identify the position of the specified type in the methods parameter List
        /// </Summary>
        /// <param name="typeToFind">Type To Find</param>
        /// <returns>Returns the position of the type in the method parameters, Returns -1 if not found</returns>
        public int GetParameterPosition(Type typeToFind)
        {
            var method = this.Invocation.Method;
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            for (var i = method.GetParameters().Length - 1; i >= 0; i--)
            {
                var paramType = method.GetParameters()[i].ParameterType;
                if (paramType != typeToFind)
                {
                    continue;
                }
                return i;
            }
            return -1;
        }

        /// <Summary>
        /// Gets the Position Of the specified Type in the in the methods parameter List
        /// </Summary>
        /// <typeparam name = "TTypeToFind"> Type to find </typeparam>
        /// <returns> Returns the position of the type in the method parameters,  Returns -1 if not found </returns>
        public int GetParameterPosition<TTypeToFind>()
        {
            return this.GetParameterPosition(typeof(TTypeToFind));
        }

        /// <Summary>
        /// Gets the specified attribute from the executing method
        /// </Summary>
        public TAttribute GetAttributeFromMethod<TAttribute>() where TAttribute : Attribute
        {
            return this.Invocation.MethodInvocationTarget.GetCustomAttributes().OfType<TAttribute>().FirstOrDefault();
        }
    }
}
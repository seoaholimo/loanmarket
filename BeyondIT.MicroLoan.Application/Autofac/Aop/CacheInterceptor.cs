using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using BeyondIT.MicroLoan.Infrastructure.Extensions;
using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;


namespace BeyondIT.MicroLoan.Application.Autofac.Aop
{
    public class CacheInterceptor : IInterceptor
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;

        public CacheInterceptor(IMemoryCache memoryCache, IMapper mapper)
        {
            _memoryCache = memoryCache;
            _mapper = mapper;
        }

        public void Intercept(IInvocation invocation)
        {
/*            //Skip void methods
            if (invocation.Method.ReturnType == typeof(void) || invocation.Method.ReturnType == typeof(Task))
            {
                invocation.Proceed();
                return;
            }

            var resultCacheSettingAttribute = invocation.Method.GetCustomAttribute<ResultCacheSettingAttribute>();

            //Skip method set to disallow cache
            if (resultCacheSettingAttribute != null && !resultCacheSettingAttribute.AllowCache)
            {
                invocation.Proceed();
                return;
            }

            Type dtoType;
            if (resultCacheSettingAttribute != null && resultCacheSettingAttribute.SerializerDtoType != null)
            {
                dtoType = invocation.Method.ReturnType.IsCollectionType() && !resultCacheSettingAttribute.SerializerDtoType.IsCollectionType()
                    ? typeof(List<>).MakeGenericType(resultCacheSettingAttribute.SerializerDtoType)
                    : resultCacheSettingAttribute.SerializerDtoType;
            }
            else
            {
                dtoType = invocation.Method.ReturnType;
            }

            string key = GenerateCacheKey(invocation);
            string jsonData = _memoryCache.Get(key)?.ToString();

            if (jsonData != null)
            {
                object dtoObject = JsonConvert.DeserializeObject(jsonData, dtoType);
                if (invocation.Method.ReturnType.IsGenericType &&
                    invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    Type genericTypeArgument = invocation.Method.ReturnType.GenericTypeArguments[0];
                    object deserializedObject = GetObject(dtoObject, genericTypeArgument);

                    Type taskCompletionSourceType = typeof(TaskCompletionSource<>).MakeGenericType(genericTypeArgument);
                    dynamic taskSource = Activator.CreateInstance(taskCompletionSourceType);
                    dynamic result = Convert.ChangeType(deserializedObject, genericTypeArgument);
                    taskSource.SetResult(result);

                    invocation.ReturnValue = taskSource.Task;
                }
                else
                {
                    invocation.ReturnValue = GetObject(dtoObject, invocation.Method.ReturnType);
                }
                return;
            }

            invocation.Proceed();

            if (invocation.ReturnValue != null)
            {
                object returnValue;
                if (invocation.ReturnValue.GetType().IsGenericType &&
                    invocation.ReturnValue.GetType().GetGenericTypeDefinition() == typeof(Task<>))
                {
                    returnValue = ((dynamic)invocation.ReturnValue).Result;
                    if (returnValue == null) return;
                }
                else
                {
                    returnValue = invocation.ReturnValue;
                }

                object dtoObject = GetObject(returnValue, dtoType);
                string jsonString = JsonConvert.SerializeObject(dtoObject);

                _memoryCache.Set(key, jsonString,
                    new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(
                            TimeSpan.FromMinutes(resultCacheSettingAttribute?.GetExpiryTimeInMinutes() ??
                                                 ResultCacheSettingAttribute.GetDefaultExpiryTimeInMinutes())));
            }*/
            
            invocation.Proceed(); //todo: Will review caching failing
        }

        private object GetObject(object sourceObject, Type targetType)
        {
            if (targetType.IsCollectionType())
            {
                Type genericTypeArgument = targetType.GenericTypeArguments[0];
                var sourceObjectList = (ICollection)sourceObject;
                var destinationObjectList = (IList)Activator.CreateInstance(targetType);

                foreach (object sourceObjectListItem in sourceObjectList)
                {
                    object targetObject = Activator.CreateInstance(genericTypeArgument);
                    if (sourceObjectListItem.GetType() == targetObject.GetType())
                    {
                        string serializedData = JsonConvert.SerializeObject(sourceObjectListItem);
                        targetObject = JsonConvert.DeserializeObject(serializedData, targetObject.GetType());
                    }
                    else
                    {
                        _mapper.Map(sourceObjectListItem, targetObject);
                    }

                    destinationObjectList.Add(targetObject);
                }

                return destinationObjectList;
            }

            object destinationObject = Activator.CreateInstance(targetType);
            _mapper.Map(sourceObject, destinationObject);


            return destinationObject;
        }

        private static string GenerateCacheKey(IInvocation invocation)
        {
            int? hashedArguments = JsonConvert.SerializeObject(invocation.Arguments)?.GetHashCode();

            return
                $"{invocation.TargetType.FullName}.{invocation.Method.Name}ArgumentsHashCode:{hashedArguments}";
        }
    }
}
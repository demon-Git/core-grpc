﻿using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Overt.Core.Grpc.H2
{
    /// <summary>
    /// GrpcClient管理类
    /// </summary>
    public class GrpcClient<T> : IGrpcClient<T> where T : ClientBase
    {
        readonly IGrpcClientFactory<T> _factory;
        readonly ConcurrentDictionary<Type, T> _clientCache = new ConcurrentDictionary<Type, T>();
        public GrpcClient(IGrpcClientFactory<T> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// 获取
        /// </summary>
        public T Client
        {
            get
            {
                return _clientCache.GetOrAdd(typeof(T), key => _factory.Get());
            }
        }

        /// <summary>
        /// 构造一个新的对象
        /// </summary>
        /// <param name="getInvoker"></param>
        /// <returns></returns>
        public T CreateClient(Func<List<ChannelWrapper>, ChannelWrapper> getChannel)
        {
            return _factory.Get(getInvoker: getInvoker);
        }
    }
}
﻿using Grpc.Core;
using System;
using System.Collections.Generic;

namespace Overt.Core.Grpc.H2
{
    /// <summary>
    /// 工厂类接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGrpcClientFactory<T> where T : ClientBase
    {
#if NET5_0_OR_GREATER
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T Get();
#else
        /// <summary>
        /// 获取Client对象
        /// </summary>
        /// <returns></returns>
        T Get(Func<List<ChannelWrapper>, ChannelWrapper> channelWrapperInvoker = null);
#endif

    }
}

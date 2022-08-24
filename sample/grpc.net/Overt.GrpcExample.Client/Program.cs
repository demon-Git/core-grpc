﻿using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Overt.Core.Grpc.H2;
using System;
using System.Threading;
using static Overt.GrpcExample.Service.Grpc.GrpcExampleService;

namespace Overt.GrpcExample.Client
{
    class Program
    {
        static IServiceProvider provider;
        static Program()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", true);

            var configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddOptions();

            // 注入GrpcClient
            services.AddGrpcClient();

            // 第三方配置，启动可用
            //services.AddGrpcConfig(config =>
            //{
            //    config.AddApollo(configuration.GetSection("apollo")).AddDefault();
            //});

            // 单服务配置
            services.Configure<GrpcClientOptions<GrpcExampleServiceClient>>(cfg =>
            {
                //var httpClientHandler = new HttpClientHandler
                //{
                //    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                //};
                cfg.GrpcChannelOptions = new GrpcChannelOptions()
                {
                    //HttpClient = new HttpClient(httpClientHandler),
                    ServiceProvider = services.BuildServiceProvider(),
                    Credentials = Grpc.Core.ChannelCredentials.Insecure,
                    //HttpHandler = new SocketsHttpHandler()
                    //{
                    //    EnableMultipleHttp2Connections = true,
                    //}
                };
            });

            // 如果需要自行配置Resolve
            // 设置一个自定义Scheme，然后自定义实现Resove即可，address.LocalPath = /{ServiceName}
            // 如果需要自行配置LoadBalance
            // 自定义实现LoadBalance，传入ServiceConfig即可

            // 单服务配置
            //services.Configure<GrpcClientOptions<GrpcExampleService1Client>>(cfg =>
            //{
            //    cfg.ConfigPath = "";
            //    cfg.Scheme = "http";
            //});

            provider = services.BuildServiceProvider();
        }


        static void Main(string[] args)
        {
            do
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.A)
                    break;


                try
                {
                    var service = provider.GetService<IGrpcClient<GrpcExampleServiceClient>>();
                    var res = service.Client.Ask(new Service.Grpc.AskRequest() { Key = "abc" });
                    Console.WriteLine(DateTime.Now + " - " + res.Content ?? "abc");

                    //var service1 = provider.GetService<IGrpcClient<GrpcExampleService1Client>>();
                    //var res1 = service1.Client.Ask(new Service.Grpc.AskRequest() { Key = "abc" });
                    //Console.WriteLine(DateTime.Now + " - " + res1.Content ?? "abc");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{DateTime.Now} - 异常");
                }

                Thread.Sleep(200);

            } while (true);

            Console.WriteLine("over");
            Console.ReadLine();
        }
    }
}

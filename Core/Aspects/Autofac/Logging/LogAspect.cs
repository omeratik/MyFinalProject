using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging;
using Core.Entities.Concrete;
using Core.Utilities.Interceptors;  // Bu namespace altında MethodInterception tanımlı
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Autofac;
using System.Collections.Generic;
using Core.Entities.Abstract;
using System.Text.Json;

namespace Core.Aspects.Autofac.Logging
{
    public class LogAspect : MethodInterception
    {
        private readonly Type _logServiceType;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogAspect(Type logServiceType)
        {
            if (logServiceType.GetInterfaces().All(i => i != typeof(ILogService)))
            {
                throw new Exception($"Wrong log service type. {logServiceType.Name} must implement ILogService");
            }

            _logServiceType = logServiceType;
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

            Console.WriteLine("LogAspect created successfully");
        }

        protected override void OnBefore(IInvocation invocation)
        {
            try
            {
                Console.WriteLine("OnBefore method started");

                // Doğrudan ILogService'i almayı deneyelim
                var logService = ServiceTool.GetService<ILogService>();

                if (logService != null)
                {
                    Console.WriteLine("LogService retrieved successfully");
                    var logDetail = GetLogDetail(invocation);
                    logService.Log(logDetail);
                    Console.WriteLine("Log operation completed");
                }
                else
                {
                    Console.WriteLine($"LogService is null! Type: {_logServiceType.Name}");

                    try
                    {
                        var container = ServiceTool.GetService<IContainer>();
                        Console.WriteLine($"Container status: {(container != null ? "Found" : "Null")}");

                        if (container != null)
                        {
                            foreach (var registration in container.ComponentRegistry.Registrations)
                            {
                                foreach (var service in registration.Services)
                                {
                                    Console.WriteLine($"Registered service: {service.Description}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error while checking registrations: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logging error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private LogDetail GetLogDetail(IInvocation invocation)
        {
            string userName = "System";
            try
            {
                userName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "Anonymous";
            }
            catch
            {
                // HttpContext alınamazsa varsayılan değeri kullan
            }

            var parameters = new List<string>();

            for (int i = 0; i < invocation.Arguments.Length; i++)
            {
                var argument = invocation.Arguments[i];
                var parameterInfo = invocation.Method.GetParameters()[i];

                if (argument != null)
                {
                    try
                    {
                        // JSON formatında loglama
                        var jsonString = JsonSerializer.Serialize(argument, new JsonSerializerOptions
                        {
                            WriteIndented = false,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                        parameters.Add($"{parameterInfo.Name}: {jsonString}");
                    }
                    catch (Exception ex)
                    {
                        // JSON serileştirme başarısız olursa düz metin olarak logla
                        parameters.Add($"{parameterInfo.Name}: (Serileştirme Hatası) {ex.Message}");
                    }
                }
                else
                {
                    parameters.Add($"{parameterInfo.Name}: null");
                }
            }

            return new LogDetail
            {
                MethodName = $"{invocation.Method.DeclaringType?.FullName}.{invocation.Method.Name}",
                User = userName,
                Parameters = string.Join(" | ", parameters),
                Date = DateTime.Now
            };
        }
    }
}

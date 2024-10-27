using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyExchangeClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7238/currencyHub", options =>
                {
                    options.HttpMessageHandlerFactory = _ => handler;
                })
                .Build();

            connection.On<double, double>("UpdateCurrencyRate", (usdToEur, gbpToEur) =>
            {
                Console.WriteLine($"Новый курс USD/EUR: {usdToEur:F2}, GBP/EUR: {gbpToEur:F2}");
            });

            try
            {
                await connection.StartAsync();
                Console.WriteLine("Подключено к серверу.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения: {ex.Message}");
            }

            Console.ReadLine();
        }
    }
}

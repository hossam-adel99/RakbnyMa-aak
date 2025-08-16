using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Services.Interfaces;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.Services.Implementations
{
    public class PaymentGatewayService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public PaymentGatewayService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<PaymentResult> ProcessCardPayment(decimal amount, string cardToken)
        {
            try
            {
                var apiKey = _config["PaymentGateway:ApiKey"];
                var endpoint = _config["PaymentGateway:CardPaymentEndpoint"];

                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var request = new
                {
                    amount = amount * 100, // Convert to cents
                    currency = "EGP",
                    token = cardToken,
                    description = "RakbnyMa3ak Booking Payment"
                };

                var response = await _httpClient.PostAsJsonAsync(endpoint, request);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new PaymentResult(
                        PaymentStatus.Failed,
                        FailureReason: $"HTTP {response.StatusCode}",
                        Message: $"فشل الدفع: {content}");
                }

                using var doc = JsonDocument.Parse(content);
                var root = doc.RootElement;

                return new PaymentResult(
                    root.GetProperty("success").GetBoolean()
                        ? PaymentStatus.Completed
                        : PaymentStatus.Failed,
                    TransactionId: root.GetProperty("id").GetString(),
                    Message: root.GetProperty("message").GetString());
            }
            catch (Exception ex)
            {
                return new PaymentResult(
                    PaymentStatus.Failed,
                    FailureReason: "خطأ في بوابة الدفع",
                    Message: $"خطأ في معالجة الدفع: {ex.Message}");
            }
        }

        public async Task<PaymentResult> ProcessVodafoneCashPayment(decimal amount, string phoneNumber)
        {
            try
            {
                var apiKey = _config["PaymentGateway:ApiKey"];
                var endpoint = _config["PaymentGateway:VodafoneEndpoint"];

                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var request = new
                {
                    amount,
                    currency = "EGP",
                    phone = phoneNumber,
                    reference = $"RB_{DateTime.UtcNow.Ticks}"
                };

                var response = await _httpClient.PostAsJsonAsync(endpoint, request);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new PaymentResult(
                        PaymentStatus.Failed,
                        FailureReason: $"HTTP {response.StatusCode}",
                        Message: $"فشل الدفع: {content}");
                }

                using var doc = JsonDocument.Parse(content);
                var root = doc.RootElement;

                return new PaymentResult(
                    root.GetProperty("success").GetBoolean()
                        ? PaymentStatus.Completed
                        : PaymentStatus.Failed,
                    TransactionId: root.GetProperty("transaction_id").GetString(),
                    Message: root.GetProperty("message").GetString());
            }
            catch (Exception ex)
            {
                return new PaymentResult(
                    PaymentStatus.Failed,
                    FailureReason: "خطأ في بوابة الدفع",
                    Message: $"خطأ في معالجة الدفع: {ex.Message}");
            }
        }
    }
}

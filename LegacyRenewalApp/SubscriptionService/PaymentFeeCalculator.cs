using System;

namespace LegacyRenewalApp.SubscriptionService;

public class PaymentFeeCalculator
{
    public (decimal fee, string notes) Calculate(string paymentMethod, decimal subtotal)
    {
        string normalized = paymentMethod.Trim().ToUpperInvariant();

        return normalized switch
        {
            "CARD" => (subtotal * 0.02m, "card payment fee; "),
            "BANK_TRANSFER" => (subtotal * 0.01m, "bank transfer fee; "),
            "PAYPAL" => (subtotal * 0.035m, "paypal fee; "),
            "INVOICE" => (0m, "invoice payment; "),
            _ => throw new ArgumentException("Unsupported payment method")
        };
    }
}
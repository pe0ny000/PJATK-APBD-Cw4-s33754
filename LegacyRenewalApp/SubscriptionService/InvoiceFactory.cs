using System;

namespace LegacyRenewalApp.SubscriptionService;

public class InvoiceFactory
{
    public RenewalInvoice Create(Customer customer, InvoiceData data)
    {
        return new RenewalInvoice
        {
            InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{customer.Id}-{data.PlanCode}",
            CustomerName = customer.FullName,
            PlanCode = data.PlanCode,
            PaymentMethod = data.PaymentMethod,
            SeatCount = data.SeatCount,
            BaseAmount = Math.Round(data.BaseAmount, 2, MidpointRounding.AwayFromZero),
            DiscountAmount = Math.Round(data.DiscountAmount, 2, MidpointRounding.AwayFromZero),
            SupportFee = Math.Round(data.SupportFee, 2, MidpointRounding.AwayFromZero),
            PaymentFee = Math.Round(data.PaymentFee, 2, MidpointRounding.AwayFromZero),
            TaxAmount = Math.Round(data.TaxAmount, 2, MidpointRounding.AwayFromZero),
            FinalAmount = Math.Round(data.FinalAmount, 2, MidpointRounding.AwayFromZero),
            Notes = data.Notes.Trim(),
            GeneratedAt = DateTime.UtcNow
        };
    }
}
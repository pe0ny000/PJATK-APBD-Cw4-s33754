using System;
using LegacyRenewalApp.SubscriptionService;

namespace LegacyRenewalApp;

public class SubscriptionRenewalService
{
    public RenewalInvoice CreateRenewalInvoice(
        int customerId,
        string planCode,
        int seatCount,
        string paymentMethod,
        bool includePremiumSupport,
        bool useLoyaltyPoints)
    {
        new InvoiceValidator().DataValidate(customerId, planCode, seatCount, paymentMethod);

        string normalizedPlanCode = planCode.Trim().ToUpperInvariant();
        string normalizedPaymentMethod = paymentMethod.Trim().ToUpperInvariant();

        var customer = new CustomerRepository().GetById(customerId);
        var plan = new SubscriptionPlanRepository().GetByCode(normalizedPlanCode);

        if (!customer.IsActive)
            throw new InvalidOperationException("Inactive customers cannot renew subscriptions");

        decimal baseAmount = (plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee;

        var (discountAmount, notes) = new DiscountCalculator()
            .Calculate(customer, plan, seatCount, useLoyaltyPoints);

        decimal subtotal = baseAmount - discountAmount;
        if (subtotal < 300m) subtotal = 300m;

        var (supportFee, supportNotes) = new SupportFeeCalculator()
            .Calculate(normalizedPlanCode, includePremiumSupport);

        var (paymentFee, paymentNotes) = new PaymentFeeCalculator()
            .Calculate(normalizedPaymentMethod, subtotal + supportFee);

        var (taxAmount, finalAmount, taxNotes) = new TaxCalculator()
            .Calculate(customer.Country, subtotal + supportFee + paymentFee);

        var data = new InvoiceData
        {
            PlanCode = normalizedPlanCode,
            PaymentMethod = normalizedPaymentMethod,
            SeatCount = seatCount,
            BaseAmount = baseAmount,
            DiscountAmount = discountAmount,
            SupportFee = supportFee,
            PaymentFee = paymentFee,
            TaxAmount = taxAmount,
            FinalAmount = finalAmount,
            Notes = notes + supportNotes + paymentNotes + taxNotes
        };

        var invoice = new InvoiceFactory().Create(customer, data);

        new InvoiceNotificationService().SaveInvoice(invoice);
        new InvoiceNotificationService().SendInvoiceEmail(customer, invoice);

        return invoice;
    }
}
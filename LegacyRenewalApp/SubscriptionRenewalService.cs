using System;
using System.Collections.Generic;
using LegacyRenewalApp.SubscriptionService;

namespace LegacyRenewalApp;

public class SubscriptionRenewalService
{
    private readonly CustomerRepository _customerRepository;
    private readonly SubscriptionPlanRepository _planRepository;
    private readonly DiscountCalculator _discountCalculator;
    private readonly SupportFeeCalculator _supportFeeCalculator;
    private readonly PaymentFeeCalculator _paymentFeeCalculator;
    private readonly TaxCalculator _taxCalculator;
    private readonly InvoiceFactory _invoiceFactory;
    private readonly InvoiceNotificationService _notificationService;
    private readonly InvoiceValidator _validator;

    public SubscriptionRenewalService()
    {
        _customerRepository = new CustomerRepository();
        _planRepository = new SubscriptionPlanRepository();
        _discountCalculator = new DiscountCalculator(new IDiscountStrategy[]
        {
            new SilverDiscountStrategy(),
            new GoldDiscountStrategy(),
            new PlatinumDiscountStrategy(),
            new EducationDiscountStrategy(),
            new LongTermLoyaltyDiscountStrategy(),
            new BasicLoyaltyDiscountStrategy(),
            new LargeTeamDiscountStrategy(),
            new MediumDiscountStrategy(),
            new SmallDiscountStrategy()
        });
        _supportFeeCalculator = new SupportFeeCalculator();
        _paymentFeeCalculator = new PaymentFeeCalculator();
        _taxCalculator = new TaxCalculator();
        _invoiceFactory = new InvoiceFactory();
        _notificationService = new InvoiceNotificationService(new LegacyBillingGatewayAdapter());
        _validator = new InvoiceValidator();
    }

    public RenewalInvoice CreateRenewalInvoice(
        int customerId,
        string planCode,
        int seatCount,
        string paymentMethod,
        bool includePremiumSupport,
        bool useLoyaltyPoints)
    {
        _validator.DataValidate(customerId, planCode, seatCount, paymentMethod);

        string normalizedPlanCode = planCode.Trim().ToUpperInvariant();
        string normalizedPaymentMethod = paymentMethod.Trim().ToUpperInvariant();

        var customer = _customerRepository.GetById(customerId);
        var plan = _planRepository.GetByCode(normalizedPlanCode);

        if (!customer.IsActive)
            throw new InvalidOperationException("Inactive customers cannot renew subscriptions");

        decimal baseAmount = (plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee;

        var (discountAmount, discountNotes) = _discountCalculator
            .Calculate(customer, plan, seatCount, useLoyaltyPoints);

        decimal subtotal = baseAmount - discountAmount;
        if (subtotal < 300m) subtotal = 300m;

        var (supportFee, supportNotes) = _supportFeeCalculator
            .Calculate(normalizedPlanCode, includePremiumSupport);

        var (paymentFee, paymentNotes) = _paymentFeeCalculator
            .Calculate(normalizedPaymentMethod, subtotal + supportFee);

        var (taxAmount, finalAmount, taxNotes) = _taxCalculator
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
            Notes = discountNotes + supportNotes + paymentNotes + taxNotes
        };

        var invoice = _invoiceFactory.Create(customer, data);

        _notificationService.SaveInvoice(invoice);
        _notificationService.SendInvoiceEmail(customer, invoice);

        return invoice;
    }
}
using System;

namespace LegacyRenewalApp.SubscriptionService;

public class InvoiceNotificationService
{
    public void SendInvoiceEmail(Customer customer, RenewalInvoice invoice)
    {
        if (string.IsNullOrWhiteSpace(customer.Email))
            return;

        string subject = "Subscription renewal invoice";
        string body = $"Hello {customer.FullName}, your renewal for plan {invoice.PlanCode} " +
                      $"has been prepared. Final amount: {invoice.FinalAmount:F2}.";

        LegacyBillingGateway.SendEmail(customer.Email, subject, body);
    }

    public void SaveInvoice(RenewalInvoice invoice)
    {
        LegacyBillingGateway.SaveInvoice(invoice);
    }
    
}
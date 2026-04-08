namespace LegacyRenewalApp.SubscriptionService;

public class InvoiceNotificationService
{
    private readonly IBillingGateway _billingGateway;

    public InvoiceNotificationService(IBillingGateway billingGateway)
    {
        _billingGateway = billingGateway;
    }

    public void SaveInvoice(RenewalInvoice invoice)
        => _billingGateway.SaveInvoice(invoice);

    public void SendInvoiceEmail(Customer customer, RenewalInvoice invoice)
    {
        if (string.IsNullOrWhiteSpace(customer.Email))
            return;

        string subject = "Subscription renewal invoice";
        string body = $"Hello {customer.FullName}, your renewal for plan {invoice.PlanCode} " +
                      $"has been prepared. Final amount: {invoice.FinalAmount:F2}.";

        _billingGateway.SendEmail(customer.Email, subject, body);
    }
}
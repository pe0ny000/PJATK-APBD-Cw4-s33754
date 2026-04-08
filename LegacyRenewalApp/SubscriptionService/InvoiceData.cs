namespace LegacyRenewalApp.SubscriptionService;

public class InvoiceData
{
    public string PlanCode { get; set; }
    public string PaymentMethod { get; set; }
    public int SeatCount { get; set; }
    public decimal BaseAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal SupportFee { get; set; }
    public decimal PaymentFee { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public string Notes { get; set; }
}
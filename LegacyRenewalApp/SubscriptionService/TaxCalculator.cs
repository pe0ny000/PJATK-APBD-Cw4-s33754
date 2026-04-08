namespace LegacyRenewalApp.SubscriptionService;

public class TaxCalculator
{
    public (decimal taxAmount, decimal finalAmount, string notes) Calculate(
        string country,
        decimal subtotal)
    {
        decimal taxRate = country switch
        {
            "Poland" => 0.23m,
            "Germany" => 0.19m,
            "Czech Republic" => 0.21m,
            "Norway" => 0.25m,
            _ => 0.20m
        };

        decimal taxAmount = subtotal * taxRate;
        decimal finalAmount = subtotal + taxAmount;
        string notes = string.Empty;

        if (finalAmount < 500m)
        {
            finalAmount = 500m;
            notes += "minimum invoice amount applied; ";
        }

        return (taxAmount, finalAmount, notes);
    }
}
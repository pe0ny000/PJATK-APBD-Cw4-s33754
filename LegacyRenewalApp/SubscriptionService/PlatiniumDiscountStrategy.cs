namespace LegacyRenewalApp.SubscriptionService;

public class PlatinumDiscountStrategy : IDiscountStrategy
{
    public string Note => "platinum discount; ";
    public bool Applies(Customer customer, SubscriptionPlan plan, int seatCount) 
        => customer.Segment == "Platinum";
    public decimal Calculate(decimal baseAmount, Customer customer) 
        => baseAmount * 0.15m;
}
namespace LegacyRenewalApp.Discounts;

public class GoldDiscountStrategy : IDiscountStrategy
{
    public string Note => "gold discount; ";
    public bool Applies(Customer customer, SubscriptionPlan plan, int seatCount) 
        => customer.Segment == "Gold";
    public decimal Calculate(decimal baseAmount, Customer customer) 
        => baseAmount * 0.10m;
}
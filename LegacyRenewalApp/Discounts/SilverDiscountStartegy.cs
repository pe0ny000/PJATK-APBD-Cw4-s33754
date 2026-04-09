namespace LegacyRenewalApp.Discounts;

public class SilverDiscountStrategy : IDiscountStrategy
{
    public string Note => "silver discount; ";
    public bool Applies(Customer customer, SubscriptionPlan plan, int seatCount) 
        => customer.Segment == "Silver";
    

    public decimal Calculate(decimal baseAmount, Customer customer) 
        => baseAmount * 0.05m;
}
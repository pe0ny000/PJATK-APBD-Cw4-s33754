namespace LegacyRenewalApp.Discounts;

public class LongTermLoyaltyDiscountStrategy : IDiscountStrategy
{
    public string Note => "long-term loyalty discount; ";
    public bool Applies(Customer customer, SubscriptionPlan plan, int seatCount) 
        => customer.YearsWithCompany >= 5;
    public decimal Calculate(decimal baseAmount, Customer customer) 
        => baseAmount * 0.07m;
}
namespace LegacyRenewalApp.SubscriptionService;

public class BasicLoyaltyDiscountStrategy : IDiscountStrategy
{
    public string Note => "basic loyalty discount; ";

    public bool Applies(Customer customer, SubscriptionPlan plan, int seatCount)
        => customer.YearsWithCompany >= 2 && customer.YearsWithCompany < 5;


    public decimal Calculate(decimal baseAmount, Customer customer)
        => baseAmount * 0.03m;
}
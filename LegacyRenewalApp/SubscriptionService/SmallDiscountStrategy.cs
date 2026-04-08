namespace LegacyRenewalApp.SubscriptionService;

public class SmallDiscountStrategy : IDiscountStrategy
{
    public string Note => "small team discount; ";
    public bool Applies(Customer customer, SubscriptionPlan plan, int seatCount) 
        => seatCount >= 10 && seatCount < 20;
    public decimal Calculate(decimal baseAmount, Customer customer) 
        => baseAmount * 0.04m;
}
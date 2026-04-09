namespace LegacyRenewalApp.Discounts;

public class MediumDiscountStrategy : IDiscountStrategy
{
    public string Note => "medium team discount; ";
    public bool Applies(Customer customer, SubscriptionPlan plan, int seatCount) 
        => seatCount >= 20 && seatCount < 50;
    public decimal Calculate(decimal baseAmount, Customer customer) 
        => baseAmount * 0.08m;
}
namespace LegacyRenewalApp.SubscriptionService;

public class LargeTeamDiscountStrategy : IDiscountStrategy
{
    public string Note => "large team discount; ";
    public bool Applies(Customer customer, SubscriptionPlan plan, int seatCount) 
        => seatCount >= 50;
    public decimal Calculate(decimal baseAmount, Customer customer) 
        => baseAmount * 0.12m;
}
namespace LegacyRenewalApp.SubscriptionService;

public class EducationDiscountStrategy : IDiscountStrategy
{
    public string Note => "education discount; ";
    public bool Applies(Customer customer, SubscriptionPlan plan, int seatCount) 
        => customer.Segment == "Education" && plan.IsEducationEligible;
    public decimal Calculate(decimal baseAmount, Customer customer) 
        => baseAmount * 0.20m;
}
namespace LegacyRenewalApp.SubscriptionService;

public interface IDiscountStrategy
{
    bool Applies(Customer customer, SubscriptionPlan plan, int seatCount);
    decimal Calculate(decimal baseAmount, Customer customer);
    string Note { get; }
}
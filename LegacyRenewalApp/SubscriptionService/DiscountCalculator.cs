using System.Collections.Generic;
using LegacyRenewalApp.Discounts;

namespace LegacyRenewalApp.SubscriptionService;

public class DiscountCalculator
{
    private readonly IEnumerable<IDiscountStrategy> _strategies;

    public DiscountCalculator(IEnumerable<IDiscountStrategy> strategies)
    {
        _strategies = strategies;
    }

    public (decimal discountAmount, string notes) Calculate(
        Customer customer,
        SubscriptionPlan plan,
        int seatCount,
        bool useLoyaltyPoints)
    {
        decimal baseAmount = (plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee;
        decimal discountAmount = 0m;
        string notes = string.Empty;

        foreach (var strategy in _strategies)
        {
            if (strategy.Applies(customer, plan, seatCount))
            {
                discountAmount += strategy.Calculate(baseAmount, customer);
                notes += strategy.Note;
            }
        }

        if (useLoyaltyPoints && customer.LoyaltyPoints > 0)
        {
            int pointsToUse = customer.LoyaltyPoints > 200 ? 200 : customer.LoyaltyPoints;
            discountAmount += pointsToUse;
            notes += $"loyalty points used: {pointsToUse}; ";
        }

        return (discountAmount, notes);
    }
}
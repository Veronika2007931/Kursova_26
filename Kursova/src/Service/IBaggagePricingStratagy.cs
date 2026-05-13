namespace Service
{
    public interface IBaggagePricingStrategy
    {
        decimal CalculatePrice(double weight);
    }
}
namespace Service
{
    public class EconomyBaggage : IBaggagePricingStrategy
    {
        public decimal CalculatePrice(double weight)
        {
            const double limit = 20.0;
            const decimal rate = 250.0m;
            return weight <= limit ? 0 : (decimal)(weight - limit) * rate;
        }
    }
}
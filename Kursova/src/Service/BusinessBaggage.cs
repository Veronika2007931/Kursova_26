namespace Service
{
    public class BusinessBaggage : IBaggagePricingStrategy
    {
        public decimal CalculatePrice(double weight)
        {
            const double limit = 35.0;
            const decimal rate = 500.0m;
            return weight <= limit ? 0 : (decimal)(weight - limit) * rate;
        }
    }
}
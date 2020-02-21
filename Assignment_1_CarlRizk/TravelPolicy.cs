using System;

namespace CarlRizk.Assignment_1
{
    public class TravelPolicy : Policy
    {
        private const int VALIDATION_MAX_DAYS = 30;
        private const int PREMIUM_FAMILY = 10;
        private const int PREMIUM_NO_FAMILY = 5;

        public string Departure { get; private set; }
        public string Destination { get; private set; }
        public bool Family { get; private set; }

        public override string PolicyType => "Travel";
        public override float Premium
        {
            get
            {
                int _numDays = (ExpiryDate - EffectiveDate).Days + 1;
                float _price = Family ? PREMIUM_FAMILY : PREMIUM_NO_FAMILY;
                return Math.Max(_numDays * _price, 0);
            }
        }

        public TravelPolicy(DateTime _effectiveDate, DateTime _expiryDate, string _departure, string _destination, bool _family)
            : base(_effectiveDate, _expiryDate)
        {
            Departure = _departure;
            Destination = _destination;
            Family = _family;
        }

        public override bool IsValid()
        {
            if (!base.IsValid()) return false;
            if ((ExpiryDate - EffectiveDate).Days + 1 > VALIDATION_MAX_DAYS) return false;
            return true;
        }

        protected override string PropertiesToString()
        {
            return base.PropertiesToString() + "\n" +
                $"Departure: {Departure}\n" +
                $"Destination: {Destination}\n" +
                $"Family: {(Family ? "Yes" : "No")}";
        }
    }
}

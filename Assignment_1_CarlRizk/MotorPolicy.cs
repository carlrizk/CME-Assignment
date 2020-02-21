using System;

namespace CarlRizk.Assignment_1
{
    public class MotorPolicy : Policy
    {
        private const float VEHICLE_MULTIPLIER = 0.2f;

        public float VehiclePrice { get; private set; }

        public override string PolicyType => "Motor";
        public override float Premium { get { return VehiclePrice * VEHICLE_MULTIPLIER; } }


        public MotorPolicy(DateTime effectiveDate, DateTime expiryDate, float vehiclePrice) : base(effectiveDate, expiryDate)
        {
            VehiclePrice = vehiclePrice;
        }

        protected override string PropertiesToString()
        {
            return base.PropertiesToString() + "\n" +
                $"Vehicle Price: {VehiclePrice} USD";
        }
    }
}

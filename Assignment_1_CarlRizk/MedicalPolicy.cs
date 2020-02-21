using System;
using System.Collections.Generic;
using System.Linq;

namespace CarlRizk.Assignment_1
{
    public class MedicalPolicy : Policy
    {
        private const int AGE_KID_LIMIT = 10;
        private const int PREMIUM_KID = 15;
        private const int AGE_ADULT_LIMIT = 45;
        private const int PREMIUM_ADULT = 30;
        private const int PREMIUM_SENIOR = 63;

        public override string PolicyType => "Medical";
        public override float Premium
        {
            get
            {
                float result = 0;
                foreach (Beneficiary ben in Beneficiaries)
                {
                    int age = ben.GetAge();
                    if (age < AGE_KID_LIMIT)
                    {
                        result += PREMIUM_KID;
                    }
                    else if (age <= AGE_ADULT_LIMIT)
                    {
                        result += PREMIUM_ADULT;
                    }
                    else result += PREMIUM_SENIOR;
                }
                return result;
            }
        }

        private List<Beneficiary> Beneficiaries { get; set; }

        public MedicalPolicy(DateTime effectiveDate, DateTime expiryDate, List<Beneficiary> beneficiaries) : base(effectiveDate, expiryDate)
        {
            Beneficiaries = beneficiaries;
        }

        public override bool IsValid()
        {
            if (!base.IsValid()) return false;
            if (Beneficiaries.Count < 1) return false;
            int _numberSelf = Beneficiaries.Count((ben) => (ben.Relationship == Relationship.Self));
            if (_numberSelf > 1) return false;
            return true;
        }

        protected override string PropertiesToString()
        {
            return base.PropertiesToString() + "\n" +
                $"Beneficiaries: {Beneficiaries.Count((ben) => ben.Relationship != Relationship.Self)}";
        }
    }
}

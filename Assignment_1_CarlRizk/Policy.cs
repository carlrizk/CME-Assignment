using System;
using System.Collections.Generic;
using System.Linq;

namespace CarlRizk.Assignment_1
{
    public abstract class Policy : IEquatable<Policy>
    {
        private static int CURRENT_ID = 0;

        public int ID { get; private set; }
        public DateTime EffectiveDate { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public string PolicyNumber { get; private set; }

        public abstract string PolicyType { get; }
        public abstract float Premium { get; }

        private HashSet<Claim> claims = new HashSet<Claim>();

        public Policy(DateTime effectiveDate, DateTime expiryDate)
        {
            ID = CURRENT_ID++;

            EffectiveDate = effectiveDate.Date;
            ExpiryDate = expiryDate.Date;

            PolicyNumber = $"{EffectiveDate.Year}-{PolicyType}-{ID}";
        }

        public bool AddClaim(Claim claim)
        {
            if (claim.PolicyNumber != PolicyNumber) throw new ArgumentException($"Claim {claim.PolicyNumber} doesn't match Policy {PolicyNumber}", "claim");
            if (claim.IncurredDate < EffectiveDate || claim.IncurredDate > ExpiryDate) throw new ArgumentException($"Claim is rejected because Policy# {PolicyNumber} is inactive or expired", "claim");
            if (claims.Contains(claim)) throw new ArgumentException("Claim already exist", "claim");
            return claims.Add(claim);
        }
        public virtual bool IsValid()
        {
            return EffectiveDate < ExpiryDate;
        }
        public ClaimsStatistics CalculateClaimsStatistics()
        {
            int count = claims.Count;
            float min = claims.Min(claim => claim.ClaimedAmount);
            float max = claims.Max(claim => claim.ClaimedAmount);
            float total = claims.Sum(claim => claim.ClaimedAmount);
            return new ClaimsStatistics(count, total, min, max);
        }

        protected virtual string PropertiesToString()
        {
            return $"Id: {ID}\n" +
                $"Effective: {EffectiveDate.ToShortDateString()}\n" +
                $"Expiry: {ExpiryDate.ToShortDateString()}\n" +
                $"Policy Number: {PolicyNumber}\n" +
                $"Premium: {Premium} USD\n" +
                $"Valid: {(IsValid() ? "Yes" : "No")}";
        }

        public override string ToString()
        {
            return PropertiesToString() + "\n-----------------------------";
        }
        public bool Equals(Policy other)
        {
            return ID == other.ID;
        }
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

namespace CarlRizk.Assignment_1
{
    public class PolicyCollection : IEnumerable, IEnumerable<Policy>
    {
        private Dictionary<string, Policy> policies = new Dictionary<string, Policy>();

        public bool AddPolicy(Policy policy)
        {
            if (!policy.IsValid())
            {
                Console.WriteLine("Cannot add policy because it is invalid.");
                return false;
            }
            policies.Add(policy.PolicyNumber, policy);
            Console.WriteLine("Policy {0} was added successfully!", policy.PolicyNumber);
            return true;
        }
        public bool AddClaim(Claim claim)
        {
            if (!policies.ContainsKey(claim.PolicyNumber))
            {
                Console.WriteLine("Cannot submit a claim for Policy# {0} because it does not exist.", claim.PolicyNumber);
                return false;
            }

            Policy _policy = policies[claim.PolicyNumber];
            if (claim.IncurredDate < _policy.EffectiveDate || claim.IncurredDate > _policy.ExpiryDate)
            {
                Console.WriteLine("Claim is rejected because Policy# {0} is inactive or expired", claim.PolicyNumber);
                return false;
            }

            return _policy.AddClaim(claim);
        }

        public IEnumerator<Policy> GetEnumerator()
        {
            return policies.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return policies.GetEnumerator();
        }
    }
}

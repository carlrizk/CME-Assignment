using Assignment_2_3_CarlRizk.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment_2_3_CarlRizk.Repository
{
    public interface IPoliciesRepository
    {
        Task<bool> SaveAsync(string methodName);

        Task<bool> PolicyExistsAsync(int policyId);
        Task<bool> PolicyExistsAsync(string policyNumber);
        Task<Policy> GetPolicyAsync(int policyId);
        Task<Policy> GetPolicyAsync(string policyNumber);
        Task<IEnumerable<Policy>> GetPoliciesAsync();
        Task<bool> CreatePolicyAsync(Policy policy);


        Task<bool> ClaimExistsAsync(int claimId);
        Task<Claim> GetClaimAsync(int claimId);
        Task<IEnumerable<Claim>> GetClaimsAsync();
        Task<bool> CreateClaimAgainstPolicyAsync(string policyNumber, Claim claim);
        Task<bool> UpdateClaimAsync(int claimId, float? claimedAmount, DateTime? incurredDate);
        Task<bool> DeleteClaimAsync(int claimId);
        Task<int> DeleteAllClaimsAgainstPolicyAsync(string policyNumber);
    }
}

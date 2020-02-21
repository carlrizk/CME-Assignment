using Assignment_2_3_CarlRizk.Contexts;
using Assignment_2_3_CarlRizk.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment_2_3_CarlRizk.Repository
{
    public class PoliciesRepository : IPoliciesRepository
    {
        private readonly PoliciesDbContext _context;
        private readonly ILogger<PoliciesRepository> _logger;

        public PoliciesRepository(PoliciesDbContext context, ILogger<PoliciesRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> PolicyExistsAsync(int policyId)
        {
            return await _context.Policies
                .AnyAsync(p => p.Id == policyId);

        }
        public async Task<bool> PolicyExistsAsync(string policyNumber)
        {
            return await _context.Policies
                .AnyAsync(p => p.PolicyNumber.ToLower() == policyNumber.ToLower());
        }
        public async Task<Policy> GetPolicyAsync(int policyId)
        {
            return await _context.Policies
                .Include(p => p.Claims)
                .Include(p => p.Beneficiaries)
                .FirstOrDefaultAsync(p => p.Id == policyId);
        }
        public async Task<Policy> GetPolicyAsync(string policyNumber)
        {
            return await _context.Policies
                .Include(p => p.Claims)
                                .Include(p => p.Beneficiaries)
                .FirstOrDefaultAsync(p => p.PolicyNumber == policyNumber);
        }
        public async Task<IEnumerable<Policy>> GetPoliciesAsync()
        {
            return await _context.Policies
                .Include(p => p.Claims)
                                .Include(p => p.Beneficiaries)
                .ToListAsync();
        }
        public async Task<bool> CreatePolicyAsync(Policy policy)
        {
            await _context.Policies.AddAsync(policy);
            return await SaveAsync(nameof(CreatePolicyAsync));
        }


        public async Task<bool> ClaimExistsAsync(int claimId)
        {
            return await _context.Claims
                .AnyAsync(c => c.Id == claimId);
        }
        public async Task<Claim> GetClaimAsync(int claimId)
        {
            return await _context.Claims
                .Include(c => c.Policy)
                .FirstOrDefaultAsync(c => c.Id == claimId);
        }
        public async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            return await _context.Claims
                .Include(c => c.Policy)
                .ToListAsync();
        }
        public async Task<bool> CreateClaimAgainstPolicyAsync(string policyNumber, Claim claim)
        {
            if (!await PolicyExistsAsync(policyNumber))
                return false;

            Policy policy = await GetPolicyAsync(policyNumber);
            policy.Claims.Add(claim);
            return await SaveAsync(nameof(CreateClaimAgainstPolicyAsync));
        }
        public async Task<bool> UpdateClaimAsync(int claimId, float? claimedAmount, DateTime? incurredDate)
        {
            if (!await ClaimExistsAsync(claimId))
                return false;

            Claim claim = await GetClaimAsync(claimId);
            if (claimedAmount.HasValue)
            {
                claim.ClaimedAmount = claimedAmount;
            }
            if (incurredDate.HasValue)
            {
                claim.IncurredDate = incurredDate;
            }

            return await SaveAsync(nameof(UpdateClaimAsync));
        }
        public async Task<bool> DeleteClaimAsync(int claimId)
        {
            if (!await ClaimExistsAsync(claimId))
                return false;

            Claim claim = await GetClaimAsync(claimId);

            _context.Claims.Remove(claim);

            return await SaveAsync(nameof(DeleteClaimAsync));
        }
        public async Task<int> DeleteAllClaimsAgainstPolicyAsync(string policyNumber)
        {
            if (!await PolicyExistsAsync(policyNumber))
                return -1;

            Policy policy = await GetPolicyAsync(policyNumber);

            int count = policy.Claims.Count;

            foreach (Claim c in policy.Claims)
            {
                _context.Remove(c);
            }

            return await SaveAsync(nameof(DeleteAllClaimsAgainstPolicyAsync)) ? count : -1;
        }

        public async Task<bool> SaveAsync(string methodName)
        {
            try
            {
                return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception exp)
            {
                _logger.LogError($"Error in {methodName}: " + exp.Message);
            }
            return false;
        }
    }
}

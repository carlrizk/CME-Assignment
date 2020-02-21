using Assignment_2_3_CarlRizk.Entities;
using Assignment_2_3_CarlRizk.Models;
using Assignment_2_3_CarlRizk.Repository;
using Assignment_2_3_CarlRizk.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_2_3_CarlRizk.Controllers
{
    [ApiController]
    [Route("api/claims")]
    public class ClaimsController : ControllerBase
    {
        private readonly IPoliciesRepository _policyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ClaimsController> _logger;
        private readonly Validator _validator;


        public ClaimsController(IPoliciesRepository policyInfoRepository, IMapper mapper, ILogger<ClaimsController> logger, Validator validator)
        {
            _policyRepository = policyInfoRepository ?? throw new ArgumentNullException(nameof(policyInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentException(nameof(validator));
        }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ClaimOutputDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<string>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetClaims([FromQuery] ClaimInputFilterDto claimFilter)
        {

            //StartElement is required and positive in the Data Transfer Object
            //NumberOfElements is required and positive in the Data Transfer Object

            if (claimFilter == null) claimFilter = new ClaimInputFilterDto();


            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<IEnumerable<string>>(false, _validator.GetErrors(ModelState)));
            }

            var claims = await _policyRepository.GetClaimsAsync();

            if (!string.IsNullOrEmpty(claimFilter.PolicyNumber))
            {
                claims = claims.Where(c => c.PolicyNumber.ToLower().Contains(claimFilter.PolicyNumber.ToLower()));
            }
            if (claimFilter.AmountFrom.HasValue)
            {
                claims = claims.Where(c => c.ClaimedAmount >= claimFilter.AmountFrom);
            }
            if (claimFilter.AmountTo.HasValue)
            {
                claims = claims.Where(c => c.ClaimedAmount <= claimFilter.AmountTo);
            }
            if (claimFilter.DateFrom.HasValue)
            {
                claims = claims.Where(c => c.IncurredDate >= claimFilter.DateFrom);
            }
            if (claimFilter.DateTo.HasValue)
            {
                claims = claims.Where(c => c.IncurredDate <= claimFilter.DateTo);
            }

            claims = claims.OrderByDescending(c => c.ClaimedAmount);
            claims = claims.Skip(claimFilter.StartElement.Value)
                .Take(claimFilter.NumberOfElements.Value);
            return Ok(new ApiResponse<IEnumerable<ClaimOutputDto>>(true, _mapper.Map<IEnumerable<ClaimOutputDto>>(claims)));
        }

        [HttpGet("{claimId}", Name = "GetClaim")]
        [ProducesResponseType(typeof(ApiResponse<ClaimOutputDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<string>>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClaim(int claimId)
        {
            if (!await _policyRepository.ClaimExistsAsync(claimId))
            {
                ModelState.AddModelError(nameof(claimId), "Claim doesnt exist");
                return NotFound(new ApiResponse<IEnumerable<string>>(false, _validator.GetErrors(ModelState)));
            }

            Claim claim = await _policyRepository.GetClaimAsync(claimId);

            return Ok(new ApiResponse<ClaimOutputDto>(true, _mapper.Map<ClaimOutputDto>(claim)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ClaimOutputDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<string>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<string>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateClaim([FromBody] ClaimInputForCreationDto claimForCreation)
        {

            //PolicyNumber is required in the Data Transfer Object
            //IncurredDate is required in the Data Transfer Object
            //ClaimedAmount is required and with minimum of 1 in the Data Transfer Object

            if (claimForCreation == null) claimForCreation = new ClaimInputForCreationDto();

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<IEnumerable<string>>(false, _validator.GetErrors(ModelState)));
            }

            if (!await _policyRepository.PolicyExistsAsync(claimForCreation.PolicyNumber))
            {
                ModelState.AddModelError(nameof(claimForCreation.PolicyNumber), "Policy doesnt exist");
                return NotFound(new ApiResponse<IEnumerable<string>>(false, _validator.GetErrors(ModelState)));
            }

            Policy policy = await _policyRepository.GetPolicyAsync(claimForCreation.PolicyNumber);

            if (claimForCreation.IncurredDate < policy.EffectiveDate)
            {
                ModelState.AddModelError(nameof(claimForCreation.IncurredDate), "IncurredDate must be greater than EffectiveDate");
            }
            if (claimForCreation.IncurredDate > policy.ExpiryDate)
            {
                ModelState.AddModelError(nameof(claimForCreation.IncurredDate), "IncurredDate must be less than ExpiryDate");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<IEnumerable<string>>(false, _validator.GetErrors(ModelState)));
            }

            Claim claim = _mapper.Map<Claim>(claimForCreation);

            if (!await _policyRepository.CreateClaimAgainstPolicyAsync(claimForCreation.PolicyNumber, claim))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>(false, "Internal Server Error"));
            }

            return CreatedAtRoute("GetClaim", new { claimId = claim.Id }, new ApiResponse<ClaimOutputDto>(true, _mapper.Map<ClaimOutputDto>(claim)));
        }

        [HttpPut("{claimId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<string>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<string>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateClaim(int claimId, [FromBody] ClaimInputForUpdateDto claimForUpdate)
        {
            //Nothing is required
            //ClaimedAmount has a minimum value of 1 in the Data Transfer Object

            if (claimForUpdate == null) claimForUpdate = new ClaimInputForUpdateDto();

            if (claimForUpdate.IncurredDate == null && claimForUpdate.ClaimedAmount == null)
            {
                ModelState.AddModelError(nameof(claimForUpdate), "One of ClaimedAmount or IncurredDate is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<IEnumerable<string>>(false, _validator.GetErrors(ModelState)));
            }

            if (!await _policyRepository.ClaimExistsAsync(claimId))
            {
                ModelState.AddModelError(nameof(claimId), "Claim doesnt exist");
                return NotFound(new ApiResponse<IEnumerable<string>>(false, _validator.GetErrors(ModelState)));
            }

            Claim claim = await _policyRepository.GetClaimAsync(claimId);

            Policy policy = claim.Policy;
            if (policy == null)                 //Policy should certainly exist
            {
                _logger.LogError($"Claim {claim.Id} is pointing to a null Policy");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>(false, "Internal Server Error"));
            }

            if (claimForUpdate.IncurredDate.HasValue)
            {
                if (claimForUpdate.IncurredDate <= policy.EffectiveDate)
                {
                    ModelState.AddModelError(nameof(claimForUpdate.IncurredDate), "IncurredDate must be greater than EffectiveDate");
                }
                if (claimForUpdate.IncurredDate >= policy.ExpiryDate)
                {
                    ModelState.AddModelError(nameof(claimForUpdate.IncurredDate), "IncurredDate must be less than ExpiryDate");
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<IEnumerable<string>>(false, _validator.GetErrors(ModelState)));
            }

            if (!await _policyRepository.UpdateClaimAsync(claimId, claimForUpdate.ClaimedAmount, claimForUpdate.IncurredDate))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>(false, "Internal Server Error"));
            }

            return NoContent();
        }

        [HttpDelete("{claimId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<string>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteClaim(int claimId)
        {
            if (!await _policyRepository.ClaimExistsAsync(claimId))
            {
                ModelState.AddModelError(nameof(claimId), "Claim doesnt exist");
                return NotFound(new ApiResponse<IEnumerable<string>>(false, _validator.GetErrors(ModelState)));
            }

            if (!await _policyRepository.DeleteClaimAsync(claimId))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>(false, "Internal Server Error"));
            }

            return Ok(new ApiResponse<bool>(true, true));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<string>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAllClaims([FromBody]ClaimInputForDeleteAllDto claimForDeleteAll)
        {

            //PolicyNumber is required in the Data Transfer Object

            if (claimForDeleteAll == null) claimForDeleteAll = new ClaimInputForDeleteAllDto();

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<IEnumerable<string>>(false, _validator.GetErrors(ModelState)));
            }

            if (!await _policyRepository.PolicyExistsAsync(claimForDeleteAll.PolicyNumber))
            {
                ModelState.AddModelError(nameof(claimForDeleteAll), "Policy doesnt exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<IEnumerable<string>>(false, _validator.GetErrors(ModelState)));
            }

            int count = await _policyRepository.DeleteAllClaimsAgainstPolicyAsync(claimForDeleteAll.PolicyNumber);

            if (count < 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>(false, "Internal Server Error"));

            return Ok(new ApiResponse<int>(true, count));
        }


    }

}

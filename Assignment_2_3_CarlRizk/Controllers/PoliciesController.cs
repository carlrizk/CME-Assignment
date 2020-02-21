using Assignment_2_3_CarlRizk.Entities;
using Assignment_2_3_CarlRizk.Models;
using Assignment_2_3_CarlRizk.Repository;
using Assignment_2_3_CarlRizk.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_2_3_CarlRizk.Controllers
{
    [ApiController]
    [Route("api/policies")]
    public class PoliciesController : ControllerBase
    {
        private readonly IPoliciesRepository _policyRepository;
        private readonly IMapper _mapper;
        private readonly Validator _validator;


        public PoliciesController(IPoliciesRepository policyInfoRepository, IMapper mapper, Validator validator)
        {
            _policyRepository = policyInfoRepository ?? throw new ArgumentNullException(nameof(policyInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PolicyOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPolicies()
        {
            IEnumerable<Policy> policies = await _policyRepository.GetPoliciesAsync();
            policies = policies.Where(p => p.Claims.Count >= 1);

            return Ok(new ApiResponse<IEnumerable<PolicyOutputDto>>(true, _mapper.Map<IEnumerable<PolicyOutputDto>>(policies)));
        }


        [HttpGet("{policyNumber}", Name = "GetPolicy")]
        [ProducesResponseType(typeof(ApiResponse<PolicyOutputDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<string>>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPolicy(string policyNumber)
        {
            if (!await _policyRepository.PolicyExistsAsync(policyNumber))
            {
                ModelState.AddModelError(nameof(policyNumber), "Policy doesnt exist");
                return NotFound(new ApiResponse<IEnumerable<string>>(false, _validator.GetErrors(ModelState)));
            }


            Policy policy = await _policyRepository.GetPolicyAsync(policyNumber);
            return Ok(new ApiResponse<PolicyOutputDto>(true, _mapper.Map<PolicyOutputDto>(policy)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PolicyOutputDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<string>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePolicy([FromBody] PolicyInputForCreationDto policyForCreation)
        {
            //EffectiveDate is required in the Data Transfer Object
            //ExpiryDate is required in the Data Transfer Object
            //Beneficiaries minimum Count of 1 in the Data Transfer Object
            if (policyForCreation == null) policyForCreation = new PolicyInputForCreationDto();

            if (policyForCreation.EffectiveDate <= DateTime.Now)
            {
                ModelState.AddModelError(nameof(policyForCreation.EffectiveDate), "Effective date should be greater than today’s date");
            }
            if (policyForCreation.ExpiryDate <= policyForCreation.EffectiveDate)
            {
                ModelState.AddModelError(nameof(policyForCreation.ExpiryDate), "Expiry date should be greater than Effective date");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<IEnumerable<string>>(false, _validator.GetErrors(ModelState)));
            }

            Policy policy = _mapper.Map<Policy>(policyForCreation);

            if (!await _policyRepository.CreatePolicyAsync(policy))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>(false, "Internal Server Error"));
            }

            return CreatedAtRoute("GetPolicy", new { policyNumber = policy.PolicyNumber }, new ApiResponse<PolicyOutputDto>(true, _mapper.Map<PolicyOutputDto>(policy)));
        }
    }
}

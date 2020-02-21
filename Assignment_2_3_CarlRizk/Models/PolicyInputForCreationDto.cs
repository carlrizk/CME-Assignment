using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment_2_3_CarlRizk.Models
{
    public class PolicyInputForCreationDto
    {

        [Required(ErrorMessage = "EffectiveDate is required")]
        public DateTime? EffectiveDate { get; set; }

        [Required(ErrorMessage = "ExpiryDate is required")]
        public DateTime? ExpiryDate { get; set; }

        [MinLength(1, ErrorMessage = "There should be at least one Beneficiary")]
        public ICollection<BeneficiaryInputDto> Beneficiaries { get; set; } = new List<BeneficiaryInputDto>();
    }
}

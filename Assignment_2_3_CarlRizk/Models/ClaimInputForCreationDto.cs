using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment_2_3_CarlRizk.Models
{
    public class ClaimInputForCreationDto
    {
        [Required(ErrorMessage = "PolicyNumber is required")]
        public string PolicyNumber { get; set; }

        [Required(ErrorMessage = "IncurredDate is required")]
        public DateTime? IncurredDate { get; set; }

        [Required(ErrorMessage = "ClaimedAmount is required")]
        [Range(1, float.MaxValue, ErrorMessage = "ClaimedAmount should be greater than 1")]
        public float? ClaimedAmount { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment_2_3_CarlRizk.Models
{
    public class ClaimInputForUpdateDto
    {
        public DateTime? IncurredDate { get; set; }

        [Range(1, float.MaxValue, ErrorMessage = "ClaimedAmount should be greater than 1")]
        public float? ClaimedAmount { get; set; }
    }
}

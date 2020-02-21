using System;

namespace Assignment_2_3_CarlRizk.Models
{
    public class PolicyOutputDto
    {
        public string PolicyNumber { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public float Premium { get; set; }
        public int NumberOfSubmittedClaims { get; set; }
    }
}

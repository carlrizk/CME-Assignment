using System;

namespace Assignment_2_3_CarlRizk.Models
{
    public class ClaimOutputDto
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime IncurredDate { get; set; }
        public float ClaimedAmount { get; set; }
    }
}

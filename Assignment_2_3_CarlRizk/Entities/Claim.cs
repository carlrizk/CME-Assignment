using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_2_3_CarlRizk.Entities
{
    public class Claim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }

        [Required]
        public DateTime? IncurredDate { get; set; }

        [Required]
        [Range(1, float.MaxValue)]
        public float? ClaimedAmount { get; set; }

        [ForeignKey("policyId")]
        public Policy Policy { get; set; }

        public string PolicyNumber { get { return Policy.PolicyNumber; } }

    }
}

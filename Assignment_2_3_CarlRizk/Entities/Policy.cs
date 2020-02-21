using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_2_3_CarlRizk.Entities
{
    public class Policy
    {
        private const int AGE_KID_LIMIT = 10;
        private const int PREMIUM_KID = 15;
        private const int AGE_ADULT_LIMIT = 45;
        private const int PREMIUM_ADULT = 30;
        private const int PREMIUM_SENIOR = 63;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string PolicyNumber { get; set; }

        [Required]
        public DateTime? EffectiveDate { get; set; }

        [Required]
        public DateTime? ExpiryDate { get; set; }

        public ICollection<Claim> Claims { get; set; } = new List<Claim>();
        public ICollection<Beneficiary> Beneficiaries { get; set; } = new List<Beneficiary>();

        public int NumberOfSubmittedClaims { get { return Claims.Count; } }
        public float Premium
        {
            get
            {
                float result = 0;
                foreach (Beneficiary ben in Beneficiaries)
                {
                    int age = ben.GetAge();
                    if (age < AGE_KID_LIMIT)
                    {
                        result += PREMIUM_KID;
                    }
                    else if (age <= AGE_ADULT_LIMIT)
                    {
                        result += PREMIUM_ADULT;
                    }
                    else result += PREMIUM_SENIOR;
                }
                return result;
            }
        }

    }
}

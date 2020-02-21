using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_2_3_CarlRizk.Entities
{
    public class Beneficiary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [EnumDataType(typeof(Gender))]
        public Gender? Gender { get; set; }

        [Required]
        [EnumDataType(typeof(Relationship))]
        public Relationship? Relationship { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [ForeignKey("PolicyId")]
        public Policy Policy { get; set; }

        public int GetAge()
        {
            int age = DateTime.Now.Year - DateOfBirth.Value.Year;
            if (DateTime.Now.DayOfYear < DateOfBirth?.DayOfYear)
                age -= 1;
            return age;
        }
    }
}

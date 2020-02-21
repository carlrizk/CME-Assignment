using Assignment_2_3_CarlRizk.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment_2_3_CarlRizk.Models
{
    public class BeneficiaryInputDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [EnumDataType(typeof(Gender), ErrorMessage = "Gender can be Male or Female")]
        public Gender? Gender { get; set; }

        [Required(ErrorMessage = "Relationship is required")]
        [EnumDataType(typeof(Relationship), ErrorMessage = "Relationship can be Self, Spouse, Son or Daughter")]
        public Relationship? Relationship { get; set; }

        [Required(ErrorMessage = "DateOfBirth is required")]
        public DateTime? DateOfBirth { get; set; }
    }
}

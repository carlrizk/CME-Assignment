using System.ComponentModel.DataAnnotations;

namespace Assignment_2_3_CarlRizk.Models
{
    public class ClaimInputForDeleteAllDto
    {
        [Required(ErrorMessage = "PolicyNumber is required")]
        public string PolicyNumber { get; set; }
    }
}

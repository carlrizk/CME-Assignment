using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment_2_3_CarlRizk.Models
{
    public class ClaimInputFilterDto
    {
        public string PolicyNumber { get; set; }
        public float? AmountFrom { get; set; }
        public float? AmountTo { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        [Required(ErrorMessage = "StartElement must be defined")]
        [Range(0, int.MaxValue, ErrorMessage = "StartElement must be positive")]
        public int? StartElement { get; set; }
        [Required(ErrorMessage = "NumberOfElements must be defined")]
        [Range(0, int.MaxValue, ErrorMessage = "NumberOfElements must be positive")]
        public int? NumberOfElements { get; set; }
    }
}

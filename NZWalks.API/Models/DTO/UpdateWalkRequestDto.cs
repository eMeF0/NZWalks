using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateWalkRequestDto
    {
        [Required]
        [MaxLength(150, ErrorMessage = "Name has to be a maximum of 150 characters!")]
        public string Name { get; set; }
        [Required]
        [MaxLength(500, ErrorMessage = "Description has to be a maximum of 500 characters!")]
        public string Description { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "LengthInKm has to be a non-negative value!")]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }
    }
}

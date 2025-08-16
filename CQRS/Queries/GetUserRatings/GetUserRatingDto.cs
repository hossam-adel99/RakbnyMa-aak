using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.CQRS.Queries.GetUserRatings
{
    public class GetUserRatingDto
    {
        public string? RaterId { get; set; }

        [Range(1, 5, ErrorMessage = "يجب أن يكون فلتر النجوم بين 1 و 5.")]
        public int? StarsFilter { get; set; }

        public bool? HasComment { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}

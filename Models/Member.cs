using System.ComponentModel.DataAnnotations;

namespace HeirWebApp.Models
{
    public class Member
    {
        public int id { get; set; }

        // = string.Empty removes the "non-nullable property" warnings.
        [Required]
        public string name { get; set; } = string.Empty;

        [Required]
        public string role { get; set; } = string.Empty;

        public string? contact { get; set; } = string.Empty;

        public string? description { get; set; } = string.Empty;

        // Self-referencing link for the hierarchy.
        // a null parentId means the member is the root of a tree.
        public int? parentId { get; set; }

        public Member()
        {
        }
    }
}

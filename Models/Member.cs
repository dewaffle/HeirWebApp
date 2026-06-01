using System.ComponentModel.DataAnnotations;

namespace HeirWebApp.Models
{
    public class Member
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Role")]
        public string role { get; set; } = string.Empty;

        [Display(Name = "Contact")]
        public string? contact { get; set; }

        [Display(Name = "Description")]
        public string? description { get; set; }

        // Self-referencing link for the hierarchy.
        // a null parentId means the member is the root of a tree.
        [Display(Name = "Parent")]
        public int? parentId { get; set; }

        public Member()
        {
        }
    }
}
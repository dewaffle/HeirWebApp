namespace HeirWebApp.Models
{
    public class Member
    {
        public int id { get; set; }
        public string name { get; set; }
        public string role { get; set; }
        public string contact { get; set; }
        public string description { get; set; }

        // Self-referencing link for the hierarchy.
        // a null parentId means the member is the root of a tree.
        public int? parentId { get; set; }

        public Member()
        {
        }
    }
}
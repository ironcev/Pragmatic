using System;

namespace Pragmatic.Example.Model
{
    public class BlogPostComment : Entity
    {
        public Guid BlogPostId { get; set; }
        public string Comment { get; set; }
        public string Title { get; set; }
        public string CommentatorName { get; set; }
        public string CommentatorEmail { get; set; }
    }
}

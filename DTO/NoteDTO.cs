using System;

namespace ThoughtKeeper.DTO
{
    public class NoteDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public int UserId { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}


using Dapper;
using System.Linq;
using ThoughtKeeper.DTO;
using System.Collections.Generic;
using ThoughtKeeper.Database;
using ThoughtKeeper.Interfaces;

namespace ThoughtKeeper.Service
{
    public class NoteService : INoteService
    {
        private readonly INoteCryptoService _noteCryptoService;
        private readonly ICategoryService _categoryService;

        public NoteService(INoteCryptoService noteCryptoService, ICategoryService categoryService)
        {
            _noteCryptoService = noteCryptoService;
            _categoryService = categoryService;
        }

        public IEnumerable<NoteDTO> GetAllNotes(int userId)
        {
            var notes = new List<NoteDTO>();
            var categories = _categoryService.GetAll();

            using (var connection = DatabaseContext.GetDbConnection())
            {
                notes = connection.Query<NoteDTO>("SELECT * FROM Notes WHERE UserId = @UserId", new { UserId = userId }).ToList();
                notes.ForEach(note =>
                {
                    note.Content = _noteCryptoService.DecryptString(note.Content);
                    note.CategoryName = categories.FirstOrDefault(category => category.Id == note.CategoryId)?.Name;
                });
            }

            return notes;
        }

        public void AddNote(NoteDTO note)
        {
            using (var connection = DatabaseContext.GetDbConnection())
            {
                var encryptedContent = _noteCryptoService.EncryptString(note.Content);
                var sql = "INSERT INTO Notes (Title, Content, DateCreated, UserId, CategoryId) VALUES (@Title, @Content, GETDATE(), @UserId, @CategoryId);";
                connection.Execute(sql, new { note.Title, Content = encryptedContent, UserId = note.UserId, CategoryId = note.CategoryId });
            }
        }

        public void EditNote(NoteDTO note, int userId)
        {
            using (var connection = DatabaseContext.GetDbConnection())
            {
                var encryptedContent = _noteCryptoService.EncryptString(note.Content);
                var sql = "UPDATE Notes SET Title = @Title, Content = @Content, CategoryId = @CategoryId WHERE Id = @Id AND UserId = @UserId;";
                connection.Execute(sql, new { note.Id, note.Title, Content = encryptedContent, CategoryId = note.CategoryId, UserId = userId });
            }
        }

        public void DeleteNote(int noteId)
        {
            using (var connection = DatabaseContext.GetDbConnection())
            {
                var sql = "DELETE FROM Notes WHERE Id = @NoteId;";
                connection.Execute(sql, new { NoteId = noteId });
            }
        }

        public void AssignNoteToCategory(int id, int? categoryId, int userId)
        {
            using (var connection = DatabaseContext.GetDbConnection())
            {
                var sql = "UPDATE Notes SET CategoryId = @CategoryId WHERE Id = @NoteId AND UserId = @UserId;";
                connection.Execute(sql, new { NoteId = id, CategoryId = categoryId, UserId = userId });
            }
        }
    }
}

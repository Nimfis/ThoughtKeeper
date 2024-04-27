using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ThoughtKeeper.DTO;

namespace ThoughtKeeper.Service
{
    public class NoteService : INoteService
    {
        private readonly string _connectionString;
        private readonly INoteCryptoService _noteCryptoService;
        private readonly IUserService _userService;

        public NoteService(string connectionString, INoteCryptoService noteCryptoService, IUserService userService)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _noteCryptoService = noteCryptoService ?? throw new ArgumentNullException(nameof(noteCryptoService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public IEnumerable<NoteDTO> GetAllNotes(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var notes = connection.Query<NoteDTO>("SELECT * FROM Notes WHERE UserId = @UserId", new { UserId = userId }).ToList();
                notes.ForEach(note => note.Content = _noteCryptoService.DecryptString(note.Content));
                return notes;
            }
        }

        public void AddNote(NoteDTO note)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var encryptedContent = _noteCryptoService.EncryptString(note.Content);
                var sql = "INSERT INTO Notes (Title, Content, DateCreated, UserId) VALUES (@Title, @Content, GETDATE(), @UserId);";
                connection.Execute(sql, new { note.Title, Content = encryptedContent, UserId = note.UserId });
            }
        }

        public void EditNote(NoteDTO note, int noteId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var encryptedContent = _noteCryptoService.EncryptString(note.Content);
                var sql = "UPDATE Notes SET Title = @Title, Content = @Content WHERE Id = @Id AND UserId = @UserId;";
                connection.Execute(sql, new { note.Id, note.Title, Content = encryptedContent, UserId = note.UserId });
            }
        }

        public void DeleteNote(int noteId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "DELETE FROM Notes WHERE Id = @NoteId;";
                connection.Execute(sql, new { NoteId = noteId });
            }
        }

        public void AssignNoteToCategory(int noteId, int categoryId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "UPDATE Notes SET CategoryId = @CategoryId WHERE Id = @NoteId AND UserId = @UserId;";
                connection.Execute(sql, new { NoteId = noteId, CategoryId = categoryId });
            }
        }

    }
}

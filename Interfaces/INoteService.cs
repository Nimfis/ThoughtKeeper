using System.Collections.Generic;
using ThoughtKeeper.DTO;

public interface INoteService
{
    IEnumerable<NoteDTO> GetAllNotes(int userId);
    void AddNote(NoteDTO note);
    void EditNote(NoteDTO note, int _userId);
    void DeleteNote(int noteId);
}

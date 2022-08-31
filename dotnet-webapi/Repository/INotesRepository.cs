
using DotnetWebAPI.Models;

namespace DotnetWebAPI.Repository {
    public interface INotesRepository {

        Task<Note?> GetNoteAsync(int id);
        Task<List<Note>> GetNotesAsync();

        Task InsertNoteAsync(Note note);

        Task<bool> DeleteNoteAsync(int id);
    }
}


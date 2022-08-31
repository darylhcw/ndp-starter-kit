using Microsoft.EntityFrameworkCore;
using DotnetWebAPI.Models;

namespace DotnetWebAPI.Repository {

    public class NotesRepository : INotesRepository {
        private readonly NotesDbContext _context;
        private readonly ILogger _logger;


        public NotesRepository(NotesDbContext context, ILoggerFactory loggerFactory) {
            _context = context;
            _logger = loggerFactory.CreateLogger("NotesRepository");
        }


        public async Task<Note?> GetNoteAsync(int id) {
            try {
                return await _context.Notes.FindAsync(id);
            } catch (Exception ex) {
                _logger.LogError($"Error in {nameof(GetNoteAsync)}:" + ex.Message);
            }

            return null;
        }


        public async Task<List<Note>> GetNotesAsync() {
            try {
                return await _context.Notes.ToListAsync();
            } catch (Exception ex) {
                _logger.LogError($"Error in {nameof(GetNotesAsync)}:" + ex.Message);
            }
            return new List<Note>();
        }


        public async Task InsertNoteAsync(Note note) {
            _context.Notes.Add(note);
            try {
                await _context.SaveChangesAsync();
            } catch (Exception ex) {
                _logger.LogError($"Error in {nameof(InsertNoteAsync)}:" + ex.Message);
            }
        }


        public async Task<bool> DeleteNoteAsync(int id) {            
            var toDelete = await _context.Notes.FindAsync(id);
            if (toDelete == null) {
                _logger.LogError($"Error in {nameof(DeleteNoteAsync)}, unable to find note with id: {id}");
                return false;
            }

            _context.Notes.Remove(toDelete);
            try {
                await _context.SaveChangesAsync();
            } catch (Exception ex) {
                _logger.LogError($"Error in {nameof(DeleteNoteAsync)}:" + ex.Message);
                return false;
            }

            return true;
        }
    }
}
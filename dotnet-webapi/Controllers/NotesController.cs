using Microsoft.AspNetCore.Mvc;
using DotnetWebAPI.Models;
using DotnetWebAPI.Repository;

namespace DotnetWebAPI.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase {
        private readonly INotesRepository _repo;
        private readonly ILogger<NotesController> _logger;


        public NotesController(INotesRepository repo, ILogger<NotesController> logger) {
            _repo = repo;
            _logger = logger;
        }


        [HttpGet(Name = "GetNotes")]
        public async Task<List<Note>> GetAll() {
            var notes = await _repo.GetNotesAsync();

            return notes;
        }


        [HttpGet("{id}", Name = "GetNote")]
        public async Task<ActionResult<Note>> Get(int id) {
            var note = await _repo.GetNoteAsync(id);
            if (note == null) return NotFound();

            return note;
        }


        [HttpPost("add", Name = "AddNote")]
        public async Task<ActionResult<Note>> Add(Note note) {
            if (note == null) return BadRequest();

            var newNote = new Note(note.Text);            
            await _repo.InsertNoteAsync(newNote);

            return CreatedAtAction(nameof(Get), new { id = newNote.Id }, newNote);
        }


        [HttpDelete("{id}", Name = "DeleteNote")]
        public async Task<ActionResult> Delete(int id) {
            bool res = await _repo.DeleteNoteAsync(id);
            if (res) {
                return NoContent();
            } else {
                return NotFound();
            }
        }
    }
}

using EFCoreWebApi.DTO;
using EFCoreWebApi.Models;
using EFCoreWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteRepository _noteRepository;

        public NotesController(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            return Ok(await _noteRepository.GetAllAsync());
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            var Note = await _noteRepository.GetByIdAsync(id);
            if (Note == null)
            {
                return NotFound();
            }
            return Ok(Note);
        }

        // POST: api/Notes
        [HttpPost]
        public async Task<ActionResult<Note>> Add(Note Note)
        {
            await _noteRepository.AddAsync(Note);
            return CreatedAtAction(nameof(GetNote), new { id = Note.ID }, Note);
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, Note Note)
        {
            if (id != Note.ID)
            {
                return BadRequest();
            }

            await _noteRepository.UpdateAsync(Note);
            return NoContent();
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            bool isDeleted = await _noteRepository.DeleteAsync(id);

            if (isDeleted)
            {
                return Ok(new { success = true, message = "Note deleted successfully." });
            }
            else
            {
                return NotFound(new { success = false, message = "Note not found or could not be deleted." });
            }
        }
        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<Note>>> SearchNote([FromBody] searchDTO search)
        {
            if (search.searchText == null || string.IsNullOrWhiteSpace(search.searchText))
            {
                return BadRequest(new { message = "Search text cannot be empty." });
            }

            var Notes = await _noteRepository.Search(search.propertyName, search.searchText);

            if (Notes == null || !Notes.Any())
            {
                return NotFound(new { message = "No Notes found." });
            }

            return Ok(Notes);
        }
        [HttpGet("getByUserId/{id}")]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotesByUserId(int id)
        {
            var Notes = await _noteRepository.GetNotesByUserIdAsync(id);
            return Ok(Notes);
        }

    }
}
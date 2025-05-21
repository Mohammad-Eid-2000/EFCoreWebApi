using EFCoreWebApi.Data;
using EFCoreWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreWebApi.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext _context;
        public NoteRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Note entity)
        {
            entity.CreationDate = DateTime.Now;
            await _context.Notes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return false;
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Note>> GetAllAsync()
        {
            return await _context.Notes.ToListAsync();
        }

        public async Task<Note?> GetByIdAsync(int id)
        {
            return await _context.Notes.FindAsync(id);

        }

        public async Task <List<Note>> GetNotesByUserIdAsync(int userId)
        {
            return await _context.Notes.Where(x=>x.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Note>> Search(string propertyName, string searchText)
        {
            IQueryable<Note> query = _context.Notes;

            switch (propertyName)
            {
                case nameof(Note.Name):
                    query = query.Where(x => x.Name == searchText);
                    break;
                case nameof(Note.Description):
                    query = query.Where(x => x.Description == searchText);
                    break;
                default:
                    throw new ArgumentException($"Property '{propertyName}' is not supported for searching.");
            }
            var result = await query.ToListAsync();
            return result;
        }

        public Task<Note?> SearchSingle(string searchText)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Note entity)
        {
            _context.Notes.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}

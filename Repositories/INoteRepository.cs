using EFCoreWebApi.Models;
using EFCoreWebApi.Repositories;

public interface INoteRepository : IRepository<Note>
{
    Task<List<Note>> GetNotesByUserIdAsync(int userId);
}
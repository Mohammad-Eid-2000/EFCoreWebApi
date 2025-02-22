using EFCoreWebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
namespace EFCoreWebApi.Models
{
    public class Note
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime CreationDate { get; set; }
    }
}

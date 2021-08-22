using System;

namespace Portfolio.Back.Repositories.Database.Dtos
{
    public class InformationDatabaseDto
    {
        public int Id { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}
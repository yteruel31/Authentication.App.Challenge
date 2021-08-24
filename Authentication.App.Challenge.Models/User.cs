using System;

namespace Authentication.App.Challenge.Models
{
    public class User
    {
        public int Id { get; set; }
        
        public string Email { get; set; }
        
        public string Name { get; set; }

        public string Bio { get; set; }

        public string Phone { get; set; }
    }
}
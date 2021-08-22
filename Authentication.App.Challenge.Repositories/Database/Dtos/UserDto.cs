using Portfolio.Back.Repositories.Database.Dtos;

namespace Authentication.App.Challenge.Repositories.Database.Dtos
{
    public class UserDto : InformationDatabaseDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public byte[] Salt { get; set; }

        public string Name { get; set; }

        public string Bio { get; set; }

        public string Phone { get; set; }
    }
}
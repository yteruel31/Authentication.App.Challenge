using Microsoft.AspNetCore.Mvc;

namespace Authentication.App.Challenge.Web
{
    public class AuthenticationHeader
    {
        [FromHeader]
        public string Authorization { get; set; }
    }
}
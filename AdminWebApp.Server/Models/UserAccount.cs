using System;

namespace AdminWebApp.Server.Models
{
    public class UserAccount
    {
        public int Userid { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool? IsActive { get; set; }
    }
}
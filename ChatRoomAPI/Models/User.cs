using ChatRoomAPI.Data;
using ChatRoomWeb.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChatRoomAPI.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Email))]
    public class User
    {
        public User(string username, string email, string passwordHash)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            IsConfirmed = false;
            CreationDate = DateTime.Now;
        }
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid? VerificationData { get; set; }
    }
}

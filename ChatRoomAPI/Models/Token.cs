using ChatRoomAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatRoomAPI.Data
{
    public class Token
    {
        [Key, Required]
        public string UserEmail { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshExpirationDate { get; set; }
    }
}

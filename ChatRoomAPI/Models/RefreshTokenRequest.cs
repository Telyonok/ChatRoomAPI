﻿namespace ChatRoomAPI.Models
{
    public class RefreshTokenRequest
    {
        public string Email { get; set; }
        public string RefreshToken { get; set; }
    }
}
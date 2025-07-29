﻿namespace TorneioSC.WebApi.Dtos.TokenResponseDtos
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string TokenType { get; set; } = "Bearer";
        public int ExpiresIn { get; set; }
    }
}

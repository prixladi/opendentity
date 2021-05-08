﻿using System.ComponentModel.DataAnnotations;

namespace Shamyr.Opendentity.Service.Models
{
    public record CreateUserModel
    {
        [Required]
        public string UserName { get; init; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; init; } = default!;

        [Required]
        public string Password { get; init; } = default!;


        public string? FirstName { get; init; } 
        public string? LastName { get; init; } = default!;
        public string? ImageUrl { get; init; } = default!;
    }
}

﻿
using System.Text.Json.Serialization;

namespace Notebook.Domain.Responses
{
    public record GetContactResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}

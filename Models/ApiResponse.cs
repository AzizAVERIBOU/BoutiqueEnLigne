using System.Collections.Generic;
using System.Text.Json.Serialization;
using BoutiqueEnLigne.Models.User;

namespace BoutiqueEnLigne.Models
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("users")]
        public List<T> Users { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("skip")]
        public int Skip { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }
    }

    public class DummyUser
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("maidenName")]
        public string MaidenName { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("birthDate")]
        public string BirthDate { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("bloodGroup")]
        public string BloodGroup { get; set; }

        [JsonPropertyName("height")]
        public double Height { get; set; }

        [JsonPropertyName("weight")]
        public double Weight { get; set; }

        [JsonPropertyName("eyeColor")]
        public string EyeColor { get; set; }

        [JsonPropertyName("hair")]
        public Hair Hair { get; set; }

        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }

        [JsonPropertyName("macAddress")]
        public string MacAddress { get; set; }

        [JsonPropertyName("university")]
        public string University { get; set; }

        [JsonPropertyName("bank")]
        public Bank Bank { get; set; }

        [JsonPropertyName("company")]
        public Company Company { get; set; }

        [JsonPropertyName("ein")]
        public string Ein { get; set; }

        [JsonPropertyName("ssn")]
        public string Ssn { get; set; }

        [JsonPropertyName("userAgent")]
        public string UserAgent { get; set; }
    }

    public class Hair
    {
        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
} 
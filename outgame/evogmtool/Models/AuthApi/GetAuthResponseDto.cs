using System;
using System.ComponentModel.DataAnnotations;

namespace evogmtool.Models.AuthApi
{
    public class GetAuthResponseDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Account { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public Publisher_ Publisher { get; set; }
        [Required]
        public Timezone_ Timezone { get; set; }
        [Required]
        public Language_ Language { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }

        public class Publisher_
        {
            [Required]
            public int PublisherId { get; set; }
            [Required]
            public string PublisherName { get; set; }
        }

        public class Timezone_
        {
            [Required]
            public string TimezoneCode { get; set; }
        }

        public class Language_
        {
            [Required]
            public string LanguageCode { get; set; }
            [Required]
            public string LanguageName { get; set; }
        }
    }
}

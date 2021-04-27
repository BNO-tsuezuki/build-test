using System;
using System.ComponentModel.DataAnnotations;

namespace evogmtool.Models.LanguageApi
{
    public class GetLanguageResponseDto
    {
        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.PlayerInformation
{
    public class TutorialProgress
    {
        public class Request
        {
            [Required]
            public evolib.PlayerInformation.TutorialType? tutorialType { get; set; }
        }

        public class Response
        {
            public evolib.PlayerInformation.TutorialType tutorialType { get; set; }
        }
    }
}

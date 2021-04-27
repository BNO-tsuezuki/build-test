using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace evogmtool.Models.GameLogApi
{
    public class GetChatSayHistoryListRequestDto : GameLogRequestBaseDto, IValidatableObject
    {
        [BindRequired]
        [Required]
        [Range(0, 3)]
        public int SearchType { get; set; }

        public long? PlayerId { get; set; }

        public string GroupId { get; set; }

        public string MatchId { get; set; }

        public int? Side { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            switch (SearchType)
            {
                case 0: // from all chat
                    if (!PlayerId.HasValue)
                    {
                        yield return new ValidationResult(
                            $"A value for the '{ nameof(PlayerId) }' property was not provided.",
                            new[] { nameof(PlayerId) });
                    }
                    break;
                case 1: // from party chat
                    // ソロの場合パーティーチャットでもGroupIdが空文字列で登録されるので、ここでは必須にしない。
                    break;
                case 2: // from team chat
                    if (string.IsNullOrWhiteSpace(MatchId))
                    {
                        yield return new ValidationResult(
                            $"A value for the '{ nameof(MatchId) }' property was not provided.",
                            new[] { nameof(MatchId) });
                    }

                    if (!Side.HasValue)
                    {
                        yield return new ValidationResult(
                            $"A value for the '{ nameof(Side) }' property was not provided.",
                            new[] { nameof(Side) });
                    }
                    else if (Side < 0 || 1 < Side)
                    {
                        yield return new ValidationResult(
                            $"The field { nameof(Side) } must be between 0 and 1.",
                            new[] { nameof(Side) });
                    }
                    break;
                case 3: // from room chat
                    if (string.IsNullOrWhiteSpace(MatchId))
                    {
                        yield return new ValidationResult(
                            $"A value for the '{ nameof(MatchId) }' property was not provided.",
                            new[] { nameof(MatchId) });
                    }
                    break;
            }
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace evoapi.ProtocolModels.PlayerInformation
{
	[System.AttributeUsage(System.AttributeTargets.Property)]
	public class PlayerNameValidateAttribute : ValidationAttribute
	{
		string nameMatchPattern = "^[a-zA-Z][a-zA-Z0-9_-]+$";

		int minimumLength = 3;
		int maxLength = 16;

		public override bool IsValid(object value)
		{
			var playerName = value as string;

			if (!Regex.IsMatch(playerName, nameMatchPattern))
			{
				ErrorMessage = "The field {0} must match the regular expression '" + nameMatchPattern + "'.";
				return false;
			}

			if (playerName.Length < minimumLength || maxLength < playerName.Length)
			{
				ErrorMessage = "The field {0} must be a string with a minimum length of " + minimumLength + " and a maximum length of " + maxLength + ".";
				return false;
			}

			return true;
		}
	}
}

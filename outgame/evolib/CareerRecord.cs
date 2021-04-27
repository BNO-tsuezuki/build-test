namespace evolib
{
	public class CareerRecord
	{
		public const int RECORD_MAX_VALUE = 999999999;

		public const string RECORD_ID_PLAY_TIME = "PlayTime";

		public enum ValueType
		{
			Sum=0,
			SumAverage,
			MaxAverage,
			Max,
			Average10Minutes,
			Rate,
		}

		public enum CategoryType
		{
			General=0,
			Special,
		}

		public enum FormatType
		{
			Time_HHHHMMSS=0,
			Time_MMSS,
			Int_2Digit,
			Int_3Digit,
			Int_9Digit,
			Decimal,
			Percentage,
		}

		public enum DisplayType
		{
			Self=0,
			CompMyTier,
			CompTop50,
			CompAll,
		}
	}
}

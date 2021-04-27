using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;


namespace evotool.ProtocolModels.OpsNotice
{
	public class CommonDesc
	{
		[Required]
		public bool? release { get; set; }

		[Required]
		public UInt64? target { get; set; }

		public string memo { get; set; }

		[Required]
		public DateTime? beginDate { get; set; }
		[Required]
		public DateTime? endDate { get; set; }


		public bool enabledEnglish { get; set; }
		public string msgEnglish { get; set; }

		public bool enabledFrench { get; set; }
		public string msgFrench { get; set; }

		public bool enabledGerman { get; set; }
		public string msgGerman { get; set; }

		public bool enabledJapanese { get; set; }
		public string msgJapanese { get; set; }
	}


	public class ChatDesc : CommonDesc
	{
		[Required]
		public int? times { get; set; }

		public int repeatedIntervalMinutes { get; set; }
	}


	public class PopupDesc : CommonDesc
	{
		public string titleEnglish { get; set; }

		public string titleFrench { get; set; }

		public string titleGerman { get; set; }

		public string titleJapanese { get; set; }
	}


	public class TopicsDesc : CommonDesc
	{
        public int priority { get; set; }

		public string titleEnglish { get; set; }

		public string titleFrench { get; set; }

		public string titleGerman { get; set; }

		public string titleJapanese { get; set; }

        public string redirectUI { get; set; }
	}


	public static class OpsNoticeExtPush
	{
		static bool Push(
				this evolib.Databases.common1.OpsNotice rec,
				CommonDesc common
		){
			var edited =
			!Equals(rec.beginDate, common.beginDate) ||
			!Equals(rec.endDate, common.endDate) ||
			(rec.enabledEnglish != common.enabledEnglish) ||
			(rec.enabledFrench != common.enabledFrench) ||
			(rec.enabledGerman != common.enabledGerman) ||
			(rec.enabledJapanese != common.enabledJapanese) ||
			(rec.enabledEnglish && rec.msgEnglish != common.msgEnglish) ||
			(rec.enabledFrench && rec.msgFrench != common.msgFrench) ||
			(rec.enabledGerman && rec.msgGerman != common.msgGerman) ||
			(rec.enabledJapanese && rec.msgJapanese != common.msgJapanese);

			rec.release = common.release.Value;
			rec.target = common.target.Value;
			rec.memo = common.memo;

			rec.beginDate = common.beginDate.Value;
			rec.endDate = common.endDate.Value;

			rec.enabledEnglish = common.enabledEnglish;
			rec.msgEnglish = common.msgEnglish;
			rec.enabledFrench = common.enabledFrench;
			rec.msgFrench = common.msgFrench;
			rec.enabledGerman = common.enabledGerman;
			rec.msgGerman = common.msgGerman;
			rec.enabledJapanese = common.enabledJapanese;
			rec.msgJapanese = common.msgJapanese;

			return edited;
		}
	
		public static bool Push(
				this evolib.Databases.common1.OpsNotice rec,
				ChatDesc chatDesc
		)
		{
			var edited = rec.Push((CommonDesc)chatDesc)
				|| (rec.times != chatDesc.times.Value)
				|| (rec.repeateIntervalMinutes != chatDesc.repeatedIntervalMinutes);

			rec.optNoticeType = evolib.OptNoticeType.Chat;

			rec.times = chatDesc.times.Value;
			rec.repeateIntervalMinutes = chatDesc.repeatedIntervalMinutes;

			if( edited)
			{
				rec.version++;
			}

			return edited;
		}
		public static bool Push(
			this evolib.Databases.common1.OpsNotice rec,
			PopupDesc popupDesc
		)
		{
			var edited = rec.Push((CommonDesc)popupDesc)
				|| (rec.enabledEnglish && rec.titleEnglish != popupDesc.titleEnglish)
				|| (rec.enabledFrench && rec.titleFrench != popupDesc.titleFrench)
				|| (rec.enabledGerman && rec.titleGerman != popupDesc.titleGerman)
				|| (rec.enabledJapanese && rec.titleJapanese != popupDesc.titleJapanese);

			rec.optNoticeType = evolib.OptNoticeType.Popup;

			rec.titleEnglish = popupDesc.titleEnglish;
			rec.titleFrench = popupDesc.titleFrench;
			rec.titleGerman = popupDesc.titleGerman;
			rec.titleJapanese = popupDesc.titleJapanese;

			if (edited)
			{
				rec.version++;
			}

			return edited;
		}
		public static bool Push(
			this evolib.Databases.common1.OpsNotice rec,
			TopicsDesc topicsDesc
		)
		{
			var edited = rec.Push((CommonDesc)topicsDesc)
				|| (rec.priority != topicsDesc.priority)
				|| (rec.enabledEnglish && rec.titleEnglish != topicsDesc.titleEnglish)
				|| (rec.enabledFrench && rec.titleFrench != topicsDesc.titleFrench)
				|| (rec.enabledGerman && rec.titleGerman != topicsDesc.titleGerman)
				|| (rec.enabledJapanese && rec.titleJapanese != topicsDesc.titleJapanese)
				|| (rec.redirectUI != topicsDesc.redirectUI);

			rec.optNoticeType = evolib.OptNoticeType.Topics;

			rec.priority = topicsDesc.priority;
			rec.titleEnglish = topicsDesc.titleEnglish;
			rec.titleFrench = topicsDesc.titleFrench;
			rec.titleGerman = topicsDesc.titleGerman;
			rec.titleJapanese = topicsDesc.titleJapanese;
			rec.redirectUI = topicsDesc.redirectUI;

			if (edited)
			{
				rec.version++;
			}

			return edited;
		}
	}
	static class OpsNoticeExtPop
	{
		static void Pop(
			this evolib.Databases.common1.OpsNotice rec,
			CommonDesc common
		)
		{
			common.release = rec.release;
			common.target = rec.target;
			common.memo = rec.memo;

			common.beginDate = rec.beginDate;
			common.endDate = rec.endDate;

			common.enabledEnglish = rec.enabledEnglish;
			common.msgEnglish = rec.msgEnglish;
			common.enabledFrench = rec.enabledFrench;
			common.msgFrench = rec.msgFrench;
			common.enabledGerman = rec.enabledGerman;
			common.msgGerman = rec.msgGerman;
			common.enabledJapanese = rec.enabledJapanese;
			common.msgJapanese = rec.msgJapanese;
		}
	
		public static ChatDesc Pop(
				this evolib.Databases.common1.OpsNotice rec,
				ChatDesc chatDesc
		)
		{
			rec.Pop((CommonDesc)chatDesc);

			chatDesc.times = rec.times;
			chatDesc.repeatedIntervalMinutes = rec.repeateIntervalMinutes;

			return chatDesc;
		}

		public static PopupDesc Pop(
			this evolib.Databases.common1.OpsNotice rec,
			PopupDesc popupDesc
		)
		{
			rec.Pop((CommonDesc)popupDesc);

			popupDesc.titleEnglish = rec.titleEnglish;
			popupDesc.titleFrench = rec.titleFrench;
			popupDesc.titleGerman = rec.titleGerman;
			popupDesc.titleJapanese = rec.titleJapanese;

			return popupDesc;
		}
		public static TopicsDesc Pop(
			this evolib.Databases.common1.OpsNotice rec,
			TopicsDesc topicsDesc
		)
		{
			rec.Pop((CommonDesc)topicsDesc);

            topicsDesc.priority = rec.priority;
			topicsDesc.titleEnglish = rec.titleEnglish;
			topicsDesc.titleFrench = rec.titleFrench;
			topicsDesc.titleGerman = rec.titleGerman;
			topicsDesc.titleJapanese = rec.titleJapanese;
			topicsDesc.redirectUI = rec.redirectUI;

			return topicsDesc;
		}
	}

	public class ChatNotice
	{
		[Required]
		public long id { get; set; }

		public ChatDesc desc { get; set; }

		public static explicit operator ChatNotice(evolib.Databases.common1.OpsNotice rec)
		{
			return new ChatNotice
			{
				id = rec.Id,
				desc = rec.Pop(new ChatDesc()),
			};
		}
	}
	public class PopupNotice
	{
		[Required]
		public long id { get; set; }

		public PopupDesc desc { get; set; }

		public static explicit operator PopupNotice(evolib.Databases.common1.OpsNotice rec)
		{
			return new PopupNotice
			{
				id = rec.Id,
				desc = rec.Pop(new PopupDesc()),
			};
		}
	}
	public class TopicsNotice
	{
		[Required]
		public long id { get; set; }

		public TopicsDesc desc { get; set; }

		public static explicit operator TopicsNotice(evolib.Databases.common1.OpsNotice rec)
		{
			return new TopicsNotice
			{
				id = rec.Id,
				desc = rec.Pop(new TopicsDesc()),
			};
		}
	}
}

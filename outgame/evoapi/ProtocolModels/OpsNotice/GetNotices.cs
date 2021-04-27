using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evoapi.ProtocolModels.OpsNotice
{
    public class GetNotices
	{
		public class Request
		{
			[Required]
			public List<string> opsNoticeCodes { get; set; }
		}


		public class CommonNotice
		{
			public string noticeCode { get; set; }

			public UInt64? target { get; set; }


			public bool enabledEnglish { get; set; }
			public string msgEnglish { get; set; }

			public bool enabledFrench { get; set; }
			public string msgFrench { get; set; }

			public bool enabledGerman { get; set; }
			public string msgGerman { get; set; }

			public bool enabledJapanese { get; set; }
			public string msgJapanese { get; set; }
		}


		public class ChatNotice : CommonNotice
		{
			public int times { get; set; }

			public int repeatedIntervalMinutes { get; set; }
		}


		public class PopupNotice : CommonNotice
		{
			public string titleEnglish { get; set; }

			public string titleFrench { get; set; }

			public string titleGerman { get; set; }

			public string titleJapanese { get; set; }
		}


		public class TopicsNotice : CommonNotice
		{
            public int priority { get; set; }

			public string titleEnglish { get; set; }

			public string titleFrench { get; set; }

			public string titleGerman { get; set; }

			public string titleJapanese { get; set; }

            public string redirectUI;
		}

		public class Response
		{
			public List<ChatNotice> chatNotices { get; set; }
			public List<PopupNotice> popupNotices { get; set; }
			public List<TopicsNotice> topicsNotices { get; set; }
		}
	}

	static class OpsNoticeExtPop
	{
		static void Pop(
			this evolib.Databases.common1.OpsNotice rec,
			GetNotices.CommonNotice common
		)
		{
			common.target = rec.target;

			common.enabledEnglish = rec.enabledEnglish;
			common.msgEnglish = rec.msgEnglish;
			common.enabledFrench = rec.enabledFrench;
			common.msgFrench = rec.msgFrench;
			common.enabledGerman = rec.enabledGerman;
			common.msgGerman = rec.msgGerman;
			common.enabledJapanese = rec.enabledJapanese;
			common.msgJapanese = rec.msgJapanese;
		}

		public static GetNotices.ChatNotice Pop(
				this evolib.Databases.common1.OpsNotice rec,
				GetNotices.ChatNotice chat
		)
		{
			rec.Pop((GetNotices.CommonNotice)chat);

			chat.times = rec.times;
			chat.repeatedIntervalMinutes = rec.repeateIntervalMinutes;

			return chat;
		}

		public static GetNotices.PopupNotice Pop(
			this evolib.Databases.common1.OpsNotice rec,
			GetNotices.PopupNotice popup
		)
		{
			rec.Pop((GetNotices.CommonNotice)popup);

			popup.titleEnglish = rec.titleEnglish;
			popup.titleFrench = rec.titleFrench;
			popup.titleGerman = rec.titleGerman;
			popup.titleJapanese = rec.titleJapanese;

			return popup;
		}

		public static GetNotices.TopicsNotice Pop(
			this evolib.Databases.common1.OpsNotice rec,
			GetNotices.TopicsNotice topics
		)
		{
			rec.Pop((GetNotices.CommonNotice)topics);

            topics.priority = rec.priority;
			topics.titleEnglish = rec.titleEnglish;
			topics.titleFrench = rec.titleFrench;
			topics.titleGerman = rec.titleGerman;
			topics.titleJapanese = rec.titleJapanese;
            topics.redirectUI = rec.redirectUI;

			return topics;
		}
	}
}

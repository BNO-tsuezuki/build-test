using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace evolib.Util
{
	public static class NotSensitiveCharsRegex
	{
		public static string Joined { get; private set; }

		static Dictionary<string, string> ReplaceDictionary = new Dictionary<string, string>();


		public static void Init()
		{
			if (0 < ReplaceDictionary.Count) return;

			List.ForEach(set =>
			{
				if (!string.IsNullOrEmpty(Joined))
				{
					Joined += "|";
				}

				var replace = "(" + string.Join("|", set) + ")";
				Joined += replace;

				foreach (var c in set)
				{
					ReplaceDictionary[c] = replace;
				}
			});
		}

		public static string Replace(string src)
		{
			var esc = Regex.Replace(src, @"\-", (match) =>
			{
				return "ー";
			});

			esc = Regex.Replace(esc, @"\\|\*|\+|\.|\?|\{|\}|\(|\)|\[|\]|\^|\$|\-|\||\/", (match) =>
			{
				return @"\" + match.Value;
			});


			return Regex.Replace(esc, Joined, (match) =>
			{
				return ReplaceDictionary[match.Value];
			});
		}

		static List<string[]> List = new List<string[]>
		{
			new string[]{ "A","Ａ","a","ａ", },
			new string[]{ "B","Ｂ","b","ｂ", },
			new string[]{ "C","Ｃ","c","ｃ", },
			new string[]{ "D","Ｄ","d","ｄ", },
			new string[]{ "E","Ｅ","e","ｅ", },
			new string[]{ "F","Ｆ","f","ｆ", },
			new string[]{ "G","Ｇ","g","ｇ", },
			new string[]{ "H","Ｈ","h","ｈ", },
			new string[]{ "I","Ｉ","i","ｉ", },
			new string[]{ "J","Ｊ","j","ｊ", },
			new string[]{ "K","Ｋ","k","ｋ", },
			new string[]{ "L","Ｌ","l","ｌ", },
			new string[]{ "M","Ｍ","m","ｍ", },
			new string[]{ "N","Ｎ","n","ｎ", },
			new string[]{ "O","Ｏ","o","ｏ", },
			new string[]{ "P","Ｐ","p","ｐ", },
			new string[]{ "Q","Ｑ","q","ｑ", },
			new string[]{ "R","Ｒ","r","ｒ", },
			new string[]{ "S","Ｓ","s","ｓ", },
			new string[]{ "T","Ｔ","t","ｔ", },
			new string[]{ "U","Ｕ","u","ｕ", },
			new string[]{ "V","Ｖ","v","ｖ", },
			new string[]{ "W","Ｗ","w","ｗ", },
			new string[]{ "X","Ｘ","x","ｘ", },
			new string[]{ "Y","Ｙ","y","ｙ", },
			new string[]{ "Z","Ｚ","z","ｚ", },
			new string[]{ "０","0", },
			new string[]{ "１","1", },
			new string[]{ "２","2", },
			new string[]{ "３","3", },
			new string[]{ "４","4", },
			new string[]{ "５","5", },
			new string[]{ "６","6", },
			new string[]{ "７","7", },
			new string[]{ "８","8", },
			new string[]{ "９","9", },
			new string[]{ "ガ","カﾞ","ｶﾞ","カ゛","ｶ゛","が","かﾞ","か゛", },
			new string[]{ "ギ","キﾞ","ｷﾞ","キ゛","ｷ゛","ぎ","きﾞ","き゛", },
			new string[]{ "グ","クﾞ","ｸﾞ","ク゛","ｸ゛","ぐ","くﾞ","く゛", },
			new string[]{ "ゲ","ケﾞ","ｹﾞ","ケ゛","ｹ゛","げ","けﾞ","け゛", },
			new string[]{ "ゴ","コﾞ","ｺﾞ","コ゛","ｺ゛","ご","こﾞ","こ゛", },
			new string[]{ "ザ","サﾞ","ｻﾞ","サ゛","ｻ゛","ざ","さﾞ","さ゛", },
			new string[]{ "ジ","シﾞ","ｼﾞ","シ゛","ｼ゛","じ","しﾞ","し゛", },
			new string[]{ "ズ","スﾞ","ｽﾞ","ス゛","ｽ゛","ず","すﾞ","す゛", },
			new string[]{ "ゼ","セﾞ","ｾﾞ","セ゛","ｾ゛","ぜ","せﾞ","せ゛", },
			new string[]{ "ゾ","ソﾞ","ｿﾞ","ソ゛","ｿ゛","ぞ","そﾞ","そ゛", },
			new string[]{ "ダ","タﾞ","ﾀﾞ","タ゛","ﾀ゛","だ","たﾞ","た゛", },
			new string[]{ "ヂ","チﾞ","ﾁﾞ","チ゛","ﾁ゛","ぢ","ちﾞ","ち゛", },
			new string[]{ "ヅ","ツﾞ","ﾂﾞ","ツ゛","ﾂ゛","づ","つﾞ","つ゛", },
			new string[]{ "デ","テﾞ","ﾃﾞ","テ゛","ﾃ゛","で","てﾞ","て゛", },
			new string[]{ "ド","トﾞ","ﾄﾞ","ト゛","ﾄ゛","ど","とﾞ","と゛", },
			new string[]{ "バ","ハﾞ","ﾊﾞ","ハ゛","ﾊ゛","ば","はﾞ","は゛", },
			new string[]{ "ビ","ヒﾞ","ﾋﾞ","ヒ゛","ﾋ゛","び","ひﾞ","ひ゛", },
			new string[]{ "ブ","フﾞ","ﾌﾞ","フ゛","ﾌ゛","ぶ","ふﾞ","ふ゛", },
			new string[]{ "ベ","ヘﾞ","ﾍﾞ","ヘ゛","ﾍ゛","べ","へﾞ","へ゛", },
			new string[]{ "ボ","ホﾞ","ﾎﾞ","ホ゛","ﾎ゛","ぼ","ほﾞ","ほ゛", },
			new string[]{ "パ","ハﾟ","ﾊﾟ","ハ゜","ﾊ゜","ぱ","はﾟ","は゜", },
			new string[]{ "ピ","ヒﾟ","ﾋﾟ","ヒ゜","ﾋ゜","ぴ","ひﾟ","ひ゜", },
			new string[]{ "プ","フﾟ","ﾌﾟ","フ゜","ﾌ゜","ぷ","ふﾟ","ふ゜", },
			new string[]{ "ペ","ヘﾟ","ﾍﾟ","ヘ゜","ﾍ゜","ぺ","へﾟ","へ゜", },
			new string[]{ "ポ","ホﾟ","ﾎﾟ","ホ゜","ﾎ゜","ぽ","ほﾟ","ほ゜", },
			new string[]{ "ヴ","ウﾞ","ｳﾞ","ウ゛","ｳ゛","ゔ","うﾞ","う゛", },
			new string[]{ "ヷ","ワﾞ","ﾜﾞ","ワ゛","ﾜ゛","わ゙","わﾞ","わ゛", },
			new string[]{ "ヺ","ヲﾞ","ｦﾞ","ヲ゛","ｦ゛","を゙","をﾞ","を゛", },
			new string[]{ "ア","ｱ","あ", },
			new string[]{ "イ","ｲ","い", },
			new string[]{ "ウ","ｳ","う", },
			new string[]{ "エ","ｴ","え", },
			new string[]{ "オ","ｵ","お", },
			new string[]{ "カ","ｶ","か", },
			new string[]{ "キ","ｷ","き", },
			new string[]{ "ク","ｸ","く", },
			new string[]{ "ケ","ｹ","け", },
			new string[]{ "コ","ｺ","こ", },
			new string[]{ "サ","ｻ","さ", },
			new string[]{ "シ","ｼ","し", },
			new string[]{ "ス","ｽ","す", },
			new string[]{ "セ","ｾ","せ", },
			new string[]{ "ソ","ｿ","そ", },
			new string[]{ "タ","ﾀ","た", },
			new string[]{ "チ","ﾁ","ち", },
			new string[]{ "ツ","ﾂ","つ", },
			new string[]{ "テ","ﾃ","て", },
			new string[]{ "ト","ﾄ","と", },
			new string[]{ "ナ","ﾅ","な", },
			new string[]{ "ニ","ﾆ","に", },
			new string[]{ "ヌ","ﾇ","ぬ", },
			new string[]{ "ネ","ﾈ","ね", },
			new string[]{ "ノ","ﾉ","の", },
			new string[]{ "ハ","ﾊ","は", },
			new string[]{ "ヒ","ﾋ","ひ", },
			new string[]{ "フ","ﾌ","ふ", },
			new string[]{ "ヘ","ﾍ","へ", },
			new string[]{ "ホ","ﾎ","ほ", },
			new string[]{ "マ","ﾏ","ま", },
			new string[]{ "ミ","ﾐ","み", },
			new string[]{ "ム","ﾑ","む", },
			new string[]{ "メ","ﾒ","め", },
			new string[]{ "モ","ﾓ","も", },
			new string[]{ "ヤ","ﾔ","や", },
			new string[]{ "ユ","ﾕ","ゆ", },
			new string[]{ "ヨ","ﾖ","よ", },
			new string[]{ "ラ","ﾗ","ら", },
			new string[]{ "リ","ﾘ","り", },
			new string[]{ "ル","ﾙ","る", },
			new string[]{ "レ","ﾚ","れ", },
			new string[]{ "ロ","ﾛ","ろ", },
			new string[]{ "ワ","ﾜ","わ", },
			new string[]{ "ヲ","ｦ","を", },
			new string[]{ "ン","ﾝ","ん", },
			new string[]{ "ァ","ｧ","ぁ", },
			new string[]{ "ィ","ｨ","ぃ", },
			new string[]{ "ゥ","ｩ","ぅ", },
			new string[]{ "ェ","ｪ","ぇ", },
			new string[]{ "ォ","ｫ","ぉ", },
			new string[]{ "ヵ","ゕ", },
			new string[]{ "ヶ","ゖ",  },
			new string[]{ "ッ","ｯ","っ", },
			new string[]{ "ャ","ｬ","ゃ", },
			new string[]{ "ュ","ｭ","ゅ", },
			new string[]{ "ョ","ｮ","ょ", },
			new string[]{ "ヮ","ゎ", },
			new string[]{ "ー","-","ｰ","―", },
			new string[]{ " ","　", },
		};
	}
}

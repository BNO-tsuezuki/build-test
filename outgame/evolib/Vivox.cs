using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;


namespace evolib
{
    public static class Vivox
    {
        public static string issuer { get; set; }
        public static string endPoint { get; set; }
        public static string domain { get; set; }

        public class Claims
        {
            public string iss { get; set; }
            public int exp { get; set; }
            public string vxa { get; set; }
            public int vxi { get; set; }
            public string f { get; set; }
            public string t { get; set; }
            public string sub { get; set; }
        }

        // Vivoxのアクセストークンを生成する
        // 安全なサーバー上でアクセストークンを生成し、それらのアクセストークンをゲームクライアントに送信する必要があるらしい
        // Vivoxデベロッパーポータル https://developer.vivox.com/apps 
        public static string VivoxGenerateToken(string key, string issuer, int exp, string vxa, int vxi, string f, string t)
        {
            var claims = new Claims
            {
                iss = issuer,
                exp = exp,
                vxa = vxa,
                vxi = vxi,
                f = f,
                t = t
            };

            List<string> segments = new List<string>();

            // ヘッダーは静的です
            var header = Base64URLEncode("{}");
            segments.Add(header);

            // ペイロードをエンコードします
            var claimsString = JsonConvert.SerializeObject(claims);
            var encodedClaims = Base64URLEncode(claimsString);

            // 署名の準備のためにセグメントを結合します
            segments.Add(encodedClaims);
            string toSign = string.Join(".", segments);

            // キーと SHA256 を使用してトークンに署名します
            string sig = SHA256Hash(key, toSign);
            segments.Add(sig);

            // 「.」を使用してトークンの 3 つの部分をすべて結合し、トークンを返します
            string token = string.Join(".", segments);

            return token;
        }

        private static string Base64URLEncode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            // 最後にパディングを削除します
            var encodedString = Convert.ToBase64String(plainTextBytes).TrimEnd('=');
            // URL セーフな文字に置き換えます
            string urlEncoded = encodedString.Replace("+", "-").Replace("/", "_");

            return urlEncoded;
        }

        private static string SHA256Hash(string secret, string message)
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                var hashString = Convert.ToBase64String(hashmessage).TrimEnd('=');
                string urlEncoded = hashString.Replace("+", "-").Replace("/", "_");

                return urlEncoded;
            }
        }
    }
}

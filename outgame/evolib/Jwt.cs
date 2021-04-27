using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using Microsoft.IdentityModel.Tokens;


namespace evolib
{
	public class Jwt<Payload> where Payload:new()
	{
		public static string issuer { get; set; }
		public static string audience { get; set; }
		public static int expiryMinutes { get; set; }

		public static string  Build( Payload payload, string signingKey)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new List<Claim>();
			foreach (var prop in typeof(Payload).GetProperties())
			{
				var obj = prop.GetValue(payload);
				claims.Add(new Claim(prop.Name, Convert.ToString(obj)));
			}
			var token = new JwtSecurityToken(
				issuer: issuer,
				audience: audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public static Payload Extract(string token)
		{
			var securityToken = new JwtSecurityToken(token);

			var payload = new Payload();

			foreach (var prop in typeof(Payload).GetProperties())
			{
				var strValue = securityToken.Claims.Single(c => c.Type == prop.Name).Value;

				prop.SetValue( payload, Util.TypeConverter.Convert(prop.PropertyType, strValue) );
			}

			return payload;
		}

		static TokenValidationParameters ValidationParameters( string signingKey)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));

			return new TokenValidationParameters
			{
				ValidateIssuer				= true,
				ValidateAudience			= true,
				ValidateIssuerSigningKey	= true,
				ValidateLifetime			= false,
				ValidateActor				= false,	//?

				ValidIssuer = issuer,
				ValidAudience = audience,
				IssuerSigningKey = securityKey,
			};
		}

		public static bool IsAuthenticated( string token, string signingKey )
		{
			SecurityToken outSecurityToken;
			var ret = new JwtSecurityTokenHandler().ValidateToken(
						token,
						ValidationParameters(signingKey),
						out outSecurityToken
			);

			return ret.Identity.IsAuthenticated;
		}

	}
}

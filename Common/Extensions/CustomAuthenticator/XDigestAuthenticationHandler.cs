using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

namespace AlifTask.Common.Extensions.CustomAuthenticator
{
	public class XDigestAuthenticationHandler : AuthenticationHandler<XDigestAuthenticationOptions>
	{
		private const string HmacSha1SecretKey = "O2RlrAeKKsW9S3TSo55mIfacNzo=";

		public XDigestAuthenticationHandler(
			IOptionsMonitor<XDigestAuthenticationOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder)
			: base(options, logger, encoder)
		{
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.TryGetValue(ConstValues.XDigestHeaderName, out var digestValue))
			{
				return AuthenticateResult.Fail("X-Digest header not found.");
			}

			var isValid = await ValidateDigest(digestValue);

			if (!isValid)
			{
				return AuthenticateResult.Fail("Invalid X-Digest value.");
			}

			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, "User")
			};

			var identity = new ClaimsIdentity(claims, Scheme.Name);
			var principal = new ClaimsPrincipal(identity);
			var ticket = new AuthenticationTicket(principal, Scheme.Name);

			return AuthenticateResult.Success(ticket);
		}

		private async Task<bool> ValidateDigest(string? digest)
		{
			if (!string.IsNullOrEmpty(digest))
			{
				Request.EnableBuffering();
				var body = await new StreamReader(Request.Body).ReadToEndAsync();
				Request.Body.Position = 0;

				var computedDigest = CalculateHMACSHA1(body, HmacSha1SecretKey);

				if (computedDigest.Equals(digest, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}

			return false;
		}

		private string CalculateHMACSHA1(string text, string key)
		{
			using (var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(key)))
			{
				byte[] hashBytes = hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(text));
				return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
			}
		}
	}
}

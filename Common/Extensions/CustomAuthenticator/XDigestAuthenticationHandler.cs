using AlifTask.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
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

		private class UserInfoDto
		{
			public Guid Id { get; set; }
			public string UserName { get; set; }
			public bool IsWalletVerified { get; set; }
		}

		private readonly AppDbContext db;

		public XDigestAuthenticationHandler(
			IOptionsMonitor<XDigestAuthenticationOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			AppDbContext db)
			: base(options, logger, encoder)
		{
			this.db = db;
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.TryGetValue(ConstValues.XDigestHeaderName, out var digestValue))
			{
				return AuthenticateResult.Fail("X-Digest header not found.");
			}

			var isValid = await ValidateDigestAsync(digestValue);

			if (!isValid)
			{
				return AuthenticateResult.Fail("Invalid X-Digest value.");
			}

			if (!Request.Headers.TryGetValue(ConstValues.XUserIdHeaderName, out var userId))
			{
				return AuthenticateResult.Fail("X-UserId header not found.");
			}

			var user = await FindUserAsync(userId);

			if (user == null)
			{
				return AuthenticateResult.Fail("Invalid X-UserId value.");
			}

			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ConstValues.WalletVerifiedStatusClaimName, user.IsWalletVerified.ToString()),
			};

			var identity = new ClaimsIdentity(claims, Scheme.Name);
			var principal = new ClaimsPrincipal(identity);
			var ticket = new AuthenticationTicket(principal, Scheme.Name);

			return AuthenticateResult.Success(ticket);
		}

		private async Task<bool> ValidateDigestAsync(string? digest)
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

		private async Task<UserInfoDto?> FindUserAsync(string? userIdAsTxt)
		{
			if (!Guid.TryParse(userIdAsTxt, out Guid userId))
				return null;

			return await db.Users.AsNoTracking()
									.Select(i => new UserInfoDto
									{
										Id = i.Id,
										UserName = i.Username,
										IsWalletVerified = i.Wallet.IsVerified
									})
									.FirstOrDefaultAsync(i => i.Id == userId);
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

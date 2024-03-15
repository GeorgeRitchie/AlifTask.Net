using Microsoft.AspNetCore.Authentication;

namespace AlifTask.Common.Extensions.CustomAuthenticator
{
	public class XDigestAuthenticationOptions : AuthenticationSchemeOptions
	{
		public static XDigestAuthenticationOptions Default() => new XDigestAuthenticationOptions();
	}
}

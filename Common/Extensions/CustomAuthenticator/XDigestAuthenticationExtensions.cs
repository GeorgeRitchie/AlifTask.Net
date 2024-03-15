using Microsoft.AspNetCore.Authentication;

namespace AlifTask.Common.Extensions.CustomAuthenticator
{
	public static class XDigestAuthenticationExtensions
	{
		public static AuthenticationBuilder AddXDigest(
			this AuthenticationBuilder builder,
			string authenticationScheme,
			Action<XDigestAuthenticationOptions> configureOptions)
		{
			return builder.AddScheme<XDigestAuthenticationOptions, XDigestAuthenticationHandler>(
				authenticationScheme, configureOptions);
		}

		public static AuthenticationBuilder AddXDigest(
			this AuthenticationBuilder builder,
			string authenticationScheme)
		{
			return builder.AddScheme<XDigestAuthenticationOptions, XDigestAuthenticationHandler>(
				authenticationScheme, null);
		}

		public static AuthenticationBuilder AddXDigest(
			this AuthenticationBuilder builder)
		{
			return builder.AddScheme<XDigestAuthenticationOptions, XDigestAuthenticationHandler>(
				ConstValues.CustomXDigestAuthenticationSchemeName, null);
		}
	}
}

namespace AlifTask.Common.Helpers
{
	public sealed class ConstValues
	{
		public const string CustomXDigestAuthenticationSchemeName = "X-Digest";
		public const string XDigestHeaderName = "X-Digest";
		public const string XUserIdHeaderName = "X-UserId";
		public const string WalletVerifiedStatusClaimName = "WalletStatus";
		public const decimal MaxAllowedBalanceForVerifiedWallet = 100000;
		public const decimal MaxAllowedBalanceForUnverifiedWallet = 10000;
	}
}

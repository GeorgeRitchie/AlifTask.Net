namespace AlifTask.Models
{
	public class WalletInfoDto
	{
		public Guid Id { get; set; }
		public bool IsVerified { get; set; }
		public string OwnerFirstName { get; set; }
		public string OwnerLastName { get; set; }
	}
}

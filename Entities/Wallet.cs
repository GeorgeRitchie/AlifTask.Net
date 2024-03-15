namespace AlifTask.Entities
{
	public class Wallet
	{
		public Guid Id { get; set; }
		public bool IsVerified { get; set; }
		public decimal Balance { get; set; }

		public Guid OwnerId { get; set; }
		public User Owner { get; set; }
		public List<Operation> Incomes { get; set; }
		public List<Operation> Outcomes { get; set; }
	}
}

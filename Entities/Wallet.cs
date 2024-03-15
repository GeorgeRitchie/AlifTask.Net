namespace AlifTask.Entities
{
	public class Wallet
	{
		public Guid Id { get; set; }
		public bool IsVerified { get; set; }
		public decimal Balance { get; set; }

		public User Owner { get; set; }
		public List<Income> Incomes { get; set; }
		public List<Outcome> Outcomes { get; set; }
	}
}

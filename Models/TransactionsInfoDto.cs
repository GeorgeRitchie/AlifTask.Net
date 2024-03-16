namespace AlifTask.Models
{
	public class TransactionsInfoDto
	{
		public Guid WalletId { get; set; }
		public int TotalIncomesCount { get; set; }
		public int TotalOutcomesCount { get; set; }
		public decimal TotalIncomesSum { get; set; }
		public decimal TotalOutcomesSum { get; set; }
	}
}

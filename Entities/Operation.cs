namespace AlifTask.Entities
{
	public class Operation
	{
		public Guid Id { get; set; }
		public DateTime DateTime { get; set; }
		public decimal Amount { get; set; }
		public OperationStatus Status { get; set; }

		public OperationType Type { get; set; }

		public Guid? FromId { get; set; }
		public Wallet? From { get; set; }
		public Guid? ToId { get; set; }
		public Wallet? To { get; set; }
	}
}

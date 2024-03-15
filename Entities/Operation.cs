namespace AlifTask.Entities
{
	public abstract class Operation
	{
		public Guid Id { get; set; }
		public DateTime DateTime { get; set; }
		public decimal Amount { get; set; }
		public OperationStatus Status { get; set; }

		public abstract OperationType Type { get; }

		public Wallet From { get; set; }
		public Wallet To { get; set; }
	}
}

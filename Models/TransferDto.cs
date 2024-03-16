using AlifTask.Entities;

namespace AlifTask.Models
{
	public class TransferDto
	{
		public decimal Amount { get; set; }
		public OperationType OperationType { get; set; }

		public Guid? FromId { get; set; }
		public Guid? ToId { get; set; }
	}
}

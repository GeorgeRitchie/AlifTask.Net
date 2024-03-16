using AlifTask.Entities;
using AlifTask.Persistence;
using FluentValidation;

namespace AlifTask.Models
{
	public class TransferDtoValidator : AbstractValidator<TransferDto>
	{
		public TransferDtoValidator(AppDbContext db)
		{
			RuleFor(i => i.OperationType).IsInEnum();

			When(i => i.OperationType == OperationType.WalletToWalletTransfer, () =>
			{
				RuleFor(i => i.Amount).InclusiveBetween(10, 3000);
				
				RuleFor(i => i.FromId).Cascade(CascadeMode.Stop).NotEmpty().Custom((fromId, context) =>
				{
					if (db.Wallets.Any(i => i.Id == fromId) == false)
						context.AddFailure($"Wallet with Id '{fromId}' not found.");
				});

				RuleFor(i=>i.ToId).Cascade(CascadeMode.Stop).NotEmpty().Custom((toId, context) =>
				{
					if (db.Wallets.Any(i => i.Id == toId) == false)
						context.AddFailure($"Wallet with Id '{toId}' not found.");
				});
			});

			When(i => i.OperationType == OperationType.Income, () =>
			{
				RuleFor(i => i.Amount).InclusiveBetween(1, 5000);

				RuleFor(i=>i.ToId).Cascade(CascadeMode.Stop).NotEmpty().Custom((toId, context) =>
				{
					if (db.Wallets.Any(i => i.Id == toId) == false)
						context.AddFailure($"Wallet with Id '{toId}' not found.");
				});
			});

			When(i => i.OperationType == OperationType.Outcome, () =>
			{
				RuleFor(i => i.Amount).GreaterThanOrEqualTo(10);

				RuleFor(i => i.FromId).Cascade(CascadeMode.Stop).NotEmpty().Custom((fromId, context) =>
				{
					if (db.Wallets.Any(i => i.Id == fromId) == false)
						context.AddFailure($"Wallet with Id '{fromId}' not found.");
				});
			});
		}
	}
}

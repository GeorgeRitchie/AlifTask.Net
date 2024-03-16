using AlifTask.Persistence;
using FluentValidation;

namespace AlifTask.Models
{
	public class WalletIdDtoValidator : AbstractValidator<WalletIdDto>
	{
		public WalletIdDtoValidator(AppDbContext db)
		{
			RuleFor(i => i.Id).Custom((id, context) =>
			{
				if (db.Wallets.Any(s => s.Id == id) == false)
					context.AddFailure($"Wallet with Id '{id}' not found.");
			});
		}
	}
}

using AlifTask.Entities;
using AlifTask.Models;
using AlifTask.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AlifTask.Services
{
	public class WalletOperationService
	{
		private readonly AppDbContext db;

		public WalletOperationService(AppDbContext db)
		{
			this.db = db;
		}

		public async Task<Result<Guid>> AddWalletToWalletTransactionAsync(TransferDto dto, CancellationToken cancellationToken = default)
		{
			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				var fromWallet = await db.Wallets.FirstAsync(i => i.Id == dto.FromId, cancellationToken);
				var toWallet = await db.Wallets.FirstAsync(i => i.Id == dto.ToId, cancellationToken);

				decimal maxAllowedBalance = toWallet.IsVerified
													? ConstValues.MaxAllowedBalanceForVerifiedWallet
													: ConstValues.MaxAllowedBalanceForUnverifiedWallet;

				if (fromWallet.Balance - dto.Amount < 0)
				{
					await transaction.RollbackAsync(cancellationToken);
					return Result.Failure(Guid.Empty, [new("", "Source wallet does not have enough money.")]);
				}

				if (toWallet.Balance + dto.Amount > maxAllowedBalance)
				{
					await transaction.RollbackAsync(cancellationToken);
					return Result.Failure(Guid.Empty, [new("", $"Transaction will make target wallet to exceed max allowed balance {maxAllowedBalance}.")]);
				}

				fromWallet.Balance -= dto.Amount;
				toWallet.Balance += dto.Amount;

				var operation = new Operation()
				{
					Id = Guid.NewGuid(),
					DateTime = DateTime.UtcNow,
					Amount = dto.Amount,
					FromId = dto.FromId,
					ToId = dto.ToId,
					Type = OperationType.WalletToWalletTransfer,
					Status = OperationStatus.Success,
				};

				db.Operations.Add(operation);

				await db.SaveChangesAsync(cancellationToken);
				await transaction.CommitAsync(cancellationToken);

				return Result.Success(operation.Id);
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync(cancellationToken);
				Log.Error("Something went wrong while adding WTW transaction: {@ex}", ex);

				return Result.Failure(Guid.Empty, [new("", ex.Message)]);
			}
		}

		public async Task<Result<Guid>> AddIncomeToWalletTransactionAsync(TransferDto dto, CancellationToken cancellationToken = default)
		{
			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				var toWallet = await db.Wallets.FirstAsync(i => i.Id == dto.ToId, cancellationToken);

				decimal maxAllowedBalance = toWallet.IsVerified
													? ConstValues.MaxAllowedBalanceForVerifiedWallet
													: ConstValues.MaxAllowedBalanceForUnverifiedWallet;

				if (toWallet.Balance + dto.Amount > maxAllowedBalance)
				{
					await transaction.RollbackAsync(cancellationToken);
					return Result.Failure(Guid.Empty, [new("", $"Transaction will make target wallet to exceed max allowed balance {maxAllowedBalance}.")]);
				}

				toWallet.Balance += dto.Amount;

				var operation = new Operation()
				{
					Id = Guid.NewGuid(),
					DateTime = DateTime.UtcNow,
					Amount = dto.Amount,
					ToId = dto.ToId,
					Type = OperationType.Income,
					Status = OperationStatus.Success,
				};

				db.Operations.Add(operation);

				await db.SaveChangesAsync(cancellationToken);
				await transaction.CommitAsync(cancellationToken);

				return Result.Success(operation.Id);
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync(cancellationToken);
				Log.Error("Something went wrong while adding ITW transaction: {@ex}", ex);

				return Result.Failure(Guid.Empty, [new("", ex.Message)]);
			}
		}

		public async Task<Result<Guid>> AddOutcomeToWalletTransactionAsync(TransferDto dto, CancellationToken cancellationToken = default)
		{
			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				var fromWallet = await db.Wallets.FirstAsync(i => i.Id == dto.FromId, cancellationToken);

				if (fromWallet.Balance - dto.Amount < 0)
				{
					await transaction.RollbackAsync(cancellationToken);
					return Result.Failure(Guid.Empty, [new("", "Source wallet does not have enough money.")]);
				}

				fromWallet.Balance -= dto.Amount;

				var operation = new Operation()
				{
					Id = Guid.NewGuid(),
					DateTime = DateTime.UtcNow,
					Amount = dto.Amount,
					FromId = dto.FromId,
					Type = OperationType.Outcome,
					Status = OperationStatus.Success,
				};

				db.Operations.Add(operation);

				await db.SaveChangesAsync(cancellationToken);
				await transaction.CommitAsync(cancellationToken);

				return Result.Success(operation.Id);
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync(cancellationToken);
				Log.Error("Something went wrong while adding OTW transaction: {@ex}", ex);

				return Result.Failure(Guid.Empty, [new("", ex.Message)]);
			}
		}
	}
}

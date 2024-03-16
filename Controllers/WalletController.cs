using AlifTask.Entities;
using AlifTask.Models;
using AlifTask.Persistence;
using AlifTask.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlifTask.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class WalletController : ControllerBase
	{
		// TODO __!!__ Here and in other places I used different ways to show several ways of practices
		// here i could move logic of GetBalanceAsync and DoesWalletExist to WalletOperationService.cs
		private readonly AppDbContext db;
		private readonly WalletOperationService walletOperationService;

		public WalletController(AppDbContext db, WalletOperationService walletOperationService)
		{
			this.db = db;
			this.walletOperationService = walletOperationService;
		}

		[HttpGet]
		public async Task<IActionResult> GetBalanceAsync([FromQuery] Guid id)
		{
			var wallet = await db.Wallets.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

			if (wallet == null)
				return Problem(statusCode: 404, title: $"Wallet with Id '{id}' not found.");

			return Ok(wallet.Balance);
		}

		[HttpGet]
		public async Task<IActionResult> DoesWalletExist([FromQuery] Guid id)
		{
			var wallet = await db.Wallets.AsNoTracking()
											.Select(i => new WalletInfoDto
											{
												Id = i.Id,
												IsVerified = i.IsVerified,
												OwnerFirstName = i.Owner.FirstName,
												OwnerLastName = i.Owner.LastName,
											})
											.FirstOrDefaultAsync(i => i.Id == id);

			if (wallet == null)
				return Problem(statusCode: 404, title: $"Wallet with Id '{id}' not found.");

			return Ok(wallet);
		}

		[HttpPost]
		public async Task<IActionResult> CreateTransaction([FromBody] TransferDto dto)
		{
			var result = dto.OperationType switch
			{
				OperationType.WalletToWalletTransfer => await walletOperationService.AddWalletToWalletTransactionAsync(dto),
				OperationType.Income => await walletOperationService.AddIncomeToWalletTransactionAsync(dto),
				OperationType.Outcome => await walletOperationService.AddOutcomeToWalletTransactionAsync(dto),
				_ => throw new NotImplementedException($"Unknow operation type detected: {dto.OperationType}"),
			};

			if (result.IsSuccess)
				return Ok(result.Value);
			else
				return BadRequest(result.Errors);
		}

		[HttpGet]
		public async Task<TransactionsInfoDto> GetOperationsInfoForCurrentMonth([FromQuery] WalletIdDto walletId)
		{
			return await walletOperationService.GetWalletOperationsOfCurrentMonth(walletId.Id);
		}
	}
}

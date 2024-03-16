using AlifTask.Models;
using AlifTask.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlifTask.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class WalletController : ControllerBase
	{
		private readonly AppDbContext db;

		public WalletController(AppDbContext db)
		{
			this.db = db;
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
											.FirstOrDefaultAsync(i=>i.Id == id);

			if (wallet == null)
				return Problem(statusCode: 404, title: $"Wallet with Id '{id}' not found.");

			return Ok(wallet);
		}
	}
}

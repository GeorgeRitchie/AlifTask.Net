using AlifTask.Entities;

namespace AlifTask.Persistence
{
	public static class DbInitializer
	{
		public static async Task InitializeAsync(AppDbContext db, CancellationToken cancellationToken = default)
		{
			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				if(db.Users.Any(i=>i.Username == "tom") == false)
				{
					var user = new User
					{
						Id = Guid.NewGuid(),
						Username = "tom",
						FirstName = "Tom",
						LastName = "Cat",
						Wallet = new Wallet()
						{
							Id = Guid.NewGuid(),
							Balance = 8442,
							IsVerified = false,
						},
					};

					db.Users.Add(user);
				}

				if(db.Users.Any(i=>i.Username == "jerry") == false)
				{
					var user = new User
					{
						Id = Guid.NewGuid(),
						Username = "jerry",
						FirstName = "Jerry",
						LastName = "Mouse",
						Wallet = new Wallet()
						{
							Id = Guid.NewGuid(),
							Balance = 9562,
							IsVerified = false,
						},
					};

					db.Users.Add(user);
				}

				if(db.Users.Any(i=>i.Username == "geralt") == false)
				{
					var user = new User
					{
						Id = Guid.NewGuid(),
						Username = "geralt",
						FirstName = "Geralt",
						LastName = "Rivia",
						Wallet = new Wallet()
						{
							Id = Guid.NewGuid(),
							Balance = 1423,
							IsVerified = false,
						},
					};

					db.Users.Add(user);
				}

				if(db.Users.Any(i=>i.Username == "yennefer") == false)
				{
					var user = new User
					{
						Id = Guid.NewGuid(),
						Username = "yennefer",
						FirstName = "Yennefer",
						LastName = "Vengerberg",
						Wallet = new Wallet()
						{
							Id = Guid.NewGuid(),
							Balance = 58462,
							IsVerified = true,
						},
					};

					db.Users.Add(user);
				}

				if(db.Users.Any(i=>i.Username == "zireael") == false)
				{
					var user = new User
					{
						Id = Guid.NewGuid(),
						Username = "zireael",
						FirstName = "Zireael",
						LastName = "Ciri",
						Wallet = new Wallet()
						{
							Id = Guid.NewGuid(),
							Balance = 100000,
							IsVerified = true,
						},
					};

					db.Users.Add(user);
				}

				await db.SaveChangesAsync(cancellationToken);
				await transaction.CommitAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync(cancellationToken);
				throw;
			}
		}
	}
}

using AlifTask.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlifTask.Persistence.Configurations
{
	public class OperationConfigurations : IEntityTypeConfiguration<Operation>
	{
		public void Configure(EntityTypeBuilder<Operation> builder)
		{
			builder.HasOne(i => i.From).WithMany(i => i.Outcomes).HasForeignKey(i => i.FromId).OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(i => i.To).WithMany(i => i.Incomes).HasForeignKey(i => i.ToId).OnDelete(DeleteBehavior.Restrict);
		}
	}
}

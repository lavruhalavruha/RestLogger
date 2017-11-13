using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using RestLogger.Domain;

namespace RestLogger.Storage.Configurations
{
    internal class ApplicationConfiguration : EntityTypeConfiguration<ApplicationEntity>
    {
        public ApplicationConfiguration()
        {
            ToTable("application");

            HasKey(x => x.Id);

            Property(x => x.Id)
                .HasColumnName("application_id")
                .HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.DisplayName)
                .HasColumnName("display_name")
                .HasColumnType("nvarchar")
                .HasMaxLength(32)
                .IsUnicode(true)
                .IsRequired();

            Property(x => x.PasswordHash)
                .HasColumnName("password_hash")
                .HasColumnType("varchar")
                .IsUnicode(false)
                .IsRequired();

            Property(x => x.PasswordSalt)
                .HasColumnName("password_salt")
                .HasColumnType("varchar")
                .IsUnicode(false)
                .IsRequired();

            HasMany(x => x.Logs)
                .WithRequired(x => x.Application)
                .HasForeignKey(x => x.ApplicationId)
                .WillCascadeOnDelete(true);
        }
    }
}

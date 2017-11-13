using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using RestLogger.Domain;

namespace RestLogger.Storage.Configurations
{
    internal class LogConfiguration : EntityTypeConfiguration<LogEntity>
    {
        public LogConfiguration()
        {
            ToTable("log");

            HasKey(x => x.Id);

            Property(x => x.Id)
                .HasColumnName("log_id")
                .HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.Logger)
                .HasColumnName("logger")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsUnicode(true)
                .IsRequired();

            Property(x => x.Level)
                .HasColumnName("level")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsUnicode(true)
                .IsRequired();

            Property(x => x.Message)
                .HasColumnName("message")
                .HasColumnType("nvarchar")
                .HasMaxLength(2048)
                .IsUnicode(true)
                .IsRequired();

            Property(x => x.ApplicationId)
                .HasColumnName("application_id")
                .HasColumnType("int")
                .IsRequired();

            HasRequired(x => x.Application)
                .WithMany(x => x.Logs)
                .HasForeignKey(x => x.ApplicationId)
                .WillCascadeOnDelete(true);
        }
    }
}

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CommunicationModule.Data.Models;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CommunicationModule.Data.Repositories;

public class CommunicationDbContext : DbContextBase
{
    public CommunicationDbContext(DbContextOptions<CommunicationDbContext> options)
        : base(options)
    {
    }

    protected CommunicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CommunicationUserEntity>().ToTable("CommunicationUser").HasKey(x => x.Id);
        modelBuilder.Entity<CommunicationUserEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();

        modelBuilder.Entity<MessageEntity>().ToTable("Message").HasKey(x => x.Id);
        modelBuilder.Entity<MessageEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
        modelBuilder.Entity<MessageEntity>().HasOne(x => x.Sender).WithMany()
            .HasForeignKey(x => x.SenderId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MessageAttachmentEntity>().ToTable("MessageAttachment").HasKey(x => x.Id);
        modelBuilder.Entity<MessageAttachmentEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
        modelBuilder.Entity<MessageAttachmentEntity>().HasOne(x => x.Message).WithMany(x => x.Attachments)
            .HasForeignKey(x => x.MessageId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MessageRecipientEntity>().ToTable("MessageRecipient").HasKey(x => x.Id);
        modelBuilder.Entity<MessageRecipientEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
        modelBuilder.Entity<MessageRecipientEntity>().HasOne(x => x.Message).WithMany(x => x.Recipients)
            .HasForeignKey(x => x.MessageId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<MessageRecipientEntity>().HasOne(x => x.Recipient).WithMany()
            .HasForeignKey(x => x.RecipientId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MessageReactionEntity>().ToTable("MessageReaction").HasKey(x => x.Id);
        modelBuilder.Entity<MessageReactionEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
        modelBuilder.Entity<MessageReactionEntity>().HasOne(x => x.Message).WithMany(x => x.Reactions)
            .HasForeignKey(x => x.MessageId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<MessageReactionEntity>().HasOne(x => x.User).WithMany()
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

        switch (Database.ProviderName)
        {
            case "Pomelo.EntityFrameworkCore.MySql":
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.CommunicationModule.Data.MySql"));
                break;
            case "Npgsql.EntityFrameworkCore.PostgreSQL":
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.CommunicationModule.Data.PostgreSql"));
                break;
            case "Microsoft.EntityFrameworkCore.SqlServer":
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.CommunicationModule.Data.SqlServer"));
                break;
        }
    }
}

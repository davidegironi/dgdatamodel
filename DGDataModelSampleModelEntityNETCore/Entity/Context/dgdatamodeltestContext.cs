using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DG.DataModelSample.Model.Entity.Models;

#nullable disable

namespace DG.DataModelSample.Model.Entity.Context
{
    public partial class dgdatamodeltestContext : DbContext
    {
        public dgdatamodeltestContext()
        {
        }

        public dgdatamodeltestContext(DbContextOptions<dgdatamodeltestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<blogs> blogs { get; set; }
        public virtual DbSet<comments> comments { get; set; }
        public virtual DbSet<footertext> footertext { get; set; }
        public virtual DbSet<footertextdesc> footertextdesc { get; set; }
        public virtual DbSet<posts> posts { get; set; }
        public virtual DbSet<poststotags> poststotags { get; set; }
        public virtual DbSet<tags> tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { if (optionsBuilder.IsConfigured == false) optionsBuilder.UseSqlServer(System.Configuration.ConfigurationManager.AppSettings["dgdatamodeltestConnectionString"]); }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<blogs>(entity =>
            {
                entity.HasKey(e => e.blogs_id);

                entity.Property(e => e.blogs_description)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.blogs_title)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<comments>(entity =>
            {
                entity.HasKey(e => e.comments_id);

                entity.Property(e => e.comments_email)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.comments_text)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.comments_username)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.HasOne(d => d.posts)
                    .WithMany(p => p.comments)
                    .HasForeignKey(d => d.posts_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_comments_posts");
            });

            modelBuilder.Entity<footertext>(entity =>
            {
                entity.HasKey(e => e.footertext_id);

                entity.Property(e => e.footertext_title)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<footertextdesc>(entity =>
            {
                entity.HasKey(e => e.footertext_id);

                entity.Property(e => e.footertext_id).ValueGeneratedNever();

                entity.Property(e => e.footertext_desc).HasColumnType("text");

                entity.HasOne(d => d.footertext)
                    .WithOne(p => p.footertextdesc)
                    .HasForeignKey<footertextdesc>(d => d.footertext_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_footertextdesc_footertext");
            });

            modelBuilder.Entity<posts>(entity =>
            {
                entity.HasKey(e => e.posts_id);

                entity.Property(e => e.posts_email)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.posts_text)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.posts_title)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.posts_username)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.HasOne(d => d.blogs)
                    .WithMany(p => p.posts)
                    .HasForeignKey(d => d.blogs_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_posts_blogs");
            });

            modelBuilder.Entity<poststotags>(entity =>
            {
                entity.HasKey(e => new { e.posts_id, e.tags_id });

                entity.Property(e => e.poststotags_comments)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.HasOne(d => d.posts)
                    .WithMany(p => p.poststotags)
                    .HasForeignKey(d => d.posts_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_poststotags_posts");

                entity.HasOne(d => d.tags)
                    .WithMany(p => p.poststotags)
                    .HasForeignKey(d => d.tags_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_poststotags_tags");
            });

            modelBuilder.Entity<tags>(entity =>
            {
                entity.HasKey(e => e.tags_id);

                entity.Property(e => e.tags_name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

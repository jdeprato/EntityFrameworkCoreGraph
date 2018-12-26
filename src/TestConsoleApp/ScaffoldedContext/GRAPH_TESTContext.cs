using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TestConsoleApp.ScaffoldedContext
{
    public partial class GRAPH_TESTContext : DbContext
    {
        public GRAPH_TESTContext()
        {
        }

        public GRAPH_TESTContext(DbContextOptions<GRAPH_TESTContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Edge1> Edge1 { get; set; }
        public virtual DbSet<Node1> Node1 { get; set; }
        public virtual DbSet<Node2> Node2 { get; set; }
        public virtual DbSet<Node3> Node3 { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQL2019;Database=GRAPH_TEST;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-t000");

            modelBuilder.Entity<Edge1>(entity =>
            {
                entity.HasAnnotation("SqlServer:GraphEdge", true);

                entity.HasIndex(e => e.EdgeId)
                    .HasName("ix_graphid")
                    .IsUnique();

                entity.HasIndex(e => new { e.FromId, e.ToId })
                    .HasName("ix_fromid");

                entity.HasIndex(e => new { e.ToId, e.FromId })
                    .HasName("ix_toid");

                entity.Property(e => e.EdgeId)
                    .IsRequired()
                    .HasColumnName("$edge_id")
                    .HasMaxLength(1000)
                    .HasAnnotation("SqlServer:GraphEdge", true)
                    .HasAnnotation("SqlServer:PseudoColumn", true);

                entity.Property(e => e.FromId)
                    .HasColumnName("$from_id")
                    .HasMaxLength(1000)
                    .HasAnnotation("SqlServer:PseudoColumn", true);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ToId)
                    .HasColumnName("$to_id")
                    .HasMaxLength(1000)
                    .HasAnnotation("SqlServer:PseudoColumn", true);
            });

            modelBuilder.Entity<Node1>(entity =>
            {
                entity.HasAnnotation("SqlServer:GraphNode", true);

                entity.HasIndex(e => e.NodeId)
                    .HasName("ix_graphid")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NodeId)
                    .IsRequired()
                    .HasColumnName("$node_id")
                    .HasMaxLength(1000)
                    .HasAnnotation("SqlServer:GraphNode", true)
                    .HasAnnotation("SqlServer:PseudoColumn", true);
            });

            modelBuilder.Entity<Node2>(entity =>
            {
                entity.HasAnnotation("SqlServer:GraphNode", true);

                entity.HasIndex(e => e.NodeId)
                    .HasName("ix_graphid")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NodeId)
                    .IsRequired()
                    .HasColumnName("$node_id")
                    .HasMaxLength(1000)
                    .HasAnnotation("SqlServer:GraphNode", true)
                    .HasAnnotation("SqlServer:PseudoColumn", true);
            });

            modelBuilder.Entity<Node3>(entity =>
            {
                entity.HasAnnotation("SqlServer:GraphNode", true);

                entity.HasIndex(e => e.NodeId)
                    .HasName("ix_graphid")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NodeId)
                    .IsRequired()
                    .HasColumnName("$node_id")
                    .HasMaxLength(1000)
                    .HasAnnotation("SqlServer:GraphNode", true)
                    .HasAnnotation("SqlServer:PseudoColumn", true);
            });
        }
    }
}

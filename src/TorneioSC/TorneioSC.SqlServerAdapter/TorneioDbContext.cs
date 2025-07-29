using Microsoft.EntityFrameworkCore;
using TorneioSC.SqlServerAdapter.Entities;

namespace TorneioSC.SqlServerAdapter
{
    public class TorneioDbContext: DbContext
    {
        public TorneioDbContext(DbContextOptions<TorneioDbContext> options)
        : base(options)
        {
        }
        // Tabelas principais
        public DbSet<Academia> Academias { get; set; }
        public DbSet<AcademiaTorneio> AcademiaTorneios { get; set; }
        public DbSet<Atleta> Atletas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Certificado> Certificados { get; set; }
        public DbSet<Chaveamento> Chaveamentos { get; set; }
        public DbSet<EquipePontuacao> EquipePontuacoes { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<EstatisticaPosEvento> EstatisticasPosEvento { get; set; }
        public DbSet<EstatisticaPreEvento> EstatisticasPreEvento { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<EventoTorneio> EventoTorneios { get; set; }
        public DbSet<Federacao> Federacoes { get; set; }
        public DbSet<Inscricao> Inscricoes { get; set; }
        public DbSet<Modalidade> Modalidades { get; set; }
        public DbSet<Resultado> Resultados { get; set; }
        public DbSet<Torneio> Torneios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações de relacionamentos e propriedades
            modelBuilder.Entity<Academia>(entity =>
            {
                entity.HasOne(a => a.Federacao)
                    .WithMany()
                    .HasForeignKey(a => a.FederacaoID)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(a => a.Estado)
                    .WithMany()
                    .HasForeignKey(a => a.EstadoID)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<AcademiaTorneio>(entity =>
            {
                entity.HasOne(at => at.Academia)
                    .WithMany()
                    .HasForeignKey(at => at.AcademiaID)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(at => at.Torneio)
                    .WithMany()
                    .HasForeignKey(at => at.TorneioID)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Atleta>(entity =>
            {
                entity.HasOne(a => a.Academia)
                    .WithMany()
                    .HasForeignKey(a => a.AcademiaID)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Certificado>(entity =>
            {
                entity.HasOne(c => c.Inscricao)
                    .WithMany()
                    .HasForeignKey(c => c.InscricaoID)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Chaveamento>(entity =>
            {
                entity.HasOne(c => c.Torneio)
                    .WithMany()
                    .HasForeignKey(c => c.TorneioID)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(c => c.Categoria)
                    .WithMany()
                    .HasForeignKey(c => c.CategoriaID)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(c => c.Modalidade)
                    .WithMany()
                    .HasForeignKey(c => c.ModalidadeID)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<EquipePontuacao>(entity =>
            {
                entity.HasOne(ep => ep.Academia)
                    .WithMany()
                    .HasForeignKey(ep => ep.AcademiaID)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(ep => ep.Torneio)
                    .WithMany()
                    .HasForeignKey(ep => ep.TorneioID)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<EventoTorneio>(entity =>
            {
                entity.HasOne(et => et.Evento)
                    .WithMany()
                    .HasForeignKey(et => et.EventoID)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(et => et.Torneio)
                    .WithMany()
                    .HasForeignKey(et => et.TorneioID)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Federacao>(entity =>
            {
                entity.HasOne(f => f.Estado)
                    .WithMany()
                    .HasForeignKey(f => f.EstadoID)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Inscricao>(entity =>
            {
                entity.HasOne(i => i.Atleta)
                    .WithMany()
                    .HasForeignKey(i => i.AtletaID)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(i => i.Torneio)
                    .WithMany()
                    .HasForeignKey(i => i.TorneioID)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(i => i.Categoria)
                    .WithMany()
                    .HasForeignKey(i => i.CategoriaID)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(i => i.Modalidade)
                    .WithMany()
                    .HasForeignKey(i => i.ModalidadeID)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Resultado>(entity =>
            {
                entity.HasOne(r => r.Inscricao)
                    .WithMany()
                    .HasForeignKey(r => r.InscricaoID)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Torneio>(entity =>
            {
                entity.HasOne(t => t.Estado)
                    .WithMany()
                    .HasForeignKey(t => t.EstadoID)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Configurações de valores padrão
            modelBuilder.Entity<AcademiaTorneio>()
                .Property(a => a.Ativo)
                .HasDefaultValue(true);

            modelBuilder.Entity<AcademiaTorneio>()
                .Property(a => a.DataInclusao)
                .HasDefaultValueSql("getdate()");

            // Adicione configurações semelhantes para outras entidades conforme necessário
            // seguindo o padrão das constraints DEFAULT do seu script SQL

            base.OnModelCreating(modelBuilder);
        }
    }
}
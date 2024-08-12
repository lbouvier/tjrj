using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Text;
using TJRJ.Entities;
using TJRJ.Repository.Mapping;
using TJRJ.ViewModels;
using TJRJ.Web.Repository.Mapping;
using TJRJ.Web.ViewModels;

namespace TJRJ.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Livro> Livros { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Assunto> Assuntos { get; set; }
        public DbSet<LivroAutor> LivroAutores { get; set; }
        public DbSet<LivroAssunto> LivroAssuntos { get; set; }
        public DbSet<RelatorioViewModel> RelatorioViewModel { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            var connectionString = "Server=localhost;Port=3306;Database=tjrj;Uid=root;Pwd=123456";
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var server = ServerVersion.AutoDetect(connectionString).Version;
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            modelBuilder.Entity<Autor>(new AutorMap().Configure);
            modelBuilder.Entity<Livro>(new LivroMap().Configure);
            modelBuilder.Entity<LivroAutor>(new LivroAutorMap().Configure);
            modelBuilder.Entity<LivroAssunto>(new LivroAssuntoMap().Configure);
            modelBuilder.Entity<Assunto>(new AssuntoMap().Configure);
            modelBuilder.Entity<RelatorioViewModel>(new RelatorioMap().Configure);

        }
    }
}


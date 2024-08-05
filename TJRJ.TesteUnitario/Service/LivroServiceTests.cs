using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TJRJ.Entities;
using TJRJ.Repository;
using TJRJ.Services;
using TJRJ.Web.Data;

namespace TJRJ.TesteUnitario.Service
{
    public class LivroServiceTests
    {
        private ApplicationDbContext _context;
        private Repository<Livro> _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            _context = new ApplicationDbContext(options);
            _repository = new Repository<Livro>(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddAsync_ShouldAddEntity()
        {
            var livro = new Livro { Cod = 1, Titulo = "Test Book", Editora = "Test Publisher", Edicao = 1, AnoPublicacao = "2021" };

            await _repository.AddAsync(livro);
            var result = await _repository.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(livro.Cod, result.Cod);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            var livro1 = new Livro { Cod = 1, Titulo = "Test Book 1", Editora = "Test Publisher 1", Edicao = 1, AnoPublicacao = "2021" };
            var livro2 = new Livro { Cod = 2, Titulo = "Test Book 2", Editora = "Test Publisher 2", Edicao = 2, AnoPublicacao = "2022" };

            await _repository.AddAsync(livro1);
            await _repository.AddAsync(livro2);
            var result = await _repository.GetAllAsync();

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            var livro = new Livro { Cod = 1, Titulo = "Test Book", Editora = "Test Publisher", Edicao = 1, AnoPublicacao = "2021" };
            await _repository.AddAsync(livro);

            var result = await _repository.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(livro.Cod, result.Cod);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            var livro = new Livro { Cod = 1, Titulo = "Test Book", Editora = "Test Publisher", Edicao = 1, AnoPublicacao = "2021" };
            await _repository.AddAsync(livro);

            livro.Titulo = "Updated Test Book";
            await _repository.UpdateAsync(livro);
            var result = await _repository.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Test Book", result.Titulo);
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteEntity()
        {
            var livro = new Livro { Cod = 1, Titulo = "Test Book", Editora = "Test Publisher", Edicao = 1, AnoPublicacao = "2021" };
            await _repository.AddAsync(livro);

            await _repository.DeleteAsync(1);
            var result = await _repository.GetByIdAsync(1);

            Assert.IsNull(result);
        }
    }
}

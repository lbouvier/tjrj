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
    public class AutorServiceTests
    {
        private ApplicationDbContext _context;
        private Repository<Autor> _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            _context = new ApplicationDbContext(options);
            _repository = new Repository<Autor>(_context);
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
            var autor = new Autor { CodAu = 1, Nome = "Test Author" };

            await _repository.AddAsync(autor);
            var result = await _repository.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(autor.CodAu, result.CodAu);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            var autor1 = new Autor { CodAu = 1, Nome = "Test Author 1" };
            var autor2 = new Autor { CodAu = 2, Nome = "Test Author 2" };

            await _repository.AddAsync(autor1);
            await _repository.AddAsync(autor2);
            var result = await _repository.GetAllAsync();

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            var autor = new Autor { CodAu = 1, Nome = "Test Author" };
            await _repository.AddAsync(autor);

            var result = await _repository.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(autor.CodAu, result.CodAu);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            var autor = new Autor { CodAu = 1, Nome = "Test Author" };
            await _repository.AddAsync(autor);

            autor.Nome = "Updated Name";
            await _repository.UpdateAsync(autor);
            var result = await _repository.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Name", autor.Nome);
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteEntity()
        {
            var autor = new Autor { CodAu = 1, Nome = "Test Author" };
            await _repository.AddAsync(autor);

            await _repository.DeleteAsync(1);
            var result = await _repository.GetByIdAsync(1);

            Assert.IsNull(result);
        }
    }
}

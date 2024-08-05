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
    public class AssuntoServiceTests
    {
        private ApplicationDbContext _context;
        private Repository<Assunto> _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            _context = new ApplicationDbContext(options);
            _repository = new Repository<Assunto>(_context);
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
            var assunto = new Assunto { CodAs = 1, Descricao = "Assunto" };

            await _repository.AddAsync(assunto);
            var result = await _repository.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(assunto.CodAs, result.CodAs);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            var assunto1 = new Assunto { CodAs = 1, Descricao = "Assunto 1" };
            var assunto2 = new Assunto { CodAs = 2, Descricao = "Assunto 2" };

            await _repository.AddAsync(assunto1);
            await _repository.AddAsync(assunto2);
            var result = await _repository.GetAllAsync();

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            var Assunto = new Assunto { CodAs = 1, Descricao = "Assunto" };
            await _repository.AddAsync(Assunto);

            var result = await _repository.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(Assunto.CodAs, result.CodAs);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            var Assunto = new Assunto { CodAs = 1, Descricao = "Assunto" };
            await _repository.AddAsync(Assunto);

            Assunto.Descricao = "Updated";
            await _repository.UpdateAsync(Assunto);
            var result = await _repository.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Updated", Assunto.Descricao);
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteEntity()
        {
            var Assunto = new Assunto { CodAs = 1, Descricao = "Assunto" };
            await _repository.AddAsync(Assunto);

            await _repository.DeleteAsync(1);
            var result = await _repository.GetByIdAsync(1);

            Assert.IsNull(result);
        }
    }
}

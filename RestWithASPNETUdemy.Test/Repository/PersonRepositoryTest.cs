using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Repository;
using RestWithASPNETUdemy.Repository.Implementation;
using System;

namespace RestWithASPNETUdemy.Test.Repository
{
    [TestClass]
    public class PersonRepositoryTest
    {
        private IRepository<Person> _personRepository;
        private const int TOTAL_DATABASE_REGISTER = 10;

        private async Task<MySQLContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<MySQLContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new MySQLContext(options);

            databaseContext.Database.EnsureCreated();

            // ensure database has registers
            if (await databaseContext.Persons.CountAsync() <= 0)
            {
                foreach (int i in Enumerable.Range(1, TOTAL_DATABASE_REGISTER))
                {
                    databaseContext.Persons.Add(GetPersonMock(i));
                    await databaseContext.SaveChangesAsync();
                }
            }

            return databaseContext;
        }
        private static Person GetPersonMock(int id)
        {
            return new()
            {
                Address = "Address test",
                FirstName = "FirstName test",
                Gender = new Random().NextDouble() < 0.5 ? "Female" : "Male",
                Id = id,
                LastName = "LastName test"
            };
        }

        [TestInitialize]
        public async Task Init()
        {
            _personRepository = new PersonRepository(await GetDbContext());
        }

        [TestMethod]
        public void Create_ReturnCreatedPerson()
        {
            //Arrange
            Person person = GetPersonMock(99);
            //Act
            var result = _personRepository.Create(person);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(person);
        }

        [TestMethod]
        public async Task FindById_ReturnPersonWithFindId()
        {
            //Arrange
            int findId = 1;
            //Act
            var result = _personRepository.FindById(findId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Person));
            result.Should().Match<Person>(r => r.Id == findId);
        }

        [TestMethod]
        public async Task Delete_ShouldRemoveFromDatabase()
        {
            //Arrange
            int id = 1;
            var person = _personRepository.FindById(id);

            //Act            
            _personRepository.Delete(id);

            //Assert
            var registers = _personRepository.FindAll();

            registers.Should().NotContain(person);
        }

        public async Task FindAll_ReturnAllDBRegisters()
        {
            //Arrange

            //Act            
            var result = _personRepository.FindAll();

            //Assert
            result.Should().HaveCount(TOTAL_DATABASE_REGISTER);
        }

        [TestMethod]
        public async Task Update_ReturnTheUpdatedPerson()
        {
            //Arrange
            var person = GetPersonMock(1);
            person.FirstName = "Tester Mario";

            //Act            
            var result = _personRepository.Update(person);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Person));
            result.Should().BeSameAs(person);
        }

    }
}
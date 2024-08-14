using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Repository.Implementation;
using RestWithASPNETUdemy.Services;
using RestWithASPNETUdemy.Services.Implementations;
using System;

namespace RestWithASPNETUdemy.Test.Services
{
    [TestClass]
    public class PersonServiceTest
    {
        private IPersonService _personService;

        [TestInitialize]
        public void Init()
        {
            _personService = A.Fake<IPersonService>();
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


        [TestMethod]
        public void Create_ReturnCreatedPerson()
        {
            //Arrange
            var person = GetPersonMock(1);

            //Act
            A.CallTo(() => _personService.Create(person)).Returns(person);
            var result = _personService.Create(person);

            //Assert
            result.Should().BeSameAs(person);
        }
    }
}
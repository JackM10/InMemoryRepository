using System.Linq;
using System;
using System.Collections.Generic;
using InMemoryRepositoryTests;
using NUnit.Framework;
using Interview;

namespace InMemoryRepository.Tests
{
    [TestFixture]
    public class PersonInMemoryTests
    {
        [Test]
        public void Add_RepoContains1Person_PersonAddedToRepo()
        {
            var testObject = new PersonTestableObject();
            
            testObject.InMemoryCarRepository.Save(testObject.PersonJack);

            Assert.AreEqual(testObject.PersonJack, testObject.InMemoryCarRepository.FindById(testObject.PersonJack.Id));
        }

        [Test]
        public void GetAll_Add2PersonsToRepo_RepoAllReturn2Persons()
        {
            var testObject = new PersonTestableObject();
            
            testObject.InMemoryCarRepository.Save(testObject.PersonJack);
            testObject.InMemoryCarRepository.Save(testObject.PersonRobert);
            
            Assert.AreEqual(2, testObject.InMemoryCarRepository.All().Count());
            Assert.AreEqual(testObject.PersonJack, testObject.InMemoryCarRepository.FindById(testObject.PersonJack.Id));
            Assert.AreEqual(testObject.PersonRobert, testObject.InMemoryCarRepository.FindById(testObject.PersonRobert.Id));
        }

        [Test]
        public void Find_RepoContainsAddedPerson_AddedPersonReturned()
        {
            var testObject = new PersonTestableObject();
            testObject.InMemoryCarRepository.Save(testObject.PersonJack);
            testObject.InMemoryCarRepository.Save(testObject.PersonRobert);
            
            var targetElement = testObject.InMemoryCarRepository.FindById(testObject.PersonJack.Id);
            var emptyElement = testObject.InMemoryCarRepository.FindById(String.Empty);

            Assert.NotNull(targetElement);
            Assert.AreEqual(new Person(), emptyElement);
        }

        [Test]
        public void Update_RepoContainsUpdateCar_UpdatedCarReturned()
        {
            var testObject = new PersonTestableObject();
            testObject.InMemoryCarRepository.Save(testObject.PersonJack);
            testObject.InMemoryCarRepository.Save(testObject.PersonRobert);
            
            var targetElement = testObject.InMemoryCarRepository.FindById(testObject.PersonJack.Id);
            var NameOfNewPerson = "Ben";
            targetElement.FirstName = NameOfNewPerson;
            testObject.InMemoryCarRepository.Save(targetElement);

            Assert.AreEqual(NameOfNewPerson, testObject.InMemoryCarRepository.FindById(testObject.PersonJack.Id).FirstName);
        }

        [Test]
        public void Delete_RepoNotContainsAddedPerson_TargetPersonRemoved()
        {
            var testObject = new PersonTestableObject();
            testObject.InMemoryCarRepository.Save(testObject.PersonJack);
            testObject.InMemoryCarRepository.Save(testObject.PersonRobert);

            testObject.InMemoryCarRepository.Delete(testObject.PersonJack.Id);
            
            Assert.AreEqual(testObject.InMemoryCarRepository.FindById(testObject.PersonJack.Id), new Person());
            Assert.AreEqual(1, testObject.InMemoryCarRepository.All().Count());
        }

        [Test]
        public void Update_RepoDoesntContainsPersonWithSameId_OldValueUpdated()
        {
            InMemoryRepository<Person> InMemoryCarRepository = new InMemoryRepository<Person>();
            var commonId = "user#0";
            var PersonJack = new Person { PersonId = commonId, FirstName = "Jack", LastName = "Poladich", Age = 29 };
            var PersonRobert = new Person { PersonId = commonId, FirstName = "Robert", LastName = "Martin", Age = 53 };

            InMemoryCarRepository.Save(PersonJack);
            InMemoryCarRepository.Save(PersonRobert);

            Assert.AreEqual("Robert", InMemoryCarRepository.FindById(commonId).FirstName);
            Assert.AreEqual(PersonRobert.Id, InMemoryCarRepository.FindById(commonId).Id);
        }
    }

    class PersonTestableObject
    {
        public InMemoryRepository<Person> InMemoryCarRepository { get; set; }
        public Person PersonJack { get; set; }
        public Person PersonRobert { get; set; }

        public PersonTestableObject()
        {
            InMemoryCarRepository = new InMemoryRepository<Person>();
            PersonJack = new Person { PersonId = "user#0", FirstName = "Jack", LastName = "Poladich", Age = 29 };
            PersonRobert = new Person { PersonId = "user#1", FirstName = "Robert", LastName = "Martin", Age = 53 };
        }
    }
}
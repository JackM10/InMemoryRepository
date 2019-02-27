using System.Linq;
using System;
using System.Collections.Generic;
using InMemoryRepositoryTests;
using NUnit.Framework;
using Interview;

namespace InMemoryRepository.Tests
{
    [TestFixture]
    public class CarInMemoryTests
    {
        [Test]
        public void Add_RepoContains1Car_CarAddedToRepo()
        {
            var testObject = new CarTestableObject();

            testObject.InMemoryCarRepository.Save(testObject.CarBMW);
            
            Assert.AreEqual(testObject.CarBMW, testObject.InMemoryCarRepository.FindById(testObject.CarBMW.Id));
        }

        [Test]
        public void GetAll_Add2CarsToRepo_RepoAllReturn2Cars()
        {
            var testObject = new CarTestableObject();
            
            testObject.InMemoryCarRepository.Save(testObject.CarBMW);
            testObject.InMemoryCarRepository.Save(testObject.CarMG);
            
            Assert.AreEqual(2, testObject.InMemoryCarRepository.All().Count());
            Assert.AreEqual(testObject.CarBMW, testObject.InMemoryCarRepository.FindById(testObject.CarBMW.Id));
            Assert.AreEqual(testObject.CarMG, testObject.InMemoryCarRepository.FindById(testObject.CarMG.Id));
        }
        
        [Test]
        public void Find_RepoContainsAddedCar_AddedCarReturned()
        {
            var testObject = new CarTestableObject();
            testObject.InMemoryCarRepository.Save(testObject.CarBMW);
            testObject.InMemoryCarRepository.Save(testObject.CarMG);
            
            var targetElement = testObject.InMemoryCarRepository.FindById(testObject.CarBMW.Id);
            var emptyElement = testObject.InMemoryCarRepository.FindById(Guid.Empty);

            Assert.NotNull(targetElement);
            Assert.AreEqual(new Car(),emptyElement);
        }

        [Test]
        public void Update_RepoContainsUpdateCar_UpdatedCarReturned()
        {
            var testObject = new CarTestableObject();
            testObject.InMemoryCarRepository.Save(testObject.CarBMW);
            testObject.InMemoryCarRepository.Save(testObject.CarMG);
            
            var targetElement = testObject.InMemoryCarRepository.FindById(testObject.CarBMW.Id);
            var NameOfNewCar = "LADA";
            targetElement.Name = NameOfNewCar;
            testObject.InMemoryCarRepository.Save(targetElement);

            Assert.AreEqual(NameOfNewCar, testObject.InMemoryCarRepository.FindById(testObject.CarBMW.Id).Name);
        }


        [Test]
        public void Delete_RepoNotContainsAddedCar_TargetCarRemoved()
        {
            var testObject = new CarTestableObject();
            testObject.InMemoryCarRepository.Save(testObject.CarBMW);
            testObject.InMemoryCarRepository.Save(testObject.CarMG);

            testObject.InMemoryCarRepository.Delete(testObject.CarBMW.CarId);
            
            Assert.AreEqual(new Car(), testObject.InMemoryCarRepository.FindById(testObject.CarBMW.Id));
            Assert.AreEqual(1, testObject.InMemoryCarRepository.All().Count());
        }

        [Test]
        public void Update_RepoDoesntContainsCarWithSameId_OldValueUpdated()
        {
            InMemoryRepository<Car> InMemoryCarRepository = new InMemoryRepository<Car>();
            var commonId = Guid.NewGuid();
            var CarBMW = new Car { CarId = commonId, Name = "BMW", HP = 100 };
            var CarMG = new Car { CarId = commonId, Name = "MG", HP = 110 };

            InMemoryCarRepository.Save(CarBMW);
            InMemoryCarRepository.Save(CarMG);

            Assert.AreEqual("MG", InMemoryCarRepository.FindById(commonId).Name);
            Assert.AreEqual(CarMG.Id, InMemoryCarRepository.FindById(commonId).Id);
        }
    }

    class CarTestableObject
    {
        public InMemoryRepository<Car> InMemoryCarRepository { get; set; }
        public Car CarBMW { get; set; }
        public Car CarMG { get; set; }

        public CarTestableObject()
        {
            InMemoryCarRepository = new InMemoryRepository<Car>();
            CarBMW = new Car { CarId = Guid.NewGuid(), Name = "BMW", HP = 100 };
            CarMG = new Car { CarId = Guid.NewGuid(), Name = "MG", HP = 110 };
        }
    }
}
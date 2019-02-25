using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using InMemoryRepositoryTests;
using Interview;
using NUnit.Framework;

namespace InMemoryRepository.Tests
{
    public class PerformanceTests
    {
        private const double ADD_OP_PERFORMING_TIME_LIMIT = 500;
        private const int DELETE_OP_PERFORMING_TIME_LIMIT = 600;
        private const int FIND_OP_PERFORMING_TIME_LIMIT = 600;
        private const int MIXED_OP_PERFORMING_TIME_LIMIT = 14000;

        [Test]
        public void PerformaceBenchmark_AddOperation()
        {
            var timeSpendonPerformingOperation = BenchmarkRunner.Run<PerformanceTests>().Reports[0].ResultStatistics.Mean;
            
            Assert.True(timeSpendonPerformingOperation < ADD_OP_PERFORMING_TIME_LIMIT);
        }

        [Test]
        public void PerformaceBenchmark_FindOperation()
        {
            var timeSpendonPerformingOperation = BenchmarkRunner.Run<PerformanceTests>().Reports[1].ResultStatistics.Mean;

            Assert.True(timeSpendonPerformingOperation < FIND_OP_PERFORMING_TIME_LIMIT);
        }

        [Test]
        public void PerformaceBenchmark_DeleteOperation()
        {
            var timeSpendonPerformingOperation = BenchmarkRunner.Run<PerformanceTests>().Reports[2].ResultStatistics.Mean;

            Assert.True(timeSpendonPerformingOperation < DELETE_OP_PERFORMING_TIME_LIMIT);
        }

        [Test]
        public void PerformaceBenchmark_StressTest_MixedOperation()
        {
            var timeSpendonPerformingOperation = BenchmarkRunner.Run<PerformanceTests>().Reports[3].ResultStatistics.Mean;

            Assert.True(timeSpendonPerformingOperation < MIXED_OP_PERFORMING_TIME_LIMIT);
        }

        [Benchmark]
        public void PerformAddOperation()
        {
            var testObject = new PersonTestableObject();

            testObject.InMemoryCarRepository.Save(testObject.PersonJack);
        }

        [Benchmark]
        public void PerformFindOperation()
        {
            var testObject = new PersonTestableObject();
            testObject.InMemoryCarRepository.Save(testObject.PersonJack);
            testObject.InMemoryCarRepository.Save(testObject.PersonRobert);

            var result = testObject.InMemoryCarRepository.FindById(testObject.PersonRobert.Id);
        }

        [Benchmark]
        public void PerformDeleteOperation()
        {
            var testObject = new PersonTestableObject();
            testObject.InMemoryCarRepository.Save(testObject.PersonJack);
            testObject.InMemoryCarRepository.Save(testObject.PersonRobert);

            testObject.InMemoryCarRepository.Delete(testObject.PersonJack.Id);
        }

        [Benchmark]
        public void PerformMixedOperations()
        {
            Parallel.For(0, Environment.ProcessorCount, i =>
            {
                Task.Run(() => CRUDmix()).Wait();
            });
        }

        private void CRUDmix()
        {
            var testObject = new PersonTestableObject();
            testObject.InMemoryCarRepository.Save(testObject.PersonJack);
            testObject.InMemoryCarRepository.Save(testObject.PersonRobert);

            testObject.InMemoryCarRepository.Delete(testObject.PersonJack.Id);
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

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using InMemoryRepositoryTests;
using Interview;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryRepository.Tests
{
    public class PerformanceTests
    {
        #region consts

        private const int ADD_OP_PERFORMING_TIME_LIMIT = 500;
        private const int UPDATE_OP_PERFORMING_TIME_LIMIT = 500;
        private const int DELETE_OP_PERFORMING_TIME_LIMIT = 600;
        private const int FIND_OP_PERFORMING_TIME_LIMIT = 600;
        private const int MIXED_OP_PERFORMING_TIME_LIMIT = 14000;

        #endregion

        #region Tests

        [Test]
        public void PerformaceBenchmark_AddOperation()
        {
            var timeSpendonPerformingOperation = BenchmarkRunner.Run<PerformanceTests>().Reports[0].ResultStatistics.Mean;

            Assert.True(timeSpendonPerformingOperation < ADD_OP_PERFORMING_TIME_LIMIT);
        }

        [Test]
        public void PerformaceBenchmark_UpdateOperation()
        {
            var timeSpendonPerformingOperation = BenchmarkRunner.Run<PerformanceTests>().Reports[1].ResultStatistics.Mean;

            Assert.True(timeSpendonPerformingOperation < UPDATE_OP_PERFORMING_TIME_LIMIT);
        }

        [Test]
        public void PerformaceBenchmark_FindOperation()
        {
            var timeSpendonPerformingOperation = BenchmarkRunner.Run<PerformanceTests>().Reports[2].ResultStatistics.Mean;

            Assert.True(timeSpendonPerformingOperation < FIND_OP_PERFORMING_TIME_LIMIT);
        }

        [Test]
        public void PerformaceBenchmark_DeleteOperation()
        {
            var timeSpendonPerformingOperation = BenchmarkRunner.Run<PerformanceTests>().Reports[3].ResultStatistics.Mean;

            Assert.True(timeSpendonPerformingOperation < DELETE_OP_PERFORMING_TIME_LIMIT);
        }

        [Test]
        public void PerformaceBenchmark_StressTest_MixedOperation()
        {
            var timeSpendonPerformingOperation = BenchmarkRunner.Run<PerformanceTests>().Reports[4].ResultStatistics.Mean;

            Assert.True(timeSpendonPerformingOperation < MIXED_OP_PERFORMING_TIME_LIMIT);
        }

        #endregion
        
        #region micro-benchmarks (order is important)

        [Benchmark]
        public void PerformAddOperation()
        {
            var testObject = new PersonTestableObject();

            testObject.InMemoryCarRepository.Save(testObject.PersonJack);
        }

        [Benchmark]
        public void PerformUpdateOperation()
        {
            InMemoryRepository<Person> inMemoryCarRepository = new InMemoryRepository<Person>();
            var commonId = "user#0";
            var personJack = new Person { PersonId = commonId, FirstName = "Jack", LastName = "Poladich", Age = 29 };
            var personRobert = new Person { PersonId = commonId, FirstName = "Robert", LastName = "Martin", Age = 53 };

            inMemoryCarRepository.Save(personJack);
            inMemoryCarRepository.Save(personRobert);
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

        #endregion

        #region Helpers

        private void CRUDmix()
        {
            var testObject = new PersonTestableObject();
            testObject.InMemoryCarRepository.Save(testObject.PersonJack);
            testObject.InMemoryCarRepository.Save(testObject.PersonRobert);

            testObject.InMemoryCarRepository.Delete(testObject.PersonJack.Id);
        }

        #endregion
    }

}

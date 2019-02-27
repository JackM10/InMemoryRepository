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
        private const int UPDATE_OP_PERFORMING_TIME_LIMIT = 1100;
        private const int DELETE_OP_PERFORMING_TIME_LIMIT = 600;
        private const int FIND_OP_PERFORMING_TIME_LIMIT = 600;
        private const int MIXED_OP_PERFORMING_TIME_LIMIT = 14000;

        #endregion

        #region Tests

        private static double AddOperationResults = double.MaxValue;
        private static double UpdateOperationResults = double.MaxValue;
        private static double FindOperationResults = double.MaxValue;
        private static double DeleteOperationResults = double.MaxValue;
        private static double MixedOperationResults = double.MaxValue;

        [Test, Order(1)]
        public void PerformaceBenchmark_BenchmarkRunner()
        {
            var benchmarkResults = BenchmarkRunner.Run<PerformanceTests>();
            AddOperationResults = benchmarkResults.Reports[0].ResultStatistics.Mean;
            UpdateOperationResults = benchmarkResults.Reports[1].ResultStatistics.Mean;
            FindOperationResults = benchmarkResults.Reports[2].ResultStatistics.Mean;
            DeleteOperationResults = benchmarkResults.Reports[3].ResultStatistics.Mean;
            MixedOperationResults = benchmarkResults.Reports[4].ResultStatistics.Mean;

            Assert.True(!benchmarkResults.HasCriticalValidationErrors);
        }

        [Test]
        public void PerformaceBenchmark_AddOperation()
        {
            Assert.True(AddOperationResults < ADD_OP_PERFORMING_TIME_LIMIT);
        }

        [Test]
        public void PerformaceBenchmark_UpdateOperation()
        {
            Assert.True(UpdateOperationResults < UPDATE_OP_PERFORMING_TIME_LIMIT);
        }

        [Test]
        public void PerformaceBenchmark_FindOperation()
        {
            Assert.True(FindOperationResults < FIND_OP_PERFORMING_TIME_LIMIT);
        }

        [Test]
        public void PerformaceBenchmark_DeleteOperation()
        {
            Assert.True(DeleteOperationResults < DELETE_OP_PERFORMING_TIME_LIMIT);
        }

        [Test]
        public void PerformaceBenchmark_StressTest_MixedOperation()
        {
            Assert.True(MixedOperationResults < MIXED_OP_PERFORMING_TIME_LIMIT);
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

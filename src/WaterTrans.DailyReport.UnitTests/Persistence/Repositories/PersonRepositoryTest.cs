using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WaterTrans.DailyReport.Application.TableEntities;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Persistence.Repositories;

namespace WaterTrans.DailyReport.UnitTests.Persistence.Repositories
{
    [TestClass]
    public class PersonRepositoryTest
    {
        [TestMethod]
        public void Create_NotThrowsException_StateIsValid()
        {
            var now = DateUtil.Now;
            var person = new PersonTableEntity
            {
                PersonId = Guid.NewGuid(),
                PersonCode = new string('X', 20),
                Name = new string('X', 256),
                LoginId = new string('X', 256),
                Title = new string('X', 100),
                Description = new string('X', 1024),
                SortNo = int.MaxValue,
                Status = PersonStatus.NORMAL.ToString(),
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
            };
            var personRepository = new PersonRepository(TestEnvironment.DBSettings);
            personRepository.Create(person);
        }

        [TestMethod]
        public void Read_NotThrowsException_StateIsValid()
        {
            var personKey = new PersonTableEntity
            {
                PersonId = Guid.Parse("00000000-1001-0000-0000-000000000000"),
            };
            var personRepository = new PersonRepository(TestEnvironment.DBSettings);
            var person = personRepository.Read(personKey);

            Assert.IsNotNull(person);
        }

        [TestMethod]
        public void Update_NotThrowsException_StateIsValid()
        {
            var personKey = new PersonTableEntity
            {
                PersonId = Guid.Parse("00000000-1001-0000-0000-000000000000"),
            };
            var personRepository = new PersonRepository(TestEnvironment.DBSettings);
            var person = personRepository.Read(personKey);
            person.UpdateTime = DateUtil.Now;
            Assert.IsTrue(personRepository.Update(person));
        }

        [TestMethod]
        public void Delete_NotThrowsException_StateIsValid()
        {
            var now = DateUtil.Now;
            var person = new PersonTableEntity
            {
                PersonId = Guid.NewGuid(),
                PersonCode = new string('Y', 20),
                Name = new string('X', 256),
                LoginId = new string('Y', 256),
                Title = new string('X', 100),
                Description = new string('X', 1024),
                SortNo = int.MaxValue,
                Status = PersonStatus.SUSPENDED.ToString(),
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
            };
            var personRepository = new PersonRepository(TestEnvironment.DBSettings);
            personRepository.Create(person);
            Assert.IsTrue(personRepository.Delete(person));
        }
    }
}

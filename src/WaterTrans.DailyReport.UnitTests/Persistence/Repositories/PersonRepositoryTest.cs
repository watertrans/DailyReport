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
        public void Create_正常_例外が発生しないこと()
        {
            var now = DateUtil.Now;
            var person = new PersonTableEntity
            {
                PersonId = Guid.NewGuid(),
                PersonCode = new string('X', 20),
                Name = new string('X', 100),
                Title = new string('X', 100),
                Description = new string('X', 400),
                SortNo = int.MaxValue,
                Status = PersonStatus.NORMAL.ToString(),
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
                DeleteTime = null,
            };
            var personRepository = new PersonRepository(TestEnvironment.DBSettings);
            personRepository.Create(person);
        }

        [TestMethod]
        public void Read_正常_例外が発生しないこと()
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
        public void Update_正常_例外が発生しないこと()
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
        public void Delete_正常_例外が発生しないこと()
        {
            var now = DateUtil.Now;
            var person = new PersonTableEntity
            {
                PersonId = Guid.NewGuid(),
                PersonCode = new string('Y', 20),
                Name = new string('X', 100),
                Title = new string('X', 100),
                Description = new string('X', 400),
                SortNo = null,
                Status = PersonStatus.DELETED.ToString(),
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
                DeleteTime = DateTimeOffset.MaxValue,
            };
            var personRepository = new PersonRepository(TestEnvironment.DBSettings);
            personRepository.Create(person);
            Assert.IsTrue(personRepository.Delete(person));
        }
    }
}

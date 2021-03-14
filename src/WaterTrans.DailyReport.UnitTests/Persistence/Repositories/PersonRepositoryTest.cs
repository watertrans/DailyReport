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
                PersonCode = "Dummy001",
                Name = "Name",
                Title = "Tile",
                Description = "Description",
                SortNo = null,
                Status = PersonStatus.NORMAL.ToString(),
                CreateTime = now,
                UpdateTime = now,
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
                PersonCode = "Dummy002",
                Name = "Name",
                Title = "Tile",
                Description = "Description",
                SortNo = null,
                Status = PersonStatus.NORMAL.ToString(),
                CreateTime = now,
                UpdateTime = now,
                DeleteTime = null,
            };
            var personRepository = new PersonRepository(TestEnvironment.DBSettings);
            personRepository.Create(person);
            Assert.IsTrue(personRepository.Delete(person));
        }
    }
}

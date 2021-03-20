using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WaterTrans.DailyReport.Application.TableEntities;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Persistence.Repositories;

namespace WaterTrans.DailyReport.UnitTests.Persistence.Repositories
{
    [TestClass]
    public class TagRepositoryTest
    {
        [TestMethod]
        public void Create_����_��O���������Ȃ�����()
        {
            var now = DateUtil.Now;
            var tag = new TagTableEntity
            {
                TagId = Guid.NewGuid(),
                TargetId = Guid.NewGuid(),
                TargetTable = new string('X', 100),
                Value = new string('X', 100),
                CreateTime = DateTimeOffset.MaxValue,
            };
            var tagRepository = new TagRepository(TestEnvironment.DBSettings);
            tagRepository.Create(tag);
        }

        [TestMethod]
        public void Read_����_��O���������Ȃ�����()
        {
            var tagKey = new TagTableEntity
            {
                TagId = Guid.Parse("00000000-5001-0000-0000-000000000000"),
            };
            var tagRepository = new TagRepository(TestEnvironment.DBSettings);
            var tag = tagRepository.Read(tagKey);

            Assert.IsNotNull(tag);
        }

        [TestMethod]
        public void Update_����_��O���������Ȃ�����()
        {
            var tagKey = new TagTableEntity
            {
                TagId = Guid.Parse("00000000-5001-0000-0000-000000000000"),
            };
            var tagRepository = new TagRepository(TestEnvironment.DBSettings);
            var tag = tagRepository.Read(tagKey);
            tag.CreateTime = DateUtil.Now;
            Assert.IsTrue(tagRepository.Update(tag));
        }

        [TestMethod]
        public void Delete_����_��O���������Ȃ�����()
        {
            var tag = new TagTableEntity
            {
                TagId = Guid.NewGuid(),
                TargetId = Guid.NewGuid(),
                TargetTable = new string('X', 100),
                Value = new string('X', 100),
                CreateTime = DateTimeOffset.MaxValue,
            };
            var tagRepository = new TagRepository(TestEnvironment.DBSettings);
            tagRepository.Create(tag);
            Assert.IsTrue(tagRepository.Delete(tag));
        }
    }
}

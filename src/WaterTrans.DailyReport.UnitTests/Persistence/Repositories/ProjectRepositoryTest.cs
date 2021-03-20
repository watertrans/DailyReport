using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WaterTrans.DailyReport.Application.TableEntities;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Persistence.Repositories;

namespace WaterTrans.DailyReport.UnitTests.Persistence.Repositories
{
    [TestClass]
    public class ProjectRepositoryTest
    {
        [TestMethod]
        public void Create_正常_例外が発生しないこと()
        {
            var project = new ProjectTableEntity
            {
                ProjectId = Guid.NewGuid(),
                ProjectCode = new string('X', 20),
                Name = new string('X', 100),
                Description = new string('X', 400),
                SortNo = int.MaxValue,
                Status = ProjectStatus.NORMAL.ToString(),
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
                DeleteTime = null,
            };
            var projectRepository = new ProjectRepository(TestEnvironment.DBSettings);
            projectRepository.Create(project);
        }

        [TestMethod]
        public void Read_正常_例外が発生しないこと()
        {
            var projectKey = new ProjectTableEntity
            {
                ProjectId = Guid.Parse("00000000-2001-0000-0000-000000000000"),
            };
            var projectRepository = new ProjectRepository(TestEnvironment.DBSettings);
            var project = projectRepository.Read(projectKey);

            Assert.IsNotNull(project);
        }

        [TestMethod]
        public void Update_正常_例外が発生しないこと()
        {
            var projectKey = new ProjectTableEntity
            {
                ProjectId = Guid.Parse("00000000-2001-0000-0000-000000000000"),
            };
            var projectRepository = new ProjectRepository(TestEnvironment.DBSettings);
            var project = projectRepository.Read(projectKey);
            project.UpdateTime = DateUtil.Now;
            Assert.IsTrue(projectRepository.Update(project));
        }

        [TestMethod]
        public void Delete_正常_例外が発生しないこと()
        {
            var project = new ProjectTableEntity
            {
                ProjectId = Guid.NewGuid(),
                ProjectCode = new string('Y', 20),
                Name = new string('X', 100),
                Description = new string('X', 400),
                SortNo = int.MaxValue,
                Status = ProjectStatus.DELETED.ToString(),
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
                DeleteTime = DateTimeOffset.MaxValue,
            };
            var projectRepository = new ProjectRepository(TestEnvironment.DBSettings);
            projectRepository.Create(project);
            Assert.IsTrue(projectRepository.Delete(project));
        }
    }
}

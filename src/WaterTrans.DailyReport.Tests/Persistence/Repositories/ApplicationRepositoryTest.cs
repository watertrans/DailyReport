using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WaterTrans.DailyReport.Application.TableEntities;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Persistence.Repositories;

namespace WaterTrans.DailyReport.Tests.Persistence.Repositories
{
    [TestClass]
    public class ApplicationRepositoryTest
    {
        [TestMethod]
        public void Create_NotThrowsException_StateIsValid()
        {
            var now = DateUtil.Now;
            var application = new ApplicationTableEntity
            {
                ApplicationId = Guid.NewGuid(),
                ClientId = new string('X', 100),
                ClientSecret = new string('X', 100),
                Name = new string('X', 100),
                Description = new string('X', 400),
                Roles = new string('X', 8000),
                Scopes = new string('X', 8000),
                GrantTypes = new string('X', 8000),
                RedirectUris = new string('X', 8000),
                PostLogoutRedirectUris = new string('X', 8000),
                Status = ApplicationStatus.NORMAL.ToString(),
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
            };
            var applicationRepository = new ApplicationRepository(TestEnvironment.DBSettings);
            applicationRepository.Create(application);
        }

        [TestMethod]
        public void Read_NotThrowsException_StateIsValid()
        {
            var applicationKey = new ApplicationTableEntity
            {
                ApplicationId = Guid.Parse("00000000-A001-0000-0000-000000000000"),
            };
            var applicationRepository = new ApplicationRepository(TestEnvironment.DBSettings);
            var application = applicationRepository.Read(applicationKey);

            Assert.IsNotNull(application);
        }

        [TestMethod]
        public void Update_NotThrowsException_StateIsValid()
        {
            var applicationKey = new ApplicationTableEntity
            {
                ApplicationId = Guid.Parse("00000000-A001-0000-0000-000000000000"),
            };
            var applicationRepository = new ApplicationRepository(TestEnvironment.DBSettings);
            var application = applicationRepository.Read(applicationKey);
            application.UpdateTime = DateUtil.Now;
            Assert.IsTrue(applicationRepository.Update(application));
        }

        [TestMethod]
        public void Delete_NotThrowsException_StateIsValid()
        {
            var now = DateUtil.Now;
            var application = new ApplicationTableEntity
            {
                ApplicationId = Guid.NewGuid(),
                ClientId = new string('Y', 100),
                ClientSecret = new string('X', 100),
                Name = new string('X', 100),
                Description = new string('X', 400),
                Roles = new string('X', 8000),
                Scopes = new string('X', 8000),
                GrantTypes = new string('X', 8000),
                RedirectUris = new string('X', 8000),
                PostLogoutRedirectUris = new string('X', 8000),
                Status = ApplicationStatus.SUSPENDED.ToString(),
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
            };
            var applicationRepository = new ApplicationRepository(TestEnvironment.DBSettings);
            applicationRepository.Create(application);
            Assert.IsTrue(applicationRepository.Delete(application));
        }
    }
}

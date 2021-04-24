using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WaterTrans.DailyReport.Application.TableEntities;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Persistence.Repositories;

namespace WaterTrans.DailyReport.Tests.Persistence.Repositories
{
    [TestClass]
    public class AccessTokenRepositoryTest
    {
        [TestMethod]
        public void Create_NotThrowsException_StateIsValid()
        {
            var now = DateUtil.Now;
            var accessToken = new AccessTokenTableEntity
            {
                TokenId = new string('X', 100),
                Name = new string('X', 100),
                Description = new string('X', 400),
                ApplicationId = Guid.NewGuid(),
                PrincipalType = PrincipalType.Application.ToString(),
                PrincipalId = Guid.NewGuid(),
                Scopes = new string('X', 8000),
                Status = AccessTokenStatus.NORMAL.ToString(),
                ExpiryTime = DateTimeOffset.MaxValue,
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
            };
            var accessTokenRepository = new AccessTokenRepository(TestEnvironment.DBSettings);
            accessTokenRepository.Create(accessToken);
        }

        [TestMethod]
        public void Read_NotThrowsException_StateIsValid()
        {
            var accessTokenKey = new AccessTokenTableEntity
            {
                TokenId = "normal",
            };
            var accessTokenRepository = new AccessTokenRepository(TestEnvironment.DBSettings);
            var accessToken = accessTokenRepository.Read(accessTokenKey);

            Assert.IsNotNull(accessToken);
        }

        [TestMethod]
        public void Update_NotThrowsException_StateIsValid()
        {
            var accessTokenKey = new AccessTokenTableEntity
            {
                TokenId = "normal",
            };
            var accessTokenRepository = new AccessTokenRepository(TestEnvironment.DBSettings);
            var accessToken = accessTokenRepository.Read(accessTokenKey);
            accessToken.UpdateTime = DateUtil.Now;
            Assert.IsTrue(accessTokenRepository.Update(accessToken));
        }

        [TestMethod]
        public void Delete_NotThrowsException_StateIsValid()
        {
            var now = DateUtil.Now;
            var accessToken = new AccessTokenTableEntity
            {
                TokenId = new string('Y', 100),
                Name = new string('X', 100),
                Description = new string('X', 400),
                ApplicationId = Guid.NewGuid(),
                PrincipalType = PrincipalType.Application.ToString(),
                PrincipalId = Guid.NewGuid(),
                Scopes = new string('X', 8000),
                Status = AccessTokenStatus.SUSPENDED.ToString(),
                ExpiryTime = DateTimeOffset.MaxValue,
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
            };
            var accessTokenRepository = new AccessTokenRepository(TestEnvironment.DBSettings);
            accessTokenRepository.Create(accessToken);
            Assert.IsTrue(accessTokenRepository.Delete(accessToken));
        }
    }
}

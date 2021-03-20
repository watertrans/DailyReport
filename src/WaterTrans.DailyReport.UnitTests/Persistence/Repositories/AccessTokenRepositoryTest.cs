using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WaterTrans.DailyReport.Application.TableEntities;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Persistence.Repositories;

namespace WaterTrans.DailyReport.UnitTests.Persistence.Repositories
{
    [TestClass]
    public class AccessTokenRepositoryTest
    {
        [TestMethod]
        public void Create_正常_例外が発生しないこと()
        {
            var now = DateUtil.Now;
            var accessToken = new AccessTokenTableEntity
            {
                TokenId = new string('X', 100),
                Name = new string('X', 100),
                Description = new string('X', 400),
                PrincipalType = PrincipalType.APPLICATION.ToString(),
                PrincipalId = Guid.NewGuid(),
                Scopes = new string('X', 8000),
                Status = AccessTokenStatus.NORMAL.ToString(),
                ExpiryTime = DateTimeOffset.MaxValue,
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
                DeleteTime = null,
            };
            var accessTokenRepository = new AccessTokenRepository(TestEnvironment.DBSettings);
            accessTokenRepository.Create(accessToken);
        }

        [TestMethod]
        public void Read_正常_例外が発生しないこと()
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
        public void Update_正常_例外が発生しないこと()
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
        public void Delete_正常_例外が発生しないこと()
        {
            var now = DateUtil.Now;
            var accessToken = new AccessTokenTableEntity
            {
                TokenId = new string('Y', 100),
                Name = new string('X', 100),
                Description = new string('X', 400),
                PrincipalType = PrincipalType.APPLICATION.ToString(),
                PrincipalId = Guid.NewGuid(),
                Scopes = new string('X', 8000),
                Status = AccessTokenStatus.DELETED.ToString(),
                ExpiryTime = DateTimeOffset.MaxValue,
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
                DeleteTime = DateTimeOffset.MaxValue,
            };
            var accessTokenRepository = new AccessTokenRepository(TestEnvironment.DBSettings);
            accessTokenRepository.Create(accessToken);
            Assert.IsTrue(accessTokenRepository.Delete(accessToken));
        }
    }
}

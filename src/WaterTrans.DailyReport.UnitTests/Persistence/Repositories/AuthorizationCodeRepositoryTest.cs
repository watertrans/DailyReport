using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WaterTrans.DailyReport.Application.TableEntities;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Persistence.Repositories;

namespace WaterTrans.DailyReport.UnitTests.Persistence.Repositories
{
    [TestClass]
    public class AuthorizationCodeRepositoryTest
    {
        [TestMethod]
        public void Create_NotThrowsException_StateIsValid()
        {
            var now = DateUtil.Now;
            var authorizationCode = new AuthorizationCodeTableEntity
            {
                CodeId = new string('X', 100),
                ApplicationId = Guid.NewGuid(),
                Status = AuthorizationCodeStatus.NORMAL.ToString(),
                ExpiryTime = DateTimeOffset.MaxValue,
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
            };
            var authorizationCodeRepository = new AuthorizationCodeRepository(TestEnvironment.DBSettings);
            authorizationCodeRepository.Create(authorizationCode);
        }

        [TestMethod]
        public void Read_NotThrowsException_StateIsValid()
        {
            var authorizationCodeKey = new AuthorizationCodeTableEntity
            {
                CodeId = "normal",
            };
            var authorizationCodeRepository = new AuthorizationCodeRepository(TestEnvironment.DBSettings);
            var authorizationCode = authorizationCodeRepository.Read(authorizationCodeKey);

            Assert.IsNotNull(authorizationCode);
        }

        [TestMethod]
        public void Update_NotThrowsException_StateIsValid()
        {
            var authorizationCodeKey = new AuthorizationCodeTableEntity
            {
                CodeId = "normal",
            };
            var authorizationCodeRepository = new AuthorizationCodeRepository(TestEnvironment.DBSettings);
            var authorizationCode = authorizationCodeRepository.Read(authorizationCodeKey);
            authorizationCode.UpdateTime = DateUtil.Now;
            Assert.IsTrue(authorizationCodeRepository.Update(authorizationCode));
        }

        [TestMethod]
        public void Delete_NotThrowsException_StateIsValid()
        {
            var now = DateUtil.Now;
            var authorizationCode = new AuthorizationCodeTableEntity
            {
                CodeId = new string('Y', 100),
                ApplicationId = Guid.NewGuid(),
                Status = AuthorizationCodeStatus.USED.ToString(),
                ExpiryTime = DateTimeOffset.MaxValue,
                CreateTime = DateTimeOffset.MaxValue,
                UpdateTime = DateTimeOffset.MaxValue,
            };
            var authorizationCodeRepository = new AuthorizationCodeRepository(TestEnvironment.DBSettings);
            authorizationCodeRepository.Create(authorizationCode);
            Assert.IsTrue(authorizationCodeRepository.Delete(authorizationCode));
        }
    }
}

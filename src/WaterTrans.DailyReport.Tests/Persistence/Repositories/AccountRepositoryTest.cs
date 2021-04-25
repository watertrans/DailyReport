using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using WaterTrans.DailyReport.Application.TableEntities;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Persistence.Repositories;

namespace WaterTrans.DailyReport.Tests.Persistence.Repositories
{
    [TestClass]
    public class AccountRepositoryTest
    {
        [TestMethod]
        public void Create_NotThrowsException_StateIsValid()
        {
            var now = DateUtil.Now;
            var account = new AccountTableEntity
            {
                AccountId = Guid.NewGuid(),
                PersonId = Guid.NewGuid(),
                Roles = JsonUtil.Serialize(new List<string>()),
                CreateTime = DateTimeOffset.MaxValue,
                LastLoginTime = DateTimeOffset.MaxValue,
            };
            var accountRepository = new AccountRepository(TestEnvironment.DBSettings);
            accountRepository.Create(account);
        }

        [TestMethod]
        public void Read_NotThrowsException_StateIsValid()
        {
            var accountKey = new AccountTableEntity
            {
                AccountId = Guid.Parse("00000000-B001-0000-0000-000000000000"),
            };
            var accountRepository = new AccountRepository(TestEnvironment.DBSettings);
            var account = accountRepository.Read(accountKey);

            Assert.IsNotNull(account);
        }

        [TestMethod]
        public void Update_NotThrowsException_StateIsValid()
        {
            var accountKey = new AccountTableEntity
            {
                AccountId = Guid.Parse("00000000-B001-0000-0000-000000000000"),
            };
            var accountRepository = new AccountRepository(TestEnvironment.DBSettings);
            var account = accountRepository.Read(accountKey);
            account.LastLoginTime = DateUtil.Now;
            Assert.IsTrue(accountRepository.Update(account));
        }

        [TestMethod]
        public void Delete_NotThrowsException_StateIsValid()
        {
            var now = DateUtil.Now;
            var account = new AccountTableEntity
            {
                AccountId = Guid.NewGuid(),
                PersonId = Guid.NewGuid(),
                Roles = JsonUtil.Serialize(new List<string>()),
                CreateTime = DateTimeOffset.MaxValue,
                LastLoginTime = DateTimeOffset.MaxValue,
            };
            var accountRepository = new AccountRepository(TestEnvironment.DBSettings);
            accountRepository.Create(account);
            Assert.IsTrue(accountRepository.Delete(account));
        }
    }
}

using System;
using System.Transactions;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.DataTransferObjects;
using WaterTrans.DailyReport.Application.TableEntities;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Domain.Constants;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Application.Services
{
    /// <summary>
    /// アカウントサービス
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountQueryService _accountQueryService;
        private readonly IPersonRepository _personRepository;
        private readonly IPersonQueryService _personQueryService;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="accountRepository"><see cref="IAccountRepository"/></param>
        /// <param name="accountQueryService"><see cref="IAccountQueryService"/></param>
        /// <param name="personRepository"><see cref="IPersonRepository"/></param>
        /// <param name="personQueryService"><see cref="IPersonQueryService"/></param>
        public AccountService(
            IAccountRepository accountRepository,
            IAccountQueryService accountQueryService,
            IPersonRepository personRepository,
            IPersonQueryService personQueryService)
        {
            _accountRepository = accountRepository;
            _accountQueryService = accountQueryService;
            _personRepository = personRepository;
            _personQueryService = personQueryService;
        }

        /// <inheritdoc/>
        public Account CreateAccount(AccountCreateDto dto)
        {
            var now = DateUtil.Now;
            var personId = Guid.NewGuid();
            var personCode = personId.ToString("N").Substring(0, 20);

            while (_personQueryService.ExistsPersonCode(personCode))
            {
                personCode = Guid.NewGuid().ToString("N").Substring(0, 20);
            }

            var existingPerson = _personQueryService.GetPersonByLoginId(dto.LoginId);
            if (existingPerson != null)
            {
                personId = existingPerson.PersonId;
            }

            var account = new AccountTableEntity
            {
                AccountId = dto.AccountId,
                PersonId = personId,
                CreateTime = now,
                LastLoginTime = now,
            };

            var person = new PersonTableEntity
            {
                PersonId = personId,
                PersonCode = personCode,
                LoginId = dto.LoginId,
                Name = dto.Name,
                Title = string.Empty,
                Description = string.Empty,
                Status = PersonStatus.NORMAL.ToString(),
                SortNo = int.MaxValue,
                CreateTime = now,
                UpdateTime = now,
            };

            using (var tran = new TransactionScope())
            {
                _accountRepository.Create(account);

                if (existingPerson == null)
                {
                    _personRepository.Create(person);
                }

                tran.Complete();
            }

            return new Account
            {
                AccountId = account.AccountId,
                Person = _personQueryService.GetPerson(personId),
                CreateTime = account.CreateTime,
                LastLoginTime = account.LastLoginTime,
            };
        }

        /// <inheritdoc/>
        public void UpdateLastLoginTime(Guid accountId)
        {
            _accountQueryService.UpdateLastLoginTime(accountId);
        }

        /// <inheritdoc/>
        public bool ExistsAccount(Guid accountId)
        {
            var accountKey = new AccountTableEntity
            {
                AccountId = accountId,
            };

            return _accountRepository.Read(accountKey) != null;
        }

        /// <inheritdoc/>
        public Account GetAccount(Guid accountId)
        {
            var accountKey = new AccountTableEntity
            {
                AccountId = accountId,
            };

            var account = _accountRepository.Read(accountKey);

            if (account == null)
            {
                return null;
            }

            return new Account
            {
                AccountId = account.AccountId,
                Person = _personQueryService.GetPerson(account.PersonId),
                CreateTime = account.CreateTime,
                LastLoginTime = account.LastLoginTime,
            };
        }
    }
}

using System;
using System.Collections.Generic;
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
    /// 従業員サービス
    /// </summary>
    public class PersonService : IPersonService
    {
        private readonly IPersonQueryService _personQueryService;
        private readonly IPersonRepository _personRepository;
        private readonly ITagRepository _tagRepository;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="personQueryService"><see cref="IPersonQueryService"/></param>
        /// <param name="personRepository"><see cref="IPersonRepository"/></param>
        /// <param name="tagRepository"><see cref="ITagRepository"/></param>
        public PersonService(
            IPersonQueryService personQueryService,
            IPersonRepository personRepository,
            ITagRepository tagRepository)
        {
            _personQueryService = personQueryService;
            _personRepository = personRepository;
            _tagRepository = tagRepository;
        }

        /// <inheritdoc/>
        public IList<Person> QueryPerson(PersonQueryDto dto)
        {
            return _personQueryService.Query(dto.Query, dto.Sort, dto);
        }

        /// <inheritdoc/>
        public Person CreatePerson(PersonCreateDto dto)
        {
            var now = DateUtil.Now;
            var entity = new PersonTableEntity
            {
                PersonId = Guid.NewGuid(),
                PersonCode = dto.PersonCode,
                LoginId = dto.LoginId,
                Name = dto.Name,
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                SortNo = dto.SortNo,
                CreateTime = now,
                UpdateTime = now,
            };

            using (var tran = new TransactionScope())
            {
                _personRepository.Create(entity);

                foreach (var tag in dto.Tags)
                {
                    _tagRepository.Create(new TagTableEntity
                    {
                        TagId = Guid.NewGuid(),
                        TargetId = entity.PersonId,
                        Value = tag,
                        TargetTable = "Person",
                        CreateTime = now,
                    });
                }

                tran.Complete();
            }
            return _personQueryService.GetPerson(entity.PersonId);
        }

        /// <inheritdoc/>
        public Person UpdatePerson(PersonUpdateDto dto)
        {
            var now = DateUtil.Now;
            var entity = _personRepository.Read(new PersonTableEntity { PersonId = dto.PersonId });

            if (dto.PersonCode != null)
            {
                entity.PersonCode = dto.PersonCode;
            }

            if (dto.LoginId != null)
            {
                entity.LoginId = dto.LoginId;
            }

            if (dto.Name != null)
            {
                entity.Name = dto.Name;
            }

            if (dto.Title != null)
            {
                entity.Title = dto.Title;
            }

            if (dto.Description != null)
            {
                entity.Description = dto.Description;
            }

            if (dto.Status != null)
            {
                entity.Status = dto.Status;
            }

            if (dto.SortNo != null)
            {
                entity.SortNo = dto.SortNo.Value;
            }

            entity.UpdateTime = now;

            using (var tran = new TransactionScope())
            {
                _personRepository.Update(entity);

                if (dto.Tags != null)
                {
                    _tagRepository.DeleteByTargetId(dto.PersonId);
                    foreach (var tag in dto.Tags)
                    {
                        _tagRepository.Create(new TagTableEntity
                        {
                            TagId = Guid.NewGuid(),
                            TargetId = entity.PersonId,
                            Value = tag,
                            TargetTable = "Person",
                            CreateTime = now,
                        });
                    }
                }

                tran.Complete();
            }
            return _personQueryService.GetPerson(entity.PersonId);
        }

        /// <inheritdoc/>
        public void DeletePerson(Guid personId)
        {
            using (var tran = new TransactionScope())
            {
                _tagRepository.DeleteByTargetId(personId);
                _personRepository.Delete(new PersonTableEntity { PersonId = personId });
                tran.Complete();
            }
        }

        /// <inheritdoc/>
        public Person GetPerson(Guid personId)
        {
            return _personQueryService.GetPerson(personId);
        }
    }
}

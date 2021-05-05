using System;
using System.Collections.Generic;
using System.Transactions;
using WaterTrans.DailyReport.Application.Abstractions;
using WaterTrans.DailyReport.Application.DataTransferObjects;
using WaterTrans.DailyReport.Application.TableEntities;
using WaterTrans.DailyReport.Application.Utils;
using WaterTrans.DailyReport.Domain.Entities;

namespace WaterTrans.DailyReport.Application.Services
{
    /// <summary>
    /// 部署サービス
    /// </summary>
    public class GroupService : IGroupService
    {
        private readonly IGroupQueryService _groupQueryService;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupPersonRepository _groupPersonRepository;
        private readonly ITagRepository _tagRepository;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="groupQueryService"><see cref="IGroupQueryService"/></param>
        /// <param name="groupRepository"><see cref="IGroupRepository"/></param>
        /// <param name="groupPersonRepository"><see cref="IGroupPersonRepository"/></param>
        /// <param name="tagRepository"><see cref="ITagRepository"/></param>
        public GroupService(
            IGroupQueryService groupQueryService,
            IGroupRepository groupRepository,
            IGroupPersonRepository groupPersonRepository,
            ITagRepository tagRepository)
        {
            _groupQueryService = groupQueryService;
            _groupRepository = groupRepository;
            _groupPersonRepository = groupPersonRepository;
            _tagRepository = tagRepository;
        }

        /// <inheritdoc/>
        public IList<Group> QueryGroup(GroupQueryDto dto)
        {
            return _groupQueryService.Query(dto.Query, dto.Sort, dto);
        }

        /// <inheritdoc/>
        public IList<GroupPerson> QueryPerson(GroupPersonQueryDto dto)
        {
            return _groupQueryService.QueryPerson(dto.GroupId, dto.Query, dto.Sort, dto);
        }

        /// <inheritdoc/>
        public Group CreateGroup(GroupCreateDto dto)
        {
            var now = DateUtil.Now;
            var entity = new GroupTableEntity
            {
                GroupId = Guid.NewGuid(),
                GroupCode = dto.GroupCode,
                GroupTree = dto.GroupTree,
                Name = dto.Name,
                Description = dto.Description,
                Status = dto.Status,
                SortNo = dto.SortNo,
                CreateTime = now,
                UpdateTime = now,
            };

            using (var tran = new TransactionScope())
            {
                _groupRepository.Create(entity);

                foreach (var tag in dto.Tags)
                {
                    _tagRepository.Create(new TagTableEntity
                    {
                        TagId = Guid.NewGuid(),
                        TargetId = entity.GroupId,
                        Value = tag,
                        TargetTable = "Group",
                        CreateTime = now,
                    });
                }

                tran.Complete();
            }
            return _groupQueryService.GetGroup(entity.GroupId);
        }

        /// <inheritdoc/>
        public Group UpdateGroup(GroupUpdateDto dto)
        {
            var now = DateUtil.Now;
            var entity = _groupRepository.Read(new GroupTableEntity { GroupId = dto.GroupId });

            if (dto.GroupCode != null)
            {
                entity.GroupCode = dto.GroupCode;
            }

            if (dto.GroupTree != null)
            {
                entity.GroupTree = dto.GroupTree;
            }

            if (dto.Name != null)
            {
                entity.Name = dto.Name;
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
                _groupRepository.Update(entity);

                if (dto.Tags != null)
                {
                    _tagRepository.DeleteByTargetId(dto.GroupId);
                    foreach (var tag in dto.Tags)
                    {
                        _tagRepository.Create(new TagTableEntity
                        {
                            TagId = Guid.NewGuid(),
                            TargetId = entity.GroupId,
                            Value = tag,
                            TargetTable = "Group",
                            CreateTime = now,
                        });
                    }
                }

                tran.Complete();
            }
            return _groupQueryService.GetGroup(entity.GroupId);
        }

        /// <inheritdoc/>
        public void DeleteGroup(Guid groupId)
        {
            using (var tran = new TransactionScope())
            {
                _tagRepository.DeleteByTargetId(groupId);
                _groupRepository.Delete(new GroupTableEntity { GroupId = groupId });
                tran.Complete();
            }
        }

        /// <inheritdoc/>
        public Group GetGroup(Guid groupId)
        {
            return _groupQueryService.GetGroup(groupId);
        }

        /// <inheritdoc/>
        public bool ContainsGroupPerson(Guid groupId, Guid personId)
        {
            return _groupPersonRepository.Read(new GroupPersonTableEntity
            {
                GroupId = groupId,
                PersonId = personId,
            }) != null;
        }

        /// <inheritdoc/>
        public void AddGroupPerson(Guid groupId, Guid personId, string positionType)
        {
            using (var tran = new TransactionScope())
            {
                _groupPersonRepository.Delete(new GroupPersonTableEntity
                {
                    GroupId = groupId,
                    PersonId = personId,
                });
                _groupPersonRepository.Create(new GroupPersonTableEntity
                {
                    GroupId = groupId,
                    PersonId = personId,
                    PositionType = positionType,
                });
                tran.Complete();
            }
        }

        /// <inheritdoc/>
        public void RemoveGroupPerson(Guid groupId, Guid personId)
        {
            _groupPersonRepository.Delete(new GroupPersonTableEntity
            {
                GroupId = groupId,
                PersonId = personId,
            });
        }

        /// <inheritdoc/>
        public IList<Group> GetOrganization()
        {
            return _groupQueryService.GetOrganization();
        }
    }
}

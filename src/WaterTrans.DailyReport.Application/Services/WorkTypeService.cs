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
    /// 業務分類サービス
    /// </summary>
    public class WorkTypeService : IWorkTypeService
    {
        private readonly IWorkTypeQueryService _workTypeQueryService;
        private readonly IWorkTypeRepository _workTypeRepository;
        private readonly ITagRepository _tagRepository;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="workTypeQueryService"><see cref="IWorkTypeQueryService"/></param>
        /// <param name="workTypeRepository"><see cref="IWorkTypeRepository"/></param>
        /// <param name="tagRepository"><see cref="ITagRepository"/></param>
        public WorkTypeService(
            IWorkTypeQueryService workTypeQueryService,
            IWorkTypeRepository workTypeRepository,
            ITagRepository tagRepository)
        {
            _workTypeQueryService = workTypeQueryService;
            _workTypeRepository = workTypeRepository;
            _tagRepository = tagRepository;
        }

        /// <inheritdoc/>
        public IList<WorkType> QueryWorkType(WorkTypeQueryDto dto)
        {
            return _workTypeQueryService.Query(dto.Query, dto.Sort, dto);
        }

        /// <inheritdoc/>
        public WorkType CreateWorkType(WorkTypeCreateDto dto)
        {
            var now = DateUtil.Now;
            var entity = new WorkTypeTableEntity
            {
                WorkTypeId = Guid.NewGuid(),
                WorkTypeCode = dto.WorkTypeCode,
                WorkTypeTree = dto.WorkTypeTree,
                Name = dto.Name,
                Description = dto.Description,
                Status = dto.Status,
                SortNo = dto.SortNo,
                CreateTime = now,
                UpdateTime = now,
            };

            using (var tran = new TransactionScope())
            {
                _workTypeRepository.Create(entity);

                foreach (var tag in dto.Tags)
                {
                    _tagRepository.Create(new TagTableEntity
                    {
                        TagId = Guid.NewGuid(),
                        TargetId = entity.WorkTypeId,
                        Value = tag,
                        TargetTable = "WorkType",
                        CreateTime = now,
                    });
                }

                tran.Complete();
            }
            return _workTypeQueryService.GetWorkType(entity.WorkTypeId);
        }

        /// <inheritdoc/>
        public WorkType UpdateWorkType(WorkTypeUpdateDto dto)
        {
            var now = DateUtil.Now;
            var entity = _workTypeRepository.Read(new WorkTypeTableEntity { WorkTypeId = dto.WorkTypeId });

            if (dto.WorkTypeCode != null)
            {
                entity.WorkTypeCode = dto.WorkTypeCode;
            }

            if (dto.WorkTypeTree != null)
            {
                entity.WorkTypeTree = dto.WorkTypeTree;
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
                _workTypeRepository.Update(entity);

                if (dto.Tags != null)
                {
                    _tagRepository.DeleteByTargetId(dto.WorkTypeId);
                    foreach (var tag in dto.Tags)
                    {
                        _tagRepository.Create(new TagTableEntity
                        {
                            TagId = Guid.NewGuid(),
                            TargetId = entity.WorkTypeId,
                            Value = tag,
                            TargetTable = "WorkType",
                            CreateTime = now,
                        });
                    }
                }

                tran.Complete();
            }
            return _workTypeQueryService.GetWorkType(entity.WorkTypeId);
        }

        /// <inheritdoc/>
        public void DeleteWorkType(Guid workTypeId)
        {
            using (var tran = new TransactionScope())
            {
                _tagRepository.DeleteByTargetId(workTypeId);
                _workTypeRepository.Delete(new WorkTypeTableEntity { WorkTypeId = workTypeId });
                tran.Complete();
            }
        }

        /// <inheritdoc/>
        public WorkType GetWorkType(Guid workTypeId)
        {
            return _workTypeQueryService.GetWorkType(workTypeId);
        }
    }
}

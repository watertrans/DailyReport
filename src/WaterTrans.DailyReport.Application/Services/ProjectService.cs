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
    /// プロジェクトサービス
    /// </summary>
    public class ProjectService : IProjectService
    {
        private readonly IProjectQueryService _projectQueryService;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectPersonRepository _projectPersonRepository;
        private readonly ITagRepository _tagRepository;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="projectQueryService"><see cref="IProjectQueryService"/></param>
        /// <param name="projectRepository"><see cref="IProjectRepository"/></param>
        /// <param name="projectPersonRepository"><see cref="IProjectPersonRepository"/></param>
        /// <param name="tagRepository"><see cref="ITagRepository"/></param>
        public ProjectService(
            IProjectQueryService projectQueryService,
            IProjectRepository projectRepository,
            IProjectPersonRepository projectPersonRepository,
            ITagRepository tagRepository)
        {
            _projectQueryService = projectQueryService;
            _projectRepository = projectRepository;
            _projectPersonRepository = projectPersonRepository;
            _tagRepository = tagRepository;
        }

        /// <inheritdoc/>
        public IList<Project> QueryProject(ProjectQueryDto dto)
        {
            return _projectQueryService.Query(dto.Query, dto.Sort, dto);
        }

        /// <inheritdoc/>
        public IList<Person> QueryPerson(ProjectPersonQueryDto dto)
        {
            return _projectQueryService.QueryPerson(dto.ProjectId, dto.Query, dto.Sort, dto);
        }

        /// <inheritdoc/>
        public Project CreateProject(ProjectCreateDto dto)
        {
            var now = DateUtil.Now;
            var entity = new ProjectTableEntity
            {
                ProjectId = Guid.NewGuid(),
                ProjectCode = dto.ProjectCode,
                Name = dto.Name,
                Description = dto.Description,
                Status = dto.Status,
                SortNo = dto.SortNo,
                CreateTime = now,
                UpdateTime = now,
            };

            using (var tran = new TransactionScope())
            {
                _projectRepository.Create(entity);

                foreach (var tag in dto.Tags)
                {
                    _tagRepository.Create(new TagTableEntity
                    {
                        TagId = Guid.NewGuid(),
                        TargetId = entity.ProjectId,
                        Value = tag,
                        TargetTable = "Project",
                        CreateTime = now,
                    });
                }

                tran.Complete();
            }
            return _projectQueryService.GetProject(entity.ProjectId);
        }

        /// <inheritdoc/>
        public Project UpdateProject(ProjectUpdateDto dto)
        {
            var now = DateUtil.Now;
            var entity = _projectRepository.Read(new ProjectTableEntity { ProjectId = dto.ProjectId });

            if (dto.ProjectCode != null)
            {
                entity.ProjectCode = dto.ProjectCode;
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
                _projectRepository.Update(entity);

                if (dto.Tags != null)
                {
                    _tagRepository.DeleteByTargetId(dto.ProjectId);
                    foreach (var tag in dto.Tags)
                    {
                        _tagRepository.Create(new TagTableEntity
                        {
                            TagId = Guid.NewGuid(),
                            TargetId = entity.ProjectId,
                            Value = tag,
                            TargetTable = "Project",
                            CreateTime = now,
                        });
                    }
                }

                tran.Complete();
            }
            return _projectQueryService.GetProject(entity.ProjectId);
        }

        /// <inheritdoc/>
        public void DeleteProject(Guid projectId)
        {
            using (var tran = new TransactionScope())
            {
                _tagRepository.DeleteByTargetId(projectId);
                _projectRepository.Delete(new ProjectTableEntity { ProjectId = projectId });
                tran.Complete();
            }
        }

        /// <inheritdoc/>
        public Project GetProject(Guid projectId)
        {
            return _projectQueryService.GetProject(projectId);
        }

        /// <inheritdoc/>
        public bool ContainsProjectPerson(Guid projectId, Guid personId)
        {
            return _projectPersonRepository.Read(new ProjectPersonTableEntity
            {
                ProjectId = projectId,
                PersonId = personId,
            }) != null;
        }

        /// <inheritdoc/>
        public void AddProjectPerson(Guid projectId, Guid personId)
        {
            using (var tran = new TransactionScope())
            {
                _projectPersonRepository.Delete(new ProjectPersonTableEntity
                {
                    ProjectId = projectId,
                    PersonId = personId,
                });
                _projectPersonRepository.Create(new ProjectPersonTableEntity
                {
                    ProjectId = projectId,
                    PersonId = personId,
                });
                tran.Complete();
            }
        }

        /// <inheritdoc/>
        public void RemoveProjectPerson(Guid projectId, Guid personId)
        {
            _projectPersonRepository.Delete(new ProjectPersonTableEntity
            {
                ProjectId = projectId,
                PersonId = personId,
            });
        }
    }
}

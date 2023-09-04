using AutoMapper;
using Mlok.Core.Data;
using Mlok.Core.Models;
using Mlok.Core.Models.ApiExceptions;
using Mlok.Core.Utils;
using Mlok.Web.Sites.JobChIN.Controllers;
using Mlok.Web.Sites.JobChIN.Models;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using Mlok.Web.Sites.JobChIN.Models.Filters;
using Mlok.Web.Sites.JobChIN.Models.WorkPositionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;

namespace Mlok.Web.Sites.JobChIN.Services
{
    public class WorkPositionService
    {
        private readonly WorkPositionAreaOfInterestController workPositionAreaOfInterestController;
        private readonly WorkPositionContractTypeController workPositionContractTypeController;
        private readonly WorkPositionFacultyController workPositionFacultyController;
        private readonly WorkPositionHardSkillController workPositionHardSkillController;
        private readonly WorkPositionSoftSkillController workPositionSoftSkillController;
        private readonly WorkPositionLanguageSkillController workPositionLanguageSkillController;
        private readonly WorkPositionUserController workPositionUserController;
        private readonly StudentWorkPositionViewedController studentWorkPositionViewedController;
        private readonly CompanyCompanyTypeController companyCompanyTypeController;
        private readonly CompanyBranchController companyBranchController;
        private readonly StudentShownInterestContorller studentShownInterestContorller;
        private readonly StudentController studentController;

        private readonly DbScopeProvider scopeProvider;
        private readonly ISettings settings;

        public WorkPositionController WorkPositionController { get; }

        public WorkPositionService(DbScopeProvider scopeProvider, ISettings settings)
        {
            this.scopeProvider = scopeProvider;
            this.settings = settings;

            workPositionAreaOfInterestController = new WorkPositionAreaOfInterestController(scopeProvider);
            workPositionContractTypeController = new WorkPositionContractTypeController(scopeProvider);
            workPositionFacultyController = new WorkPositionFacultyController(scopeProvider);
            workPositionHardSkillController = new WorkPositionHardSkillController(scopeProvider);
            workPositionSoftSkillController = new WorkPositionSoftSkillController(scopeProvider);
            workPositionLanguageSkillController = new WorkPositionLanguageSkillController(scopeProvider);
            workPositionUserController = new WorkPositionUserController(scopeProvider);
            studentWorkPositionViewedController = new StudentWorkPositionViewedController(scopeProvider);
            companyCompanyTypeController = new CompanyCompanyTypeController(scopeProvider);
            companyBranchController = new CompanyBranchController(scopeProvider);
            studentShownInterestContorller = new StudentShownInterestContorller(scopeProvider);
            studentController = new StudentController(scopeProvider);

            WorkPositionController = new WorkPositionController(scopeProvider);
        }

        /// <summary>
        /// Create a new work position. Check if the work position can be created.
        /// </summary>
        /// <param name="model">State of the work position to create.</param>
        public Result<WorkPositionUpdateDto> Create(int companyId, WorkPositionCreateDto model)
        {
            var workPosition = Mapper.Map<WorkPosition>(model);
            workPosition.Visibility = new Visibility() { Hidden = false };
            return Update(companyId, workPosition);
        }

        /// <summary>
        /// Save the state of the work position. Only properties that are not null, will be updated.
        /// </summary>
        /// <param name="model">State of the work position to save.</param>
        public Result<WorkPositionUpdateDto> Update(CompanyUser companyUser, WorkPositionUpdateDto model)
        {
            var workPosition = Mapper.Map<WorkPosition>(model);

            if (!WorkPositionController.ValidateWorkPosition(companyUser, model.WorkPositionId))
                return new Result<WorkPositionUpdateDto>(new NotFoundException($"Work position with id '{model.WorkPositionId}' not found"));

            return Update(companyUser.CompanyId, workPosition);
        }

        public WorkPosition Update(WorkPosition workPosition)
        {
            if (workPosition.BasicInfo != null && workPosition.BasicInfo.BranchId.HasValue && !companyBranchController.HasCompanyBranch(workPosition.CompanyId.Value, workPosition.BasicInfo.BranchId.Value))
                workPosition.BasicInfo.BranchId = null;

            WorkPositionController.Update(workPosition);

            if (workPosition.BasicInfo != null)
            {
                workPositionContractTypeController.Join(workPosition);
                workPositionUserController.Join(workPosition);
            }
            if (workPosition.Candidates != null)
            {
                workPositionAreaOfInterestController.Join(workPosition);
                workPositionFacultyController.Join(workPosition);
                workPositionHardSkillController.Join(workPosition);
                workPositionSoftSkillController.Join(workPosition);
                workPositionLanguageSkillController.Join(workPosition);
            }

            return workPosition;
        }

        /// <summary>
        /// Return page of work positions that match the filter.
        /// </summary>
        public Result<StudentWorkPositionsPagedDto> GetStudentWorkPositionsPaged(WorkPositionFilterDto filterDto, int studentId)
        {
            var filter = Mapper.Map<WorkPositionFilter>(filterDto);
            filter.Active = true;
            filter.Published = true;
            filter.IncludeCompany = true;
            filter.IncludeStudentDetails = true;
            filter.StudentId = studentId;
            IPaginationInfo paginationInfo;
            var workPositions = WorkPositionController.GetPaged(filterDto.PageNo, settings.WorkPositionsPerPage, filter, out paginationInfo)
                .Select(Mapper.Map<StudentWorkPositionListViewDto>);

            return new Result<StudentWorkPositionsPagedDto>(new StudentWorkPositionsPagedDto()
            {
                PaginationInfo = paginationInfo,
                WorkPositions = workPositions,
            });
        }

        /// <summary>
        /// Get the work position for student view. The view statistic is saved.
        /// </summary>
        public WorkPosition GetStudentView(int workPositionId, int studentId)
        {
            var workPosition = WorkPositionController.GetById(workPositionId);
            if (workPosition != null && workPosition.BasicInfo.Publication <= DateTime.Now && workPosition.BasicInfo.Expiration >= DateTime.Now)
                studentWorkPositionViewedController.Add(studentId, workPositionId);

            return workPosition;
        }

        /// <summary>
        /// Show interest to work position. The model has to correspond to the work position.
        /// </summary>
        /// <param name="model">Model that represents the student interest.</param>
        public Result ShowInterest(int studentId, ShowInterest model)
        {
            if (studentShownInterestContorller.HasShownInterest(model.WorkPositionId, studentId))
                return new Result(new ConflictException("Already shown interest"));
            var workPosition = WorkPositionController.GetById(model.WorkPositionId);
            if (workPosition.CandidateRequest.CoverLetter && model.CoverLetter.IsNullOrWhiteSpace())
                return new Result(new BadRequestException("Cover letter can not be empty"));
            if (!workPosition.CandidateRequest.AdditionalQuestions.IsNullOrWhiteSpace() && model.CoverLetter.IsNullOrWhiteSpace())
                return new Result(new BadRequestException("Aditional question can not be empty"));


            studentShownInterestContorller.Add(studentId, model);
            return new Result();
        }

        /// <summary>
        /// Returns the page of work postions for given company.
        /// </summary>
        /// <param name="companyUser">Company user. If the comapny use is not company admin then returns only assigned work positions.</param>
        /// <param name="pageNo">Number of page.</param>
        /// <param name="current">If returns the current work positions or expired.</param>
        public Result<CompanyWorkPositionsPagedDto> GetCompanyWorkPositionsPaged(CompanyUser companyUser, int pageNo, bool current)
        {
            var filter = new WorkPositionFilter()
            {
                CompanyId = companyUser.CompanyId,
                CompanyUserId = companyUser.Role != Role.CompanyAdmin ? companyUser.Member.Id : (int?)null,
                Active = current,
                IncludeHidden = true,
                IncludeStats = true,
            };
            IPaginationInfo paginationInfo;
            var workPositions = WorkPositionController.GetPaged(pageNo, settings.WorkPositionsPerPage, filter, out paginationInfo)
                .Select(Mapper.Map<CompanyWorkPositionListViewDto>);

            return new Result<CompanyWorkPositionsPagedDto>(new CompanyWorkPositionsPagedDto()
            {
                PaginationInfo = paginationInfo,
                WorkPositions = workPositions,
            });
        }

        /// <summary>
        /// Return the current state of work position.
        /// </summary>
        /// <param name="workPositionId">Id of work position.</param>
        public Result<WorkPositionUpdateDto> GetCompanyWorkPositionDetail(CompanyUser companyUser, int workPositionId)
        {
            var workPosition = WorkPositionController.GetById(workPositionId);
            if (workPosition == null || !workPosition.HasAccess(companyUser))
                return new Result<WorkPositionUpdateDto>(new NotFoundException($"Work position with id '{workPositionId}' not found"));
            return new Result<WorkPositionUpdateDto>(Mapper.Map<WorkPositionUpdateDto>(workPosition));
        }

        /// <summary>
        /// Returns the page of students that shown interest for given work position.
        /// </summary>
        /// <param name="workPositionId">Id of the work posiotion.</param>
        /// <param name="page">Page number to display.</param>
        public Result<WorkPositionStudentsPagedDto> GetWorkPositionStudents(CompanyUser companyUser, int workPositionId, int pageNo)
        {
            if (!WorkPositionController.ValidateWorkPosition(companyUser, workPositionId))
                return new Result<WorkPositionStudentsPagedDto>(new NotFoundException($"Work position with id '{workPositionId}' not found"));
            var filter = new StudentFilter()
            {
                WorkPositionId = workPositionId,
            };
            IPaginationInfo paginationInfo;
            var students = studentController.GetPaged(pageNo, settings.WorkPositionsPerPage, filter, out paginationInfo)
                .Select(Mapper.Map<WorkPositionStudentListViewDto>);

            return new Result<WorkPositionStudentsPagedDto>(new WorkPositionStudentsPagedDto()
            {
                PaginationInfo = paginationInfo,
                Students = students,
            });
        }

        /// <summary>
        /// Returns the page of students sorted by match score.
        /// </summary>
        /// <param name="filter">The filter object representing the match criteria.</param>
        public Result<WorkPositionStudentsPagedDto> SearchStudentPaged(CompanyUser companyUser, SearchStudentFilterDto filterDto)
        {
            var workPosition = Mapper.Map<WorkPosition>(filterDto);
            IPaginationInfo paginationInfo = null;
            var workPositionDb = WorkPositionController.GetPaged(1, 1, new WorkPositionFilter() { Hidden = true, CompanyId = companyUser.CompanyId }, out paginationInfo)?.FirstOrDefault();
            if (workPositionDb != null)
                workPosition.WorkPositionId = workPositionDb.WorkPositionId;
            Update(workPosition);

            var filter = new StudentFilter()
            {
                CompanyId = companyUser.CompanyId,
                HiddenWorkPositionId = workPosition.WorkPositionId,
            };
            var students = studentController.GetPaged(filterDto.PageNo, settings.WorkPositionsPerPage, filter, out paginationInfo)
                .Select(Mapper.Map<WorkPositionStudentListViewDto>);

            return new Result<WorkPositionStudentsPagedDto>(new WorkPositionStudentsPagedDto()
            {
                PaginationInfo = paginationInfo,
                Students = students,
            });
        }

        /// <summary>
        /// Return the preview of the work position on the given model. The preview is the same as the one the student will see after the publication.
        /// </summary>
        /// <param name="model">State of the work position to display.</param>
        public Result<string> GetWorkPositionDetail(Company company, WorkPositionUpdateDto model)
        {
            var workPosition = Mapper.Map<WorkPosition>(model);
            var dto = new WorkPositionDetailDto()
            {
                WorkPosition = workPosition,
                Company = company
            };
            return new Result<string>(settings.RenderWorkPosition(dto));
        }

        /// <summary>
        /// Return the intervals in which company can create the new work positions.
        /// </summary>
        /// <param name="workPositionId">Id of work position to ignore.</param>
        public Result<IEnumerable<Interval>> GetCompanyFreeSlots(int companyId, int? workPositionId)
        {
            return new Result<IEnumerable<Interval>>(GetFreeSlots(companyId, DateTime.MaxValue, workPositionId));
        }

        /// <summary>
        /// Anonymize the work position data. Do not delete the work position for statistics use.
        /// </summary>
        public void AnonymizeWorkPosition(int companyId)
        {
            foreach (var workPosition in WorkPositionController.GetByCompanyId(companyId))
            {
                workPosition.AnonymizeData();
                Update(workPosition);
            }
        }

        /// <summary>
        /// Updates the work position if there are no conflicts with other work positions and expiration date is correctly set up.
        /// </summary>
        Result<WorkPositionUpdateDto> Update(int companyId, WorkPosition workPosition)
        {
            workPosition.CompanyId = companyId;
            bool isHidden = workPosition.Visibility?.Hidden ?? false;

            if (workPosition.WorkPositionId > 0)
            {
                var oldWorkPosition = WorkPositionController.GetById(workPosition.WorkPositionId);
                if (oldWorkPosition == null)
                    return new Result<WorkPositionUpdateDto>(new NotFoundException($"Work position with id {workPosition.WorkPositionId} not found"));
                if (oldWorkPosition.CompanyId != workPosition.CompanyId)
                    return new Result<WorkPositionUpdateDto>(new AccessDeniedException($"Access denied"));

                // Check work position max duration
                if (workPosition.BasicInfo != null)
                {
                    if (oldWorkPosition.BasicInfo.Publication < DateTime.Now)
                        workPosition.BasicInfo.Publication = oldWorkPosition.BasicInfo.Publication;

                    if (workPosition.BasicInfo.Expiration < DateTime.Now)
                        return new Result<WorkPositionUpdateDto>(new AccessDeniedException("Expired work position can not be modified"));
                }
                else if (oldWorkPosition.BasicInfo.Expiration < DateTime.Now)
                    return new Result<WorkPositionUpdateDto>(new AccessDeniedException("Expired work position can not be modified"));

                if (workPosition.Visibility == null)
                    isHidden = oldWorkPosition.Visibility.Hidden;
            }
            if (workPosition.BasicInfo != null && workPosition.BasicInfo.Expiration > workPosition.BasicInfo.Publication.AddMonths(settings.WorkPositionMaxDuration))
                return new Result<WorkPositionUpdateDto>(new BadRequestException("Work position max duration has been exceeded"));

            using (var scope = scopeProvider.CreateScope())
            {
                if (!isHidden && workPosition.BasicInfo != null && !HasFreeSlot(companyId, workPosition))
                    return new Result<WorkPositionUpdateDto>(new ConflictException("More active work positions than allowed"));
                Update(workPosition);
                scope.Complete();
            }

            return new Result<WorkPositionUpdateDto>(Mapper.Map<WorkPositionUpdateDto>(workPosition));
        }

        /// <summary>
        /// Check if the work position can be created on the given interval.
        /// </summary>
        private bool HasFreeSlot(int companyId, WorkPosition workPosition)
        {
            var start = workPosition.BasicInfo.Publication < DateTime.Now.Date ? DateTime.Now : workPosition.BasicInfo.Publication;
            var slot = GetFreeSlots(companyId, workPosition.BasicInfo.Expiration, workPosition.WorkPositionId)
                .FirstOrDefault(x => x.Start <= start && x.End >= workPosition.BasicInfo.Expiration);
            return slot != null;
            
        }

        /// <summary>
        /// Returns the intervals at witch the company can create new work positions due to purchased company types.
        /// </summary>
        /// <param name="companyId">Id of the company.</param>
        /// <param name="to">Until when calculate the intervals.</param>
        /// <param name="workPositionId">Id of the work position to ignore.</param>
        /// <returns></returns>
        private IEnumerable<Interval> GetFreeSlots(int companyId, DateTime to, int? workPositionId = null)
        {
            var workPositions = WorkPositionController.GetCurrentByCompanyId(companyId, to)
                        .Where(x => x.WorkPositionId != workPositionId);

            var result = new List<Interval>();
            var start = DateTime.Now.Date;
            var beginnings = workPositions.ToDictionaryWithMerge(x => x.BasicInfo.Publication < start ? start : x.BasicInfo.Publication, x => 1, (agg, x) => ++agg);
            var ends = workPositions.ToDictionaryWithMerge(x => x.BasicInfo.Expiration, x => -1, (agg, x) => --agg);

            var merge = beginnings.Concat(ends).ToDictionaryWithMerge(x => x.Key, x => x.Value, (agg, pair) => agg + pair.Value).OrderBy(x => x.Key);
            var pivots = companyCompanyTypeController.GetCompanyActive(companyId, to).ToArray();
            if (!pivots.Any())
                return Enumerable.Empty<Interval>();
            var pivotIndex = 0;
            var pivot = pivots[pivotIndex];
            pivot.ActiveFrom = pivot.ActiveFrom < start ? start : pivot.ActiveFrom;
            int count = 0;
            Interval interval = null;
            foreach (var item in merge)
            {
                if (item.Key >= to)
                {
                    if (interval != null)
                    {
                        interval.End = item.Key;
                        result.Add(interval);
                    }
                    break;
                }
                if (item.Key > pivot.ActiveTo)
                {
                    bool hasNext = pivotIndex + 1 < pivots.Length;
                    if (!hasNext)
                    {
                        if (interval != null)
                        {
                            interval.End = pivot.ActiveTo;
                            result.Add(interval);
                        }
                        break;
                    }
                    if (interval != null && count > 0 && (pivot.ActiveTo - pivots[pivotIndex + 1].ActiveFrom) > TimeSpan.FromDays(1))
                    {
                        interval.End = pivot.ActiveTo;
                        result.Add(interval);
                        interval = null;
                    }
                    pivotIndex++;
                    pivot = pivots[pivotIndex];
                }
                if (interval == null && item.Key > pivot.ActiveFrom && count < pivot.NumberOfWorkPosition)
                {
                    interval = new Interval()
                    {
                        Start = pivot.ActiveFrom,
                    };
                }
                count += item.Value;
                if (interval != null && count >= pivot.NumberOfWorkPosition)
                {
                    interval.End = item.Key;
                    result.Add(interval);
                    interval = null;
                }
                if (interval == null && count < pivot.NumberOfWorkPosition)
                {
                    interval = new Interval()
                    {
                        Start = item.Key,
                    };
                }
            }
            if (interval == null)
            {
                interval = new Interval()
                {
                    Start = pivot.ActiveFrom,
                };
            }
            for (int i = pivotIndex + 1; i < pivots.Length; i++)
            {
                if ((pivot.ActiveTo - pivots[i].ActiveFrom) > TimeSpan.FromDays(1))
                {
                    interval.End = pivot.ActiveTo;
                    result.Add(interval);
                    interval = new Interval()
                    {
                        Start = pivots[i].ActiveFrom,
                    };
                }
                pivot = pivots[i];
            }
            if (interval != null)
            {
                interval.End = pivot.ActiveTo;
                result.Add(interval);
            }
            return result;
        }
    }
}
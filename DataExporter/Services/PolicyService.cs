using DataExporter.Dtos;
using DataExporter.Entities;
using DataExporter.Models;
using DataExporter.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataExporter.Services
{
    internal class PolicyService : IPolicyService
    {
        private ExporterDbContext _dbContext;

        public PolicyService(ExporterDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
        }

        /// <summary>
        /// Creates a new policy from the DTO.
        /// </summary>
        /// <param name="policy"></param>
        /// <returns>Returns a ReadPolicyDto representing the new policy, if succeded. Returns null, otherwise.</returns>
        public async Task<ReadPolicyDto?> CreatePolicyAsync(CreatePolicyDto createPolicyDto, CancellationToken cancellationToken)
        {
            var policy = MapToPolicyEntity(createPolicyDto);

            await _dbContext
                .Policies
                .AddAsync(policy, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return await ReadPolicyAsync(policy.Id, cancellationToken);
        }

        /// <summary>
        /// Retrives all policies.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a list of ReadPoliciesDto.</returns>
        public async Task<IList<ReadPolicyDto>> ReadPoliciesAsync(ReadPoliciesFilterRequest? request, CancellationToken cancellationToken)
        {
            var query = _dbContext
                .Policies
                .AsNoTracking()
                .Include(x => x.Notes)
                .AsQueryable();
            
            if (request?.StartDate is not null)
            {
                query = query.Where(x => x.StartDate >= request.StartDate.Value);
            }

            if (request?.EndDate is not null)
            {
                query = query.Where(x => x.StartDate <= request.EndDate.Value);
            }

            var policy = await query
                .ToListAsync(cancellationToken);

            return policy.Select(x => MapToPolicyDto(x)).ToList();
        }

        /// <summary>
        /// Retrieves a policy by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a ReadPolicyDto.</returns>
        public async Task<ReadPolicyDto?> ReadPolicyAsync(int id, CancellationToken cancellationToken)
        {
            var policy = await _dbContext.Policies
                .AsNoTracking()
                .Include(x => x.Notes)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (policy == null)
            {
                return null;
            }

            return MapToPolicyDto(policy);
        }

        private ReadPolicyDto MapToPolicyDto(Policy policy)
        {
            // TODO add automapper
            return new ReadPolicyDto()
            {
                Id = policy.Id,
                PolicyNumber = policy.PolicyNumber,
                Premium = policy.Premium,
                StartDate = policy.StartDate,
                Notes = policy.Notes.Select(x => new NoteDto { Id = x.Id, Text = x.Text }).ToList()
            };
        }

        private Policy MapToPolicyEntity(CreatePolicyDto policyDto)
        {
            // TODO add automapper
            return new Policy()
            {
                PolicyNumber = policyDto.PolicyNumber,
                Premium = policyDto.Premium,
                StartDate = policyDto.StartDate,
                Notes = policyDto.Notes?.Select(x => MapToNoteEntity(x)).ToList() ?? new List<Note>(0)
            };
        }

        private Note MapToNoteEntity(CreateNoteDto noteDto)
        {
            // TODO add automapper
            return new Note()
            {
                Text = noteDto.Text
            };
        }
    }
}

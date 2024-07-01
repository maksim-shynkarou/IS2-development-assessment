using DataExporter.Dtos;
using DataExporter.Models;

namespace DataExporter.Services.Interfaces
{
    public interface IPolicyService
    {
        Task<ReadPolicyDto> CreatePolicyAsync(CreatePolicyDto createPolicyDto, CancellationToken cancellationToken);

        Task<IList<ReadPolicyDto>> ReadPoliciesAsync(ReadPoliciesFilterRequest? request, CancellationToken cancellationToken);

        Task<ReadPolicyDto?> ReadPolicyAsync(int id, CancellationToken cancellationToken);
    }
}

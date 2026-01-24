using BB.CR.Models;
using BB.CR.Providers.Bases;
using BB.CR.Views;
using BB.CR.Views.Criterias;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace BB.CR.Repositories
{
    public interface IDangKyHienMauRepository
    {
        Task<ReturnResponse<List<DangKyHienMauView>>> LoadAsync(DangKyHienMauCriteria criteria, ILogger logger, IMapper mapper, string? userCode, bool isLocal = false);
        Task<ReturnResponse<DangKyHienMauView>> CreateAsync(DangKyHienMau model, ILogger logger, IMapper mapper, string? userCode, string? identityCard);
        Task<ReturnResponse<DangKyHienMauView>> UpdateAsync(long id, DangKyHienMau model, ILogger logger, IMapper mapper);
        Task<ReturnResponse<DangKyHienMauView>> GetAsync(long id, ILogger logger, IMapper mapper);

        Task<ReturnResponse<DonorSignatureInfoView>> GetDonorSignatureAsync(
            long id,
            bool includeImage,
            ILogger logger,
            IMapper mapper,
            string? identityCard);

        Task<ReturnResponse<DonorSignatureInfoView>> SaveDonorSignatureAsync(
            long id,
            DonorSignatureSaveRequest request,
            ILogger logger,
            IMapper mapper,
            string? identityCard,
            string? deviceId);

        Task<ReturnResponse<DonorSignatureInfoView>> GetUserSignatureAsync(
            bool includeImage,
            ILogger logger,
            IMapper mapper,
            string? identityCard);

        Task<ReturnResponse<DonorSignatureInfoView>> SaveUserSignatureAsync(
            DonorSignatureSaveRequest request,
            ILogger logger,
            IMapper mapper,
            string? identityCard,
            string? deviceId);
    }
}

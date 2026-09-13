using Microsoft.Extensions.Caching.Memory;
using Kondongpu.Application.Common.Interfaces;
using Kondongpu.Application.Features.Checkups.ViewModels;
using Kondongpu.Domain.Entities;
using Kondongpu.Domain.ValueObjects;

namespace Kondongpu.Application.Services;
public class CheckupStatusService : ICheckupStatusService
{
    private readonly ICheckupItemCacheService _checkupItemCacheService;

    public CheckupStatusService(
        ICheckupItemCacheService checkupItemCacheService)
    {
        _checkupItemCacheService = checkupItemCacheService;
    }

    public async Task UpdateStatusesAsync(List<CheckupViewModel> checkups)
    {
        var orderCodeMap = await _checkupItemCacheService.GetAllOrderCodesAsync();

        foreach (var checkup in checkups)
        {
            UpdateVisionStatus(checkup, orderCodeMap);
            UpdateDentalStatus(checkup, orderCodeMap);             
        }
    }

    private static void UpdateVisionStatus(
        CheckupViewModel checkup,
        Dictionary<string, HashSet<string>> orderCodeMap)
    {
        if (!orderCodeMap.TryGetValue(CheckupClassValue.VisionScreening, out var visionCodes))
        {
            checkup.HasVisionOrder = false;
            checkup.VisionStatusName = "X";
            return;
        }       

        checkup.HasVisionOrder = checkup.ServiceOrder?.Any(visionCodes.Contains) == true;


        if (!checkup.HasVisionOrder)
        {
            checkup.VisionStatusName = "X";
            return;
        }

        // TODO:

        // เช็คจากตาราง Vision ว่ามีผลหรือยัง
        checkup.HasVisionResult = checkup.Visions?.Any() == true;

        checkup.VisionStatusName = checkup.HasVisionResult ? "Completed" : "Pending";
    }

    private static void UpdateDentalStatus(
       CheckupViewModel checkup,
       Dictionary<string, HashSet<string>> orderCodeMap)
    {
        if (!orderCodeMap.TryGetValue(CheckupClassValue.Dental, out var dentalCodes))
        {
            checkup.HasDentalOrder = false;
            checkup.DentalStatusName = "X";
            return;
        }

        checkup.HasDentalOrder = checkup.ServiceOrder?.Any(dentalCodes.Contains) == true;


        if (!checkup.HasDentalOrder)
        {
            checkup.DentalStatusName = "X";
            return;
        }

        // TODO:

        // เช็คจากตาราง Dental ว่ามีผลหรือยัง
        checkup.HasDentalResult = checkup.Dentals?.Any() == true;

        checkup.DentalStatusName = checkup.HasDentalResult ? "Completed" : "Pending";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using evolib.Services.MasterData;
using evotool.Models;
using evotool.ProtocolModels.GMTool.UnitApi;
using Microsoft.EntityFrameworkCore;

namespace evotool.Services.GMTool
{
    public interface IUnitService
    {
        Task<GetUnitResponse> GetUnit();
        Task<PutUnitTemporaryAvailabilityResponse> PutUnitTemporaryAvailability(string mobileSuitId, PutUnitTemporaryAvailabilityRequest request);
    }

    public class UnitService : BaseService, IUnitService
    {
        private readonly ITranslationService _translationService;

        public UnitService(IServicePack servicePack, ITranslationService translationService) : base(servicePack)
        {
            _translationService = translationService;
        }

        public async Task<GetUnitResponse> GetUnit()
        {
            await MasterDataLoader.LoadAsync();

            var masterData = MasterDataLoader.LatestMasterData;

            if (masterData == null)
            {
                // todo: error message
                throw new Exception("master data not exist");
            }

            var mobileSuitIds = masterData.AllMobileSuitIds.OrderBy(x => x);
            var dictionary = _translationService.GetMobileSuitDictionary();

            var temporarilyDisabledItems = await Common2DB.DisabledMobileSuits.ToListAsync();

            var response = new GetUnitResponse();
            response.units = new List<GetUnitResponse.Unit>();

            foreach (var mobileSuitId in mobileSuitIds)
            {
                var mobileSuit = masterData.GetMobileSuit(mobileSuitId);

                if (mobileSuit == null) continue;

                TranslationItemCommon translationItem;
                dictionary.TryGetValue(mobileSuitId, out translationItem);

                var isEnabledInGmTool = !temporarilyDisabledItems.Any(x => x.itemId == mobileSuit.itemId);

                response.units.Add(new GetUnitResponse.Unit
                {
                    mobileSuitId = mobileSuitId,
                    displayNameJapanese = translationItem?.JpText ?? mobileSuitId,
                    displayNameEnglish = translationItem?.EnText ?? mobileSuitId,
                    isEnabledOnMasterData = mobileSuit.availabled,
                    isEnabledOnGmTool = isEnabledInGmTool,
                    isAvailable = mobileSuit.availabled && isEnabledInGmTool,
                });
            }

            return response;
        }

        public async Task<PutUnitTemporaryAvailabilityResponse> PutUnitTemporaryAvailability(string mobileSuitId, PutUnitTemporaryAvailabilityRequest request)
        {
            var response = new PutUnitTemporaryAvailabilityResponse();

            await MasterDataLoader.LoadAsync();

            var masterData = MasterDataLoader.LatestMasterData;

            if (masterData == null)
            {
                // todo: error message
                throw new Exception("master data not exist");
            }

            var mobileSuit = masterData.GetMobileSuit(mobileSuitId);

            if (mobileSuit == null)
            {
                // todo: error message
                throw new BadRequestException("unit not exist");
            }

            var temporarilyDisabledItem = await Common2DB.DisabledMobileSuits.Where(r => r.itemId == mobileSuit.itemId).FirstOrDefaultAsync();

            if (request.isEnabledOnGmTool)
            {
                if (temporarilyDisabledItem != null)
                {
                    Common2DB.DisabledMobileSuits.Remove(temporarilyDisabledItem);
                    await Common2DB.SaveChangesAsync();
                }
            }
            else
            {
                if (temporarilyDisabledItem == null)
                {
                    await Common2DB.DisabledMobileSuits.AddAsync(
                        new evolib.Databases.common2.DisabledMobileSuit
                        {
                            itemId = mobileSuit.itemId,
                        });
                    await Common2DB.SaveChangesAsync();
                }
            }

            return response;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using evolib.Services.MasterData;
using evotool.Models;
using evotool.ProtocolModels.GMTool.MiscApi;

namespace evotool.Services.GMTool
{
    public interface IMiscService
    {
        Task<GetSeasonListResponse> GetSeasonList();
        Task<GetMobileSuitListResponse> GetMobileSuitList();
    }

    public class MiscService : BaseService, IMiscService
    {
        private readonly ITranslationService _translationService;

        public MiscService(IServicePack servicePack, ITranslationService translationService) : base(servicePack)
        {
            _translationService = translationService;
        }

        public async Task<GetSeasonListResponse> GetSeasonList()
        {
            await MasterDataLoader.LoadAsync();

            var masterData = MasterDataLoader.LatestMasterData;

            if (masterData == null)
            {
                // todo: error message
                throw new Exception("master data not exist");
            }

            var dictionary = _translationService.GetSeasonDictionary();

            var seasons = masterData.AllSeasons.OrderBy(x => x.seasonNo);

            var response = new GetSeasonListResponse();
            response.seasons = new List<GetSeasonListResponse.Season>();

            foreach (var season in seasons)
            {
                if (season.seasonNo == 0)
                {
                    response.seasons.Add(new GetSeasonListResponse.Season
                    {
                        seasonNo = season.seasonNo,
                        displayNameJapanese = "カジュアル",
                        displayNameEnglish = "Casual",
                    });
                }
                else
                {
                    TranslationItemCommon translationItem;
                    dictionary.TryGetValue(season.seasonNo.ToString(), out translationItem);

                    response.seasons.Add(new GetSeasonListResponse.Season
                    {
                        seasonNo = season.seasonNo,
                        displayNameJapanese = translationItem?.JpText ?? season.seasonNo.ToString(),
                        displayNameEnglish = translationItem?.EnText ?? season.seasonNo.ToString(),
                    });
                }
            }

            return response;
        }

        public async Task<GetMobileSuitListResponse> GetMobileSuitList()
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

            var response = new GetMobileSuitListResponse();
            response.mobileSuits = new List<GetMobileSuitListResponse.MobileSuit>();

            foreach (var mobileSuitId in mobileSuitIds)
            {
                var mobileSuit = masterData.GetMobileSuit(mobileSuitId);

                if (mobileSuit == null) continue;

                TranslationItemCommon translationItem;
                dictionary.TryGetValue(mobileSuitId, out translationItem);

                response.mobileSuits.Add(new GetMobileSuitListResponse.MobileSuit
                {
                    mobileSuitId = mobileSuitId,
                    displayNameJapanese = translationItem?.JpText ?? mobileSuitId,
                    displayNameEnglish = translationItem?.EnText ?? mobileSuitId,
                });
            }

            return response;
        }
    }
}

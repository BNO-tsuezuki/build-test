using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Amazon.Lambda.Core;
using MasterDataProcess.Models.Translation;

namespace MasterDataProcess.Models.Master
{
    public class MasterData
    {
        public IList<MobileSuit> MobileSuit { get; private set; }
        public IList<Tutorial> Tutorial { get; private set; }
        public IList<Level> Level { get; private set; }

        private MasterDataJson _master;
        private TranslationDataJson _translation;

        public MasterData(MasterDataJson masterData, TranslationDataJson translationDataJson)
        {
            _master = masterData;
            _translation = translationDataJson;

            BuildMobileSuitMaster();
            BuildTutorialMaster();
            BuildLevelMaster();
        }

        private void BuildMobileSuitMaster()
        {
            var mobileSuitJsonList = GetRootItem("MasterData_Character");

            if (mobileSuitJsonList == null) return;

            MobileSuit = mobileSuitJsonList
                .Select(m =>
                {
                    var mobileSuit = JsonSerializer.Deserialize<MobileSuit>(m.GetRawText());

                    mobileSuit.TranslatedName = _translation.MobileSuit.Texts
                        .FirstOrDefault(x => x.Id == mobileSuit.Name)?.JpText ?? mobileSuit.Name;

                    var equipmentJsonList = GetChildItem(mobileSuit.EquipmentsDataTable);

                    if (equipmentJsonList == null) return mobileSuit;

                    mobileSuit.Equipment = equipmentJsonList
                        .Select(w =>
                        {
                            var equipment = JsonSerializer.Deserialize<Equipment>(w.GetRawText());

                            equipment.TranslatedName = _translation.Weapon.Texts
                                .FirstOrDefault(x => x.Id == equipment.Name)?.JpText ?? equipment.Name;

                            return equipment;
                        })
                        .ToList();

                    return mobileSuit;
                })
                .ToList();
        }

        private void BuildTutorialMaster()
        {
            Tutorial = _translation.Tutorial.Texts
                .Select(x => new Tutorial
                {
                    Id = x.Id,
                    TranslatedName = x.JpText,
                })
                .OrderBy(x => int.Parse(x.Id))
                .ToList();
        }

        private void BuildLevelMaster()
        {
            Level = _translation.Level.Texts
                .Select(x => new Level
                {
                    Id = x.Id,
                    TranslatedName = x.JpText,
                })
                .OrderBy(x => x.Id)
                .ToList();
        }

        private IList<JsonElement> GetRootItem(string name)
        {
            var rootItem = _master?.Root?.FirstOrDefault(x => x.Name == name)?.Rows;

            if (rootItem == null) LambdaLogger.Log($"[ERROR] [Master] Root item not found. Name: {name}");

            return rootItem;
        }

        private IList<JsonElement> GetChildItem(string dataTableName)
        {
            var match = Regex.Match(dataTableName, "DataTable'([^']+)'");

            if (match.Groups.Count < 2) return null;

            var key = match.Groups[1].Value;

            var childItem = _master?.Children[key]?.Rows;

            if (childItem == null) LambdaLogger.Log($"[ERROR] [Master] Children item not found. Key: {dataTableName}");

            return childItem;
        }
    }
}

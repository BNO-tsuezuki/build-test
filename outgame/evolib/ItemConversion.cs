using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.EntityFrameworkCore;


namespace evolib
{
    public static class ItemConversion
    {
        public enum Result
        {
            Err,
            Ok,
        }

        public class ConvertModel
        {
            public Result result { get; set; }
            public GiveAndTake.Model model { get; set; }
        }

        public static string AssetsType { get { return "MP"; } }

        public static ConvertModel GiveConvertModel(
                                            Services.MasterData.IMasterData masterData, 
                                            string itemId)
        {
            var r = new ConvertModel();

            var item = masterData.GetItemFromItemId(itemId);
            if (item == null)
            {
                r.result = Result.Err;
                return r;
            }
            var materialConversion = masterData.GetMaterialConversion(item.rankType);
            if (materialConversion == null)
            {
                r.result = Result.Err;
                return r;
            }

            var assetsInfo = masterData.GetAssetsInfoByType(AssetsType);
            if (assetsInfo == null)
            {
                r.result = Result.Err;
                return r;
            }

            r.result = Result.Ok;
            r.model = new GiveAndTake.Model
            {
                type = GiveAndTake.Type.Assets,
                assetsId = assetsInfo.assetsId,
                itemId = "",
                amount = materialConversion.value,
            };

            return r;
        }
    }
}

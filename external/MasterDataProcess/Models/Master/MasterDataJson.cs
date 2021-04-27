using System.Collections.Generic;
using System.Text.Json;

namespace MasterDataProcess.Models.Master
{
    public class MasterDataJson
    {
        public IList<long> Version { get; set; }
        public IList<RootItem> Root { get; set; }
        public IDictionary<string, ChildrenItem> Children { get; set; }
    }

    public class RootItem
    {
        public string Name { get; set; }
        public IList<JsonElement> Rows { get; set; }
    }

    public class ChildrenItem
    {
        public string Name { get; set; }
        public IList<JsonElement> Rows { get; set; }
    }
}

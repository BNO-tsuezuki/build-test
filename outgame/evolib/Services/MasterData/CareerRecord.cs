using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolib.Services.MasterData
{
	public interface ICareerRecord
	{
		string recordItemId { get; }
		evolib.CareerRecord.ValueType valueType { get; }
		evolib.CareerRecord.CategoryType categoryType { get; }
		evolib.CareerRecord.FormatType formatType { get; }
		string mobileSuitId { get; }
	}

	public class CareerRecord : ICareerRecord
    {
		public string recordItemId { get; private set; }
		public evolib.CareerRecord.ValueType valueType { get; private set; }
		public evolib.CareerRecord.CategoryType categoryType { get; private set; }
		public evolib.CareerRecord.FormatType formatType { get; private set; }
		public string mobileSuitId { get; }

		//----
		public CareerRecord(string recordItemId,
			evolib.CareerRecord.ValueType valueType,
			evolib.CareerRecord.CategoryType categoryType,
			evolib.CareerRecord.FormatType formatType,
			string mobileSuitId)
		{
			this.recordItemId = recordItemId;
			this.valueType = valueType;
			this.categoryType = categoryType;
			this.formatType = formatType;
			this.mobileSuitId = mobileSuitId;
		}
	}
}

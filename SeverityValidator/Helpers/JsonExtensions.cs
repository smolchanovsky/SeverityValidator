using Newtonsoft.Json;

namespace SeverityValidator.Helpers
{
	internal static class Json
	{
		private const string nullJsonStr = "null";

		public static string ToJson(this object obj)
		{
			if (obj == null)
				return nullJsonStr;
			return JsonConvert.SerializeObject(obj);
		}
	}
}

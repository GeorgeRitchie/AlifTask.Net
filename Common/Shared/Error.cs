namespace AlifTask.Common.Shared
{
	public class Error
	{
		public string Key { get; set; }
		public string Description { get; set; }

		public Error(string key, string description)
		{
			Key = key;
			Description = description;
		}
	}
}

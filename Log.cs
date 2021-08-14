using System;

namespace backend
{
	public class Log
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();

		public DateTime createdTime { get; set; } = DateTime.UtcNow;

		public string text { get; set; }
	}

	public class CreateLog
	{
		public string text { get; set; }
	}

	public static class LogExtensions
	{
		public static LogEntry ToTable(this Log log)
		{
			return new LogEntry
			{
			PartitionKey = "TODO",
			RowKey = log.Id,
			createdTime = log.createdTime,
			text = log.text
			};
		}

		public static Log ToTodo(this LogEntry logEntry)
		{
			return new Log
			{
			Id = logEntry.RowKey,
			createdTime = logEntry.createdTime,
			text = logEntry.text
			};
		}
	}
}
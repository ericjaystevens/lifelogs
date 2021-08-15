using System;
using Microsoft.Azure.Cosmos.Table;

namespace backend
{
	public class LogEntry : TableEntity
	{
		public DateTime createdTime { get; set; }

		public string text { get; set; }
	}
	public class Log
	{
		public string Id { get; set; } = (DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks).ToString("d19");

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
			PartitionKey = "LogEntry",
			RowKey = log.Id,
			createdTime = log.createdTime,
			text = log.text
			};
		}

		public static Log ToLogEntries(this LogEntry logEntry)
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
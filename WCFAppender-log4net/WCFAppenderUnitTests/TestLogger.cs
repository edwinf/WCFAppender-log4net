using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WCFAppender_log4net.Interface;

namespace WCFAppenderUnitTests
{
	public class TestLogger : IWCFLogger
	{
		public string[] LastLogOutput;

		public void Append(log4net.Core.LoggingEvent[] logEvents)
		{
			LastLogOutput = new string[logEvents.Length];
			for(int i=0;i<logEvents.Length;i++)
			{
				LastLogOutput[i] = "SERVER - " + logEvents[i].RenderedMessage;
			}
		}

		public void Append(string[] logEntries)
		{
			LastLogOutput = logEntries;
		}
	}
}

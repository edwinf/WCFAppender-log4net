using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFAppender_log4net.Interface
{
	interface IWCFLogger
	{
		[OperationContract(Name="AppendLoggingEvents", IsOneWay=true)]
		void Append(log4net.Core.LoggingEvent[] logEvents);

		[OperationContract(Name="AppendRenderedLogEntries",IsOneWay=true)]
		void Append(string[] logEntries);
	}
}

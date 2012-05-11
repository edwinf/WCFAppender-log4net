using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFAppender_log4net.Interface
{
	[ServiceContract]
	public interface IWCFLogger
	{
		/// <summary>
		/// This is the prefered method to use as it gives complete control through log4net on the server side how messages are going to 
		/// be rendered and stored.  
		/// </summary>
		/// <param name="logEvents"></param>
		[OperationContract(Name="AppendLoggingEvents", IsOneWay=true)]
		void Append(log4net.Core.LoggingEvent[] logEvents);

		/// <summary>
		/// This method will translate the message on the client side and sent to the server as a string.  The server side 
		/// of this implementation will most likely require custom programming.
		/// </summary>
		/// <param name="logEntries"></param>
		[OperationContract(Name="AppendRenderedLogEntries",IsOneWay=true)]
		void Append(string[] logEntries);
	}
}

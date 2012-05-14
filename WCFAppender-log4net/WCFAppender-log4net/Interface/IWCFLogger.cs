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
		[UseNetDataContractSerializer]
		void Append(LoggingEventWrapper[] logEvents);

		/// <summary>
		/// This method will translate the message on the client side and sent to the server as a string.  The server side 
		/// of this implementation will most likely require custom programming.
		/// </summary>
		/// <param name="logEntries"></param>
		[OperationContract(Name="AppendRenderedLogEntries",IsOneWay=true)]
		void Append(string[] logEntries);
	}
}



/*
 *  Copyright © 2012 edwinf (https://github.com/edwinf)
 *  
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using log4net;
using WCFAppender_log4net.Interface;

namespace WCFServices
{
	public class WCFLogger : IWCFLogger
	{
		private ILog log = LogManager.GetLogger(typeof(WCFLogger));

		/// <summary>
		/// Log the remote events directly to the currently defined logger
		/// </summary>
		/// <param name="logEvents">serialized logging events from the client</param>
		public void Append(log4net.Core.LoggingEvent[] logEvents)
		{
			for(int i=0;i<logEvents.Length;i++)
			{
				log.Logger.Log(logEvents[i]);
			}
		}

		/// <summary>
		/// Log the pre-rendered string entries to *******
		/// </summary>
		/// <param name="logEntries">The pre-rendered log entries</param>
		public void Append(string[] logEntries)
		{
			//TODO: this is a placeholder.  Please implement your specific logic here if you plan to use this function, 
			//or use a render layout that just uses the message itself.  The possible issue is that you depend entirely on the filter from 
			//your client as do not have any concept of level, filters, etc.

			for(int i=0;i<logEntries.Length;i++)
			{
				log.Debug(logEntries);
			}
		}
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
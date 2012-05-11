
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net.Appender;
using log4net.Util;
using WCFAppender_log4net.Interface;

namespace WCFAppender_log4net
{
	public class WCFAppender : BufferingAppenderSkeleton
	{
		Type typeDescriptor = typeof(WCFAppender);
		private IWCFLogger _LoggingService;
		public string URL { get; set; }
		public bool RenderOnClient { get; set; }

		internal IWCFLogger LoggingChannel
		{
			get
			{
				return _LoggingService;
			}
			set
			{
				_LoggingService = value;
			}
		}

		public WCFAppender()
		{
			RenderOnClient = true;
		}

		public override void ActivateOptions()
		{
			base.ActivateOptions();
			CreateChannel();
		}

		protected override void OnClose()
		{
			base.OnClose();
			IClientChannel channel = _LoggingService as IClientChannel;
			if (channel != null)
			{
				channel.Dispose();
			}
		}

		protected override void SendBuffer(log4net.Core.LoggingEvent[] events)
		{
			if (ConfirmChannelAcceptable())
			{
				try
				{
					if (this.RenderOnClient)
					{
						string[] logs = new string[events.Length];
						for (int i = 0; i < events.Length; i++)
						{
							logs[i] = base.RenderLoggingEvent(events[i]);
						}
						_LoggingService.Append(logs);
					}
					else
					{
						_LoggingService.Append(events);
					}
				}
				catch (Exception ex)
				{
					LogLog.Error(typeDescriptor, "Error sending log events to remoe endpoint", ex);
				}
			}
			else
			{
				LogLog.Warn(typeDescriptor, "WCF Channel cannot be confirmed as acceptable, not sending events");
			}
		}

		private bool CreateChannel()
		{
			LogLog.Debug(typeDescriptor, "Creating WCF Channel");
			bool ret = false;
			try
			{
				EndpointAddress address = new EndpointAddress(this.URL);
				Binding binding = ChooseBinding();
				ChannelFactory<IWCFLogger> factory = new ChannelFactory<IWCFLogger>(binding, address);
				_LoggingService = factory.CreateChannel();
				ret = true;
				LogLog.Debug(typeDescriptor, "WCF Channel Created Successfully");
			}
			catch (Exception ex)
			{
				LogLog.Error(typeDescriptor, "Error creating WCF Channel", ex);
			}
			return ret;
		}

		private Binding ChooseBinding()
		{
			return new BasicHttpBinding();
		}

		private bool ConfirmChannelAcceptable()
		{
			bool ret = true;
			if (_LoggingService == null)
			{
				ret = CreateChannel();
			}
			else
			{
				IClientChannel channel = _LoggingService as IClientChannel;
				if (channel != null && channel.State != CommunicationState.Opened)
				{
					LogLog.Debug(typeDescriptor, "Channel not in a good state, disposing and creating a new one");
					channel.Dispose();
					return CreateChannel();
				}
			}
			return ret;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
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
		private Type typeDescriptor = typeof(WCFAppender);
		private IWCFLogger _LoggingService;

		/// <summary>
		/// The URL to send the information to
		/// </summary>
		public string URL { get; set; }

		/// <summary>
		/// Whether to render the log event on the client or render the event on the server. Rendering is the act of transforming the log
		/// objects and exceptions to the log message.  Either way, the originating information from the client will always be used. 
		/// Defaults to False
		/// </summary>
		public bool RenderOnClient { get; set; }

		/// <summary>
		/// The WCF binding to use.  HTTP or NETTCP.  Defaults to HTTP
		/// </summary>
		public BindingType BindingType { get; set; }
		
		/// <summary>
		/// If the configuration is non-basic, you can configure additional properties in the app config and give the binding a name.  Passing the name
		/// in this parameter will create the configuration with that named value.
		/// </summary>
		public string BindingConfigurationName { get; set; }

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
			RenderOnClient = false;
			this.BindingType = WCFAppender_log4net.BindingType.HTTP;
		}

		public override void ActivateOptions()
		{
			base.ActivateOptions();

			if (this.RenderOnClient)
			{
				this.Fix = log4net.Core.FixFlags.All;
			}
			else
			{
				//don't attempt to render the message or exception on the client
				this.Fix = log4net.Core.FixFlags.All ^ log4net.Core.FixFlags.Message ^ log4net.Core.FixFlags.Exception;
			}
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
						LoggingEventWrapper[] wrappers = new LoggingEventWrapper[events.Length];
						for (int i = 0; i < events.Length; i++)
						{
							wrappers[i] = new LoggingEventWrapper(events[i]);
						}
						_LoggingService.Append(wrappers);
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
			if (this.BindingType == WCFAppender_log4net.BindingType.NETTCP)
			{
				if (String.IsNullOrWhiteSpace(this.BindingConfigurationName))
				{
					return new NetTcpBinding();
				}
				else
				{
					return new NetTcpBinding(this.BindingConfigurationName);
				}
			}
			else
			{
				if (String.IsNullOrWhiteSpace(this.BindingConfigurationName))
				{
					return new BasicHttpBinding();
				}
				else
				{
					return new BasicHttpBinding(this.BindingConfigurationName);
				}
			}
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
				if (channel != null && (channel.State == CommunicationState.Faulted || channel.State == CommunicationState.Closed))
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
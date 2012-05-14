using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Core;
using log4net.Repository;
using log4net.Util;

namespace WCFAppender_log4net.Interface
{
	/// <summary>
	/// This class is a wrapper for the log4net LoggingEvent to support the serialization and transport of a cmoplete LoggingEvent object across a WCF boundry.
	/// The log4net LoggingEvent does not transport the message object or the Exception object during serialization, so this wrapper sucks those objects out and keeps
	/// them seperate.  Then on the other side of a seriLization call, it can reconstruct the correct logging event for rendering.
	/// </summary>
	[Serializable]
	public class LoggingEventWrapper
	{
		private LoggingEvent _LoggingEvent;
		private object _MessageObject;
		private Exception _LoggedException;

		public LoggingEventWrapper(LoggingEvent ev)
		{
			_LoggingEvent = ev;	
			_MessageObject = ev.MessageObject;
			_LoggedException = ev.ExceptionObject;
		}

		public LoggingEvent GetReconstructedLoggingEvent(ILoggerRepository renderingRepo)
		{
			LoggingEvent ret = new LoggingEvent(null, renderingRepo, null, _LoggingEvent.Level, _MessageObject, _LoggedException);

			BindingFlags eFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			FieldInfo pi;

			LoggingEventData data = _LoggingEvent.GetLoggingEventData(FixFlags.None);
			//reset message so it gets rendered again 
			data.Message = null;

			pi = ret.GetType().GetField("m_data", eFlags);
			if (pi != null)
			{
				pi.SetValue(ret, data);
			}


			//reflectivly set the rest of the properties.
			ret.Fix = FixFlags.Exception | FixFlags.Message;
			return ret;
		}
	}
}

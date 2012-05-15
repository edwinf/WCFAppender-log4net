Log4Net WCF Appender
=================

1. *Overview*
	A log4net appender that pushes log entries to a WCF service. 

2. Sample Config:
<pre><code>&lt;log4net&gt;
	&lt;appender name="WCFAppender" type="WCFAppender_log4net.WCFAppender, WCFAppender-log4net"&gt;
			&lt;BindingType&gt;HTTP&lt;/BindingType&gt;
			&lt;RenderOnClient&gt;True&lt;/RenderOnClient&gt;
			&lt;URL&gt;http://localhost:43573/WCFLogger.svc&lt;/URL&gt;
			&lt;layout type="log4net.Layout.PatternLayout" value="%date [%thread] %-5level %logger - %message%newline" /&gt;
	&lt;/appender&gt;
	&lt;root&gt;
		&lt;level value="ALL" /&gt;
		&lt;appender-ref ref=""WCFAppender"" /&gt;
	&lt;/root&gt;
&lt;/log4net&gt;
</code></pre>

3. The WCF appender has two modes: render on client and render on service.  The render on client takes the log event data 
and renders the log string on the client and sends it to the WCF service as a string.  This is the most interoperable as there is no object 
serialization taking place.  The render on server is a little more complex, but offers the benifit of controlling the rendering in a central place, 
and can be changed without redeploying your client.  The event is serialized to the server using .Net specific seralization, to avoid unknown type errors.  The type DLLs
much match on the client and on the server for server side rendering to work.

The implementations of both functions log to log4net on the server side for ease of configurability; however, this implementation is left open for you 
to change however you'd like.

4. License: 
   This appender is licensed under the apache 2 license.  Which basically means: 
		a) use it / modify it however you want as long as you leave the copyright notice in the source.
		b) If you're just using the DLLs, you don't need to worry about anything.

   Copyright 2012 edwinf (https://github.com/edwinf)
   
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at
 
       http://www.apache.org/licenses/LICENSE-2.0
 
   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
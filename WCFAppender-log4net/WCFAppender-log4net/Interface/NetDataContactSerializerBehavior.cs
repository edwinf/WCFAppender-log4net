using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WCFAppender_log4net.Interface
{
	public class NetDataContractOperationBehavior : DataContractSerializerOperationBehavior
	{
		public NetDataContractOperationBehavior(OperationDescription operation)
			: base(operation)
		{
		}

		public NetDataContractOperationBehavior(OperationDescription operation, DataContractFormatAttribute dataContractFormatAttribute)
			: base(operation, dataContractFormatAttribute)
		{
		}

		public override XmlObjectSerializer CreateSerializer(Type type, string name, string ns,
			 IList<Type> knownTypes)
		{
			return new NetDataContractSerializer(name, ns);
		}

		public override XmlObjectSerializer CreateSerializer(Type type, XmlDictionaryString name,
			 XmlDictionaryString ns, IList<Type> knownTypes)
		{
			return new NetDataContractSerializer(name, ns);
		}
	}

	public class UseNetDataContractSerializerAttribute : Attribute, IOperationBehavior
	{
		public void AddBindingParameters(OperationDescription description, BindingParameterCollection parameters)
		{
		}

		public void ApplyClientBehavior(OperationDescription description,
			 System.ServiceModel.Dispatcher.ClientOperation proxy)
		{
			ReplaceDataContractSerializerOperationBehavior(description);
		}

		public void ApplyDispatchBehavior(OperationDescription description,
			 System.ServiceModel.Dispatcher.DispatchOperation dispatch)
		{
			ReplaceDataContractSerializerOperationBehavior(description);
		}

		public void Validate(OperationDescription description)
		{
		}

		private static void ReplaceDataContractSerializerOperationBehavior(OperationDescription description)
		{
			DataContractSerializerOperationBehavior dcsOperationBehavior =
			description.Behaviors.Find<DataContractSerializerOperationBehavior>();

			if (dcsOperationBehavior != null)
			{
				description.Behaviors.Remove(dcsOperationBehavior);
				description.Behaviors.Add(new NetDataContractOperationBehavior(description));
			}
		}
	}
}

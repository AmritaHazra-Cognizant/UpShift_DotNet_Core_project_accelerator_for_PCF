using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace WFA.ECS.Framework.Core.ExceptionHandling
{
    [DataContract(Name ="WFFault_Type", Namespace ="http://service.wellsfargoadvisors.com/entity/message/2007/")]
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    public class WFFault_Type:IExtensibleDataObject, INotifyPropertyChanged
    {
        public WFFault_Type() { }

        [DataMember(EmitDefaultValue = false, Order = 7, Name = "adviceText")]
        public string AdviceText { get; set; }


        [DataMember(EmitDefaultValue = false, Order = 6, Name = "dataReferance")]
        public DataReferance_Type DataReferance { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 10, Name = "embeddedException")]
        public string EmbeddedException { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 9, Name = "exceptionInstancedId")]
        public string ExceptionInstancedId { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 5, Name = "faultActor")]
        public string FaultActor { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired =true, Name = "faultCode")]
        public string FaultCode { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 3, IsRequired =true, Name = "faultReasonText")]
        public string FaultReasonText { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "faultSubcode")]
        public string FaultSubcode { get; set; }


        [DataMember(IsRequired =true, Name = "falutType")]
        public FaultType_Enum FalutType { get; set; }


        [DataMember(IsRequired = true, Order = 4, Name = "severity")]
        public FaultSeverity_Enum Severity { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 11, Name = "stackTrace")]
        public string StackTrace { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 8, Name = "technicalText")]
        public string TechnicalText { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [CollectionDataContract(Name = "WFFaultList_Type", Namespace = "http://service.wellsfargoadvisors.com/entity/message/2007/", ItemName ="WFFault")]
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    public class WFFaultList_Type: List<WFFault_Type>
    {
        public WFFaultList_Type() { }
    }


    public class DataReferance_Type: IExtensibleDataObject, INotifyPropertyChanged
    {
        public DataReferance_Type() { }

        public ExtensionDataObject ExtensionData { get; set; }

        [DataMember(EmitDefaultValue =false, Name ="referenceId")]
        public string ReferenceId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [DataContract(Name = "FaultType_Enum", Namespace = "http://service.wellsfargoadvisors.com/entity/message/2007/")]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    public enum FaultType_Enum
    {
        APPL = 0,
        SYSTEM = 1
    }

    [DataContract(Name = "FaultSeverity_Enum", Namespace = "http://service.wellsfargoadvisors.com/entity/message/2007/")]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    public enum FaultSeverity_Enum
    {
        INFORMATION = 0,
        WARNING = 1,
        ERROR = 2,
        CRITICAL_ERROR = 3
    }
}

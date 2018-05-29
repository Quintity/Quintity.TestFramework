using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public enum ChangeType
    {
        [EnumMember]
        Unknown,
        [EnumMember]
        Update,
        [EnumMember]
        Add,
        [EnumMember]
        Remove,
        [EnumMember]
        Copy,
        [EnumMember]
        Move
    }

    [DataContract]
    public enum Status
    {
        [EnumMember]
        Incomplete,
        [EnumMember]
        Inactive,
        [EnumMember]
        Active,
        [EnumMember]
        Obsolete,
        [EnumMember, System.ComponentModel.Browsable(false)]
        Unavailable
    }

    [DataContract]
    public enum TestVerdict
    {
        [EnumMember, System.ComponentModel.Browsable(false)]
        Unknown,
        [EnumMember, System.ComponentModel.Browsable(false)]
        Error,
        [EnumMember]
        Fail,
        [EnumMember]
        Pass,
        [EnumMember]
        Inconclusive,
        [EnumMember, System.ComponentModel.Browsable(false)]
        DidNotExecute
    }

    [DataContract]
    public enum TerminationReason
    {
        /// <summary>
        /// Test execution ran to normal completion.
        /// </summary>
        [EnumMember]
        Normal,
        /// <summary>
        /// User or user application initiated termination.
        /// </summary>
        [EnumMember]
        UserInitiated,
        /// <summary>
        /// Terminated by executing test script execution.
        /// </summary>
        [EnumMember]
        RuntimeStopRequest,
        /// <summary>
        /// Termination initiated by failure in test listener (listener configured to terminate).
        /// </summary>
        [EnumMember]
        ListenerError,
        /// <summary>
        /// Termination due to framework runtime execution.
        /// </summary>
        [EnumMember]
        RuntimeException,
        /// <summary>
        /// Termination due to an unhandled exception in test code.
        /// </summary>
        [EnumMember]
        UnhandledException,
        /// <summary>
        /// Termination due to an unknown runtime error.
        /// </summary>
        [EnumMember]
        Unknown
    }

    [DataContract]
    public enum TestType
    {
        [EnumMember]
        Automated,
        [EnumMember]
        Manual
    }

    [DataContract]
    public enum OnFailure
    {
        [EnumMember]
        Continue,
        [EnumMember]
        Stop
    }
}

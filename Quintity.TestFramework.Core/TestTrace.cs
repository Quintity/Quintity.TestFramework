using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestTrace : TestArtifact
    {
        #region Events definition

        /// <summary>
        /// Test trace message event delegate definition.
        /// </summary>
        /// <param name="message">Test trace message.</param>
        public delegate void TestTraceEventHandler(string virtualUser, string traceMessage);

        /// <summary>
        /// Test trace message event definition.
        /// </summary>
        public static event TestTraceEventHandler OnTestTrace;

        internal static void fireTestTraceEvent(string message)
        {
            if (OnTestTrace != null)
            {
                OnTestTrace(Thread.CurrentThread.Name, message);
            }
        }

        #endregion

        public static void Trace(string format, params object[] args)
        {
            Trace(string.Format(format, args));
        }

        /// <summary>
        /// Sets the client application trace callback method.
        /// </summary>
        /// <param name="traceMessage"></param>
        public static void Trace(string traceMessage)
        {
            fireTestTraceEvent(traceMessage);
        }
    }
}

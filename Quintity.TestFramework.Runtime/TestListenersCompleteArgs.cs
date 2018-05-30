using System.Runtime.Serialization;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Runtime
{
    [DataContract]
    public class TestListenersCompleteArgs
    {
        #region Class data members

        #endregion

        #region Class properties

        [DataMember]
        public TerminationReason TerminationSource { get; set; }

        [DataMember]
        public string Explanation { get; set; }

        [DataMember]
        public TestProfile TestProfile { get; set; }

        #endregion

        #region Class constructors

        public TestListenersCompleteArgs()
        { }

        #endregion

        #region Class public methods

        public override string ToString()
        {
            return $"Termination source:  {TerminationSource} \r\nExplanation:  {Explanation}";
        }

        #endregion

        #region Class private methods

        #endregion
    }
}

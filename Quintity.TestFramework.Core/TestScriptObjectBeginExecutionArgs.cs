using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestScriptObjectBeginExecutionArgs
    {
        #region Class data members

        [DataMember]
        private string _virtualUser;

        #endregion

        #region Class properties

        [IgnoreDataMember]
        public string VirtualUser
        { get { return _virtualUser; } }

        #endregion

        #region Class constructors

        public TestScriptObjectBeginExecutionArgs(string virtualUser)
        {
            _virtualUser = virtualUser;
        }

        #endregion

        #region Class public methods

        #endregion

        #region Class private methods

        #endregion
    }
}

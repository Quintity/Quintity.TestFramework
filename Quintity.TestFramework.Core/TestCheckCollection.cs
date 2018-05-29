using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [CollectionDataContract
      (Name = "TestChecks",
      ItemName = "TestCheck")]
    public class TestCheckCollection : List<TestCheck>
    {

         #region Constructor

        internal TestCheckCollection(){ }

        internal TestCheckCollection(TestCheckCollection original)
            : base(original)
        { }

        #endregion

        #region Public methods

        public override string ToString()
        {
            return string.Format("Test checks:  {0}", this.Count > 0 ? formatChecks() : "None");
        }

        #endregion

        #region Private methods

        private string formatChecks()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("\r\n");

            for (int i = 0; i < this.Count; i++)
            {
                sb.AppendLine(String.Format("{0}. {1}\r\n", i, this[i].ToString()));
            }

            sb.AppendLine("Total checks:  " + this.Count.ToString());

            return sb.ToString();
        }

        #endregion

    }
}

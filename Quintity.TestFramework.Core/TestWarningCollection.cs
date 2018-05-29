using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [CollectionDataContract
      (Name = "TestWarnings",
      ItemName = "TestWarning")]
    public class TestWarningCollection : List<TestWarning>
    {
        #region Constructor

        internal TestWarningCollection(){ }

        internal TestWarningCollection(TestWarningCollection original)
            : base(original)
        { }

        #endregion

        #region Public methods

        public override string ToString()
        {
            return string.Format("Test warnings:  {0}", this.Count > 0 ? formatWarnings() : "None");
        }

        #endregion

        #region Private methods

        private string formatWarnings()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("\r\n");

            for (int i = 0; i < this.Count; i++)
            {
                sb.AppendLine(String.Format("{0}. {1}\r\n", i, this[i].ToString()));
            }

            sb.AppendLine("Total warnings:  " + this.Count.ToString());

            return sb.ToString();
        }

        #endregion
    }
}

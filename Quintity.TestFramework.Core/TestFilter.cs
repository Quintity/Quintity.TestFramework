using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestFilter
    {
        #region Data members

        public List<string> Tags
        { get; set; }

        public int SamplingRate
        { get; set; }

        #endregion

        #region Constructor

        public TestFilter(List<string> tags, int samplingRate)
        {
            Tags = tags;
            SamplingRate = samplingRate;
        }

        public TestFilter()
        {
            SamplingRate = 100;
        }

        #endregion

        #region Public methods

        public override string ToString()
        {
            string tags = "Filter tags:  ";

            foreach (string tag in Tags)
            {
                tags += string.Format("{0}, ", tag);
            }

            tags = tags.TrimEnd(new char[]{','}) + @"\r\n";

            return "Sampling rate:  " + SamplingRate.ToString();
        }

        #endregion
    }
}

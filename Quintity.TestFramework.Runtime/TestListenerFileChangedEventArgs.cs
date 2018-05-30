using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quintity.TestFramework.Runtime
{
    public class TestListenerFileChangedEventArgs
    {
        #region Properties

        private string _filePath;
        public string FilePath
        { get { return _filePath; } }

        #endregion

        #region Constructor

        public TestListenerFileChangedEventArgs(string filePath)
        {
            _filePath = filePath;
        }

        #endregion
    }
}

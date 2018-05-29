using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Quintity.TestFramework.Core
{
    public class FileNameEditorExt : FileNameEditor
    {
        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            base.InitializeDialog(openFileDialog);
            openFileDialog.DefaultExt = "dll";
            openFileDialog.InitialDirectory = TestProperties.TestConfigs;
            openFileDialog.Filter = "Test Listener Assembly|*.dll";
        }
    }
}

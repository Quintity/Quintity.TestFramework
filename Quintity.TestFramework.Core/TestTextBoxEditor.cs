using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Quintity.TestFramework.Core
{
    public class TestTextBoxEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// Displays test parameters dialog for edit from properties window.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService wfservice = provider.GetService(typeof(IWindowsFormsEditorService))
                             as IWindowsFormsEditorService;
            RichTextBox textBox = new RichTextBox();
            textBox.Text = value as string;
            textBox.BorderStyle = BorderStyle.None;
            textBox.Multiline = true;
            textBox.Size = new System.Drawing.Size(20, 80);
            //textBox.BackColor = System.Drawing.Color.AliceBlue;
            textBox.LinkClicked += textBox_LinkClicked;
            wfservice.DropDownControl(textBox);

            return textBox.Text;
        }

        void textBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
    }
}

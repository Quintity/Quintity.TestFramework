using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Runtime
{
    [DataContract]
    public class TestListenerDescriptor
    {
        #region Class data members

        [CategoryAttribute("Test Listener"),
        DescriptionAttribute("The test listener's user friendly name."),
        PropertyOrder(0)]
        [DataMember(Order = 0)]
        public string Name
        { get; set; }

        [CategoryAttribute("Test Listener"),
        DescriptionAttribute("Description of test listener's purpose."),
        PropertyOrder(3)]
        [DataMember(Order = 1), DefaultValue(null)]
        [Editor(typeof(TestTextBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Description
        { get; set; }

        [CategoryAttribute("Test Listener"),
        DescriptionAttribute("The test listeners current status (Inactive will disable the listener at execution)."),
        PropertyOrder(2), DefaultValue(Status.Inactive)]
        [DataMember(Order = 2)]
        public Status Status
        { get; set; }

        [CategoryAttribute("Test Listener"), DisplayName("On Failure"),
        DescriptionAttribute("Test execution behavoir in the event of a listener runtime failure" +
            " (Stop will terminate test execution, Continue will deactivate the listener and continue test execution."),
        PropertyOrder(7), DefaultValue(OnFailure.Stop)]
        [DataMember(Order = 4)]
        public OnFailure OnFailure
        { get; set; }

        [CategoryAttribute("Test Listener"),
        DescriptionAttribute("The full path to the test listener's assembly."),
        PropertyOrder(4)]
        [DataMember(Order = 6)]
        [EditorAttribute(typeof(FileNameEditorExt), typeof(System.Drawing.Design.UITypeEditor))]
        public string Assembly
        { get; set; }

        [CategoryAttribute("Test Listener"), 
        DescriptionAttribute("The test listener's type within the test assembly."),
        PropertyOrder(5), ReadOnly(true)]
        // TODO - this may make test listener dialog gag.
       // [EditorAttribute(typeof(TypeComboEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DataMember(Order = 7)]
        public string Type
        { get; set; }

        [DataMember(Order =8)]
        //TODO:  Need to figure out Wexman piece.
        //[Editor(typeof(Wexman.Design.GenericDictionaryEditor<string,string>), typeof(System.Drawing.Design.UITypeEditor))]
        public Dictionary<string, string> Parameters
        { get; set; }

        [IgnoreDataMember]
        [Browsable(false)]
        public int HashCode
        { get; set; }

        #endregion

        #region Class constructors

        public TestListenerDescriptor()
        {
            OnFailure = Core.OnFailure.Stop;
            Status = Core.Status.Inactive;
            Parameters = new Dictionary<string, string>();
        }

        #endregion

        #region Class public methods

        public override string ToString()
        {
            return string.Format("Name:  {0}\r\n    Status:  {1}\r\n    Description:  {2}\r\n" +
                "    Assemby:  {3}\r\n    Type:  {4}\r\n    On Failure:  {5}",
                this.Name,
                this.Status,
                this.Description,
                this.Assembly,
                this.Type,
                this.OnFailure);
        }

        #endregion

    }
}

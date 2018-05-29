using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    [TypeConverter(typeof(PropertySorter))]
    [DefaultProperty("UserID")]
    [Serializable]
    abstract public class TestScriptObject : TestArtifact
    {
        #region Class event definitions

        public delegate void TestPropertyChangedEventHandler(TestScriptObject testScriptObject, TestPropertyChangedEventArgs args);
        public static event TestPropertyChangedEventHandler OnTestPropertyChanged;

        internal void notifyTestPropertyChangedEvent(TestPropertyChangedEventArgs args)
        {
            if (OnTestPropertyChanged != null)
            {
                OnTestPropertyChanged(this, args);
            }
        }

        internal delegate void PrepareToSaveEventHandler(TestScriptObject testScriptObject);
        internal static event PrepareToSaveEventHandler OnPrepareToSave;

        internal static void firePrepareToSaveEvent(TestScriptObject testScriptObject)
        {
            if (OnPrepareToSave != null)
            {
                OnPrepareToSave(testScriptObject);
            }
        }

        #endregion

        #region Class data members

        [DataMember(Order = 10)]
        protected Guid _parentID;

        [DataMember(Order = 11)]
        protected string _userId;

        [CategoryAttribute("General"),
        DisplayName("User Identifier"),
        DescriptionAttribute("User-specific identifier."),
        PropertyOrder(10)]
        virtual public string UserID
        {
            get { return _userId; }
            set
            {
                if (_userId != value)
                {
                    object oldValue = _userId;
                    _userId = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [IgnoreDataMember]
        [Browsable(false)]
        virtual public Guid ParentID
        { get { return _parentID; } }

        [CategoryAttribute("Metadata"),
        DisplayName("Parent Identifier"),
        DescriptionAttribute("Parent identifier."),
        PropertyOrder(2)]
        virtual public string ParentIDAsString
        {
            get
            {
                return _parentID != Guid.Empty ? _parentID.ToString() : null;
            }
        }

        [DataMember(Order = 12, IsRequired = true)]
        protected string _title;

        [Browsable(false)]
        virtual public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    object oldValue = _title;
                    _title = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [DataMember(Order = 13)]
        protected string _description;

        [Browsable(false)]
        virtual public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    object oldValue = _description;
                    _description = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [DataMember(Order = 14)]
        protected Status _status;

        [CategoryAttribute("General"),
        DescriptionAttribute("Test artifact's current status."),
        PropertyOrder(11),
        DefaultValue(Status.Active)]
        virtual public Status Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    object oldValue = _status;
                    _status = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        #endregion

        #region Class constructors

        internal TestScriptObject()
            : base()
        { }

        internal TestScriptObject(string title)
            : this(title, null)
        { }

        internal TestScriptObject(string title, TestScriptObject parent)
            : this()
        {
            _title = title;

            if (parent != null)
            {
                _parentID = parent.SystemID;
            }
        }

        internal TestScriptObject(TestScriptObject orignalTestScriptObject, TestScriptObjectContainer parent)
            : base(orignalTestScriptObject)
        {
            this._parentID = parent != null ? parent.SystemID : Guid.Empty;
            this._userId = orignalTestScriptObject._userId;
            this._title = TestUtils.SafeCopyString(orignalTestScriptObject._title);
            this._description = TestUtils.SafeCopyString(orignalTestScriptObject._description);
            this._status = orignalTestScriptObject._status;
        }

        #endregion

        #region Class internal methods

        #endregion

        #region Class public methods

        public void SetParent(TestScriptObject parent)
        {
            _parentID = parent.SystemID;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion

        #region Class private methods

        #endregion
    }
}

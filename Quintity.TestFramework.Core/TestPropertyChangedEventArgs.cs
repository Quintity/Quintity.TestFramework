using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Quintity.TestFramework.Core
{
    public class TestPropertyChangedEventArgs : EventArgs
    {
        #region Data members

        public string Property
        { get; set; }

        public ChangeType ChangeType
        { get; set; }

        public object FormerValue
        { get; set; }

        public object CurrentValue
        { get; set; }

        #endregion

        #region Constructors

        public TestPropertyChangedEventArgs(object formerValue, object currentValue, [CallerMemberName] string property = "" )
            : this(ChangeType.Update, formerValue, currentValue, property)
        { }

        public TestPropertyChangedEventArgs(ChangeType changeType, object formerValue, object currentValue, [CallerMemberName] string property = "")
        {
            Property = property;
            ChangeType = changeType;
            FormerValue = formerValue;
            CurrentValue = currentValue;
        }

        #endregion

    }
}

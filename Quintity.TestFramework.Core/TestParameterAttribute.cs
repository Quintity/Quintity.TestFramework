/*
 * Copyright 2002 - 2007 Quintity, LLC.  All Rights Reserved.
 * Use is subject to license terms.
 * 
*/
using System;
using System.Runtime.InteropServices;

namespace Quintity.TestFramework.Core
{
    /// <summary>
    /// Test parameter attribute optional for test method parameters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class TestParameterAttribute : TestObjectAttribute
    {
        #region Data members
        private object m_defaultValue;
        #endregion fields.

        #region Constructors.

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TestParameterAttribute()
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestParameterAttribute(string alias)
            : this(alias, null, null)
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestParameterAttribute(string alias, string description)
            : base(alias, description, null)
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestParameterAttribute(string alias, string description, object defaultValue)
            : this(alias, description, defaultValue, null)
        {
        }

        /// <summary>
        /// Constructor for TestParameterAttribute
        /// </summary>
        /// <param name="description">Parameter description</param>
        /// <param name="defaultValue">Default parameter value.</param>
        public TestParameterAttribute(string alias, string description, object defaultValue, object tag)
            : base(alias, description, tag)
        {
            this.m_defaultValue = defaultValue;
        }

        #endregion constructors.

        #region TestParameterAttribute properties.

        /// <summary>
        /// Returns class's default value;
        /// </summary>
        public object DefaultValue
        {
            get { return m_defaultValue; }
        }

        #endregion properties.
    }
}

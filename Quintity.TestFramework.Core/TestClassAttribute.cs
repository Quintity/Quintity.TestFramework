/*
 * Copyright 2002 - 2007 Quintity, LLC.  All Rights Reserved.
 * Use is subject to license terms.
 * 
*/
using System;

namespace Quintity.TestFramework.Core
{
    /// <summary>
    /// Test class attribute required for all recognized test classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TestClassAttribute : TestObjectAttribute
    {
        /// <summary>
        /// Constructor for TestClassAttribute
        /// </summary>
        public TestClassAttribute()
        {
        }

        /// <summary>
        /// Constructor for TestClassAttribute
        /// </summary>
        /// <param name="alias">Alternate class name.</param>
        public TestClassAttribute(string alias)
            : base(alias)
        {
        }

        /// <summary>
        /// Constructor for TestClassAttribute
        /// </summary>
        /// <param name="alias">Alternate class name.</param>
        /// <param name="description">Class's description.</param>
        public TestClassAttribute(string alias, string description)
            : base(alias, description)
        {
        }

        /// <summary>
        /// Constructor for TestClassAttribute
        /// </summary>
        /// <param name="alias">Alternate class name.</param>
        /// <param name="description">Class's description.</param>
        /// <param name="tag">Class's tag object.</param>
        public TestClassAttribute(string alias, string description, object tag)
            : base(alias, description, tag)
        {
        }
    }
}
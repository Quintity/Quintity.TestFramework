/*
 * Copyright 2002 - 2007 Quintity, LLC.  All Rights Reserved.
 * Use is subject to license terms.
 * 
*/
using System;

namespace Quintity.TestFramework.Core
{
    /// <summary>
    /// Test method attribute required for all callable test methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class TestMethodAttribute : TestObjectAttribute
    {
        /// <summary>
        /// Constructor for TestMethodAttribute
        /// </summary>
        public TestMethodAttribute()
        {
        }

        /// <summary>
        /// Constructor for TestMethodAttribute
        /// </summary>
        /// <param name="alias">Method alias name.</param>
        public TestMethodAttribute(string alias)
            : base(alias)
        {
        }

        /// <summary>
        /// Constructor for TestMethodAttribute
        /// </summary>
        /// <param name="sAuthor">Name of method's author</param>
        /// <param name="sDescription">Method description</param>
        public TestMethodAttribute(string alias, string description)
            : base(alias, description)
        {
        }

        /// <summary>
        /// Constructor for TestMethodAttribute
        /// </summary>
        /// <param name="alias">Alternate method name.</param>
        /// <param name="description">Method's description.</param>
        /// <param name="tag">Method's tag object.</param>
        public TestMethodAttribute(string alias, string description, object tag)
            : base(alias, description, tag)
        {
        }
    }
}

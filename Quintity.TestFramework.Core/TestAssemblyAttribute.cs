/*
 * Copyright 2002 - 2007 Quintity, LLC.  All Rights Reserved.
 * Use is subject to license terms.
 * 
*/
using System;

namespace Quintity.TestFramework.Core
{
    /// <summary>
    /// Test assembly attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class TestAssemblyAttribute : TestObjectAttribute
    {
        /// <summary>
        /// Constructor for TestAssemblyAttribute
        /// </summary>
        public TestAssemblyAttribute()
        {
        }

        /// <summary>
        /// Constructor for TestAssemblyAttribute
        /// </summary>
        /// <param name="alias">Alternate assembly name.</param>
        /// <param name="description">Assembly's description.</param>
        public TestAssemblyAttribute(string alias, string description)
            : base(alias, description)
        {
        }

        /// <summary>
        /// Constructor for TestAssemblyAttribute
        /// </summary>
        /// <param name="alias">Alternate assembly name.</param>
        /// <param name="description">Assembly's description.</param>
        /// <param name="tag">Assembly's tag object.</param>
        public TestAssemblyAttribute(string alias, string description, object tag)
            : base(alias, description, tag)
        {
        }
    }
}
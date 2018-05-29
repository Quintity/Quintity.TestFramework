using System;
using System.Collections.Generic;
using System.Text;

namespace Quintity.TestFramework.Core
{
    /// <summary>
    /// Base attribute class of test objects (assembly, classes, methods, parameters, etc.)
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class TestObjectAttribute : Attribute
    {
        #region Data members
        private string m_alias;
        private string m_description;
        private object m_tag;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor for TestObjectAttribute
        /// </summary>
        public TestObjectAttribute()
        {
        }

        /// <summary>
        /// Constructor for TestObjectAttribute
        /// </summary>
        /// <param name="alias">Alternate object name.</param>
        public TestObjectAttribute(string alias)
            : this(alias, null, null)
        {
        }

        /// <summary>
        /// Constructor for TestObjectAttribute
        /// </summary>
        /// <param name="alias">Alternate object name.</param>
        /// <param name="description">Object's description.</param>
        public TestObjectAttribute(string alias, string description)
            : this(alias, description, null)
        {
        }

        /// <summary>
        /// Constructor for TestObjectAttribute
        /// </summary>
        /// <param name="alias">Alternate object name.</param>
        /// <param name="description">Object's description.</param>
        /// <param name="tag">Object's additional data tag object.</param>
        public TestObjectAttribute(string alias, string description, object tag)
        {
            m_alias = alias;
            m_description = description;
            m_tag = tag;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Return test object's alias.
        /// </summary>
        public string Alias
        {
            get
            {
                return m_alias;
            }
        }

        /// <summary>
        /// Returns object's description.
        /// </summary>
        public string Description
        {
            get
            {
                return m_description;
            }
        }
        /// <summary>
        /// Returns object's tag object.
        /// </summary>
        public object Tag
        {
            get { return m_tag; }
        }

        #endregion
    }
}

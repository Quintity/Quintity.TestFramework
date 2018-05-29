using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quintity.TestFramework.Core
{
    public partial class TestUtils
    {
            /// <summary>
            /// Verifies that the specified condition is false. The assertion fails if the condition is true.
            /// </summary>
            /// <param name="condition">The condition to verify is false.</param>
            internal static void IsFalse(Boolean condition, Exception e)
            {
                // If true, throw an exception.
                if (condition)
                {
                    throw e;
                }
            }

            /// <summary>
            /// Verifies that the specified condition is true. The assertion fails if the condition is false.
            /// </summary>
            /// <param name="condition">The condition to verify is true.</param>
            public static void IsTrue(Boolean condition, Exception e)
            {
                if (!condition)
                {
                    throw e;
                }
            }

            /// <summary>
            /// Verifies that the specified object is not null. The assertion fails if it is null
            /// </summary>
            /// <param name="value">The object to verify is not null.</param>
            public static void IsNotNull(Object value, Exception e)
            {
                // If value is null, throw exception.
                if (null == value)
                {
                    throw e;
                }
            }

            /// <summary>
            /// Verifies that the specified object is null. The assertion fails if it is not null.
            /// </summary>
            /// <param name="value">The object to verify is null.</param>
            public static void IsNull(Object value, Exception e)
            {
                // If value is null, throw exception.
                if (null != value)
                {
                    throw e;
                }
            }
        }
    }

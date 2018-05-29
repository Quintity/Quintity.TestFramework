using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Core
{
    public class TestReflection
    {
        #region Class public static methods

        static public Assembly LoadTestAssembly(string assemblyFile)
        {
            return Assembly.LoadFrom(assemblyFile);
        }

        static public Type GetTestClass(Assembly assembly, string testClass)
        {
            var types = assembly.GetTypes();
            Type type = assembly.GetType(testClass);

            if (type == null || !isTestClass(type))
            {
                type = null;
            }

            return type;
        }

        static public List<Type> GetTestClasses(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();

            List<Type> testTypes = new List<Type>();

            foreach (Type type in types)
            {
                if (isTestClass(type))
                {
                    testTypes.Add(type);
                }
            }

            return testTypes;
        }

        static public MethodInfo GetTestMethod(Type testClass, string testMethod, Type[] parameterTypes)
        {
            MethodInfo methodInfo = null;

            methodInfo = testClass.GetMethod(testMethod, parameterTypes);
 
            if (methodInfo == null || !isTestMethod(methodInfo))
            {
                methodInfo = null;
            }

            return methodInfo;
        }

        static public List<MethodInfo> GetTestMethods(Type testClass)
        {
            List<MethodInfo> testMethods = new List<MethodInfo>();

            IEnumerable<MethodInfo> methods = testClass.GetRuntimeMethods();

            foreach (MethodInfo method in methods)
            {
                if (isTestMethod(method))
                {
                    testMethods.Add(method);
                }
            }

            return testMethods;
        }

       

        #endregion

        #region Class private static methods

        static private bool isTestMethod(MethodInfo methodInfo)
        {
            bool isMethod = false;

            if (methodInfo.ReturnType == typeof(TestVerdict) && methodInfo.IsPublic && !methodInfo.IsStatic && isTestMethodAttributeSet(methodInfo))
            {
                isMethod = true;
            }

            return isMethod;
        }

        static private bool isTestClass(Type type)
        {
            bool isClass = false;

            if (type.IsSubclassOf(typeof(TestClassBase)) && isTestClassAttributeSet(type) &&
                type.IsVisible && type.IsPublic && !type.IsAbstract)
            {
                isClass = true;
            }

            return isClass;
        }

        static private bool isTestClassAttributeSet(Type type)
        {
            bool isSet = false;

            if (type.GetCustomAttribute(typeof(TestClassAttribute)) != null)
            {
                isSet = true;
            }

            return isSet;
        }

        static private bool isTestMethodAttributeSet(MethodInfo methodInfo)
        {
            bool isSet = false;

            if (methodInfo.GetCustomAttribute(typeof(TestMethodAttribute)) != null)
            {
                isSet = true;
            }

            return isSet;
        }

        #endregion
    }
}

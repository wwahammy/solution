//-----------------------------------------------------------------------
// <copyright company="CoApp Project">
//     Copyright (c) 2010  Garrett Serack. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------



namespace AutopackageTestProject
{
    using System;
    using System.Collections.Generic;
    using CoApp.Toolkit.Extensions;

    public class TestTarget {
        public string Name;
        public string Version;
        public List<TestTarget> Dependencies = new List<TestTarget>();

        public List<Tuple<string, string>> Policies = new List<Tuple<string, string>>();
        
        // public string PolicyLowerVersion;
        // public string PolicyHigherVersion;

        public bool Built = false;
        public bool Ignore = true;

        public override string ToString()
        {
            return "{0} {1}".format(Name, Version);
        }

        public static bool operator ==(TestTarget t1, TestTarget t2)
        {
            return t1.Name == t2.Name && t1.Version == t2.Version;
        }

        public static bool operator != (TestTarget t1, TestTarget t2)
        {
            return t1.Name != t2.Name || t1.Version != t2.Version;
        }
    }
}

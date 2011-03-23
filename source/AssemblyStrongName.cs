//-----------------------------------------------------------------------
// <copyright company="CoApp Project">
//     Copyright (c) 2011 Garrett Serack . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("CoApp Project")]
[assembly: AssemblyCopyright("Copyright (c) Garrett Serack, CoApp Contributors 2010-2011")]

// disable warning about using /keyfile instead of AssemblyKeyFile
#pragma warning disable 1699

#if SIGN_ASSEMBLY || TEST_SIGN_ASSEMBLY
[assembly: AssemblyVersion("1.0.2.370")] //SIGNED VERSION
[assembly: AssemblyFileVersion("1.0.2.370")] //SIGNED VERSION

#if SIGN_ASSEMBLY 
    [assembly: AssemblyKeyFileAttribute(@"..\coapp-signing\coapp-release-public-key.snk")]
    [assembly:AssemblyDelaySignAttribute(true)]
#endif 

#if TEST_SIGN_ASSEMBLY
    [assembly: AssemblyKeyFileAttribute(@"..\coapp-solution\CoApp-Development.SNK")]
    [assembly:AssemblyDelaySignAttribute(true)]
#endif 

#else

[assembly: AssemblyVersion("1.0.3.370")] //UNSIGNED VERSION
[assembly: AssemblyFileVersion("1.0.3.370")] //UNSIGNED VERSION

#endif

#pragma warning restore 1699



















































































































































































































































































































































































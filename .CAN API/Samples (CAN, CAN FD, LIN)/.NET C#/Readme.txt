Samples for VCI4 .NET Wrapper
=============================

The samples in  this directory show how to access VCI4 devices via the 
VCI4 .NET Wrapper component. 


Prerequisites:

  - VS2010 or later should work, VS2013 or later recommended
  - NuGet Package Manager should be installed. You can install it via the VisualStudio Extension manager.

  
Build:
  
  Open the solution and do a batch build. NuGet should restore your Package dependencies
  automatically via package restore (See https://docs.nuget.org/ndocs/consume-packages/package-restore for details).
  
  
Known issues:

  Sometimes NuGet does not copy the necessary native DLLs (vcinet.x64.dll, vcinet.x86.dll ) on rebuild to 
  the destination directory. This behaviour has been seen on VS2010 when using "rebuild" on the solution.
  Doing a "batch build" seems to circumvent the problem.

Differences between VCI3 .NET Wrapper and VCI4 .NET Wrapper:

  The former version of the VCI the VCI3 came with their own wrapper components living in the VCI3 namespace. 
  The VCI4 also installs them to maintain backward compatibility. 
  Existing applications using the VCI3 .NET wrapper should work out of the box with the VCI4 installed.

  The VCI4 .NET Wrapper should be used to develop new applications because
   - it avoids the GAC and therefore possible version conflicts between applications
   - it supports target agnostic applications (AnyCPU) because it switches between native implementations
     depending on detected runtime environment
   - is the supported way to access the VCI4 API via .NET

  In more detail the different components are:

  1. namespace Ixxat.Vci3: previous version installed with VCI3 and VCI4 for .NET 2 and .NET 3.5 (vcinet2.dll)

    - implemented in C++/CLI
    - depends on VC2008 runtime (MSVCR90.dll and MSVCM90.dll)
    - for compatibility reasons this version is also installed by the VCI4 setup
    - because it is obsolete no manual and samples are provided with the current VCI3 setup
    - targets .NET 2
    - the latest version works on top of VCI3 and VCI4 installations (uses VCIAPI.dll)
    - installs 32 and 64bit components both named vcinet2.dll in the .NET2 GAC

  2. namespace Ixxat.Vci3: previous version installed with VCI3 and VCI4 for .NET 4 (vcinet4.dll)

    - same source as vcinet2.dll but compiled for .NET 4
    - implemented in C++/CLI
    - depends on VC2010 runtime (MSVCR100.dll and MSVCM100.dll)
    - for compatibility reasons this version is also installed by the VCI4 setup
    - because it is obsolete no manual and samples are provided with the current VCI3 setup
    - targets .NET 4
    - the latest version works on top of VCI3 and VCI4 installations (uses VCIAPI.dll)
    - installs 32 and 64bit components both named vcinet4.dll in the .NET4 GAC

  3. namespace Ixxat.Vci4: current version of the .NET wrapper installable via NuGet (Ixxat.Vci4.dll, Ixxat.Vci4.Contract.dll, vcinet.x64.dll, vcinet.x86.dll)

    - components are NOT installed in the GAC, they should be deployed with the app
    - It consist of an interface definition component (Ixxat.Vci4.Contract.dll, MSIL, implemented in C#)
      a loader (Ixxat.Vci4.dll, MSIL, implemented in C#) and
      two target specific implementation components (vcinet.x64.dll, vcinet.x86.dll, implemented in C++/CLI).
      This design makes it possible to write .NET Applications and use MSIL output.
      The Loader detects wether the app is running on x64 or x86 and loads the correct implementation component.
    - the implementation components (vcinet.x64.dll, vcinet.x86.dll) depend on the VC2008 runtime (MSVCR90.dll and MSVCM90.dll)
    - components are available via NuGet [1], they should be deployed with the app along with the VC2008 runtime
    - included in the NuGet package are components for .NET 3.5 and .NET 4
    - manual/help available via NuGet or Ixxat website
    - a setup which installs the components and samples (VCI 4 .Net Wrapper_4_0_255_0.exe) is available via the IXXAT website [3]
    - there are some minor differences in usage patterns opposed to the previous version, the differences are listed in a chapter in [2]
    - the wrapper works on top of VCI3 and VCI4 installations (uses VCIAPI.dll)

    
Online resources:
  
  [1] https://www.nuget.org/packages?q=ixxat

  [2] .NET-API Software Version 4 SOFTWARE DESIGN GUIDE,
      see http://www.ixxat.com/docs/librariesprovider8/default-document-library/downloads/vci-v3/4-02-0250-20021--net-softwaredesignguide-en.pdf?sfvrsn=6

  [3] VCI 4 .Net Wrapper Version 4.0.255.0,
      see http://www.ixxat.com/docs/librariesprovider8/default-document-library/downloads/vci-v3/vci-4-net-wrapper-4-0-255-0.exe?sfvrsn=6


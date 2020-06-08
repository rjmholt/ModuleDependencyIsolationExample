# Example AssemblyLoadContext-using PowerShell Module

This repository demonstrates a simple example for isolating module dependencies from the rest of PowerShell.

The `old` directory contains the pre-isolation module,
and `new` represents the same module after implementing an isolation technique.

The main purpose of this example is to demonstrate how to use an Assembly Load Context
in .NET Core/.NET 5 to isolate module dependendies in PowerShell 6+.

Be warned that the .NET Framework isolation technique is not ideal;
there's no guarantee that the isolation technique for .NET Framework will work well.

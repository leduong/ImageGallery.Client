#!/bin/bash

set -e

# Install OpenCover and ReportGenerator, and save the path to their executables.
nuget install -Verbosity quiet -OutputDirectory packages -Version 4.6.519 OpenCover
nuget install -Verbosity quiet -OutputDirectory packages -Version 2.4.5.0 ReportGenerator

OPENCOVER=$PWD/packages/OpenCover.4.6.519/tools/OpenCover.Console.exe
REPORTGENERATOR=$PWD/packages/ReportGenerator.2.4.5.0/tools/ReportGenerator.exe

CONFIG=Release
# Arguments to use for the build
DOTNET_BUILD_ARGS="-c $CONFIG"
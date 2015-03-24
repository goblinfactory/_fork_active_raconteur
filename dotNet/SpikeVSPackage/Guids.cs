// Guids.cs
// MUST match guids.h
using System;

namespace GoblinfactoryLtd.SpikeVSPackage
{
    static class GuidList
    {
        public const string guidSpikeVSPackagePkgString = "d40d62a1-4ed2-4f34-a1dd-7c11f431cb63";
        public const string guidSpikeVSPackageCmdSetString = "99d1f2d0-4493-4e7c-962b-6b37e35484bf";

        public static readonly Guid guidSpikeVSPackageCmdSet = new Guid(guidSpikeVSPackageCmdSetString);
    };
}
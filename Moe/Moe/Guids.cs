// Guids.cs
// MUST match guids.h
using System;

namespace SenthilKumarSelvaraj.Moe
{
    static class GuidList
    {
        public const string guidMoePkgString = "86658643-3d17-4216-8f2c-b46d7a418138";
        public const string guidMoeCmdSetString = "039b49c1-2abd-415f-ae9c-817a16abdd38";
        public const string guidToolWindowPersistanceString = "7ea1e336-aaa9-45e8-9da3-f57ad164df7a";

        public static readonly Guid guidMoeCmdSet = new Guid(guidMoeCmdSetString);
    };
}
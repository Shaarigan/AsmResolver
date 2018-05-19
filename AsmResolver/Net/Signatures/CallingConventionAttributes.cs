﻿using System;

namespace AsmResolver.Net.Signatures
{
    [Flags]
    public enum CallingConventionAttributes : byte
    {
        Default = 0x0,
        C = 0x1,
        StdCall = 0x2,
        ThisCall = 0x3,
        FastCall = 0x4,
        VarArg = 0x5,
        Field = 0x6,
        Local = 0x7,
        Property = 0x8,
        GenericInstance = 0xA,

        Generic = 0x10,
        HasThis = 0x20,
        ExplicitThis = 0x40,
        Sentinel = 0x41, // TODO: support sentinel types.
    }
}
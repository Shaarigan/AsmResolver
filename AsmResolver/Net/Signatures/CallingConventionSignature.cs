﻿using System;
using AsmResolver.Net.Cts;

namespace AsmResolver.Net.Signatures
{
    public abstract class CallingConventionSignature : ExtendableBlobSignature
    {
        private const CallingConventionAttributes SignatureTypeMask = (CallingConventionAttributes)0xF;

        public static CallingConventionSignature FromReader(MetadataImage image, IBinaryStreamReader reader, bool readToEnd = false)
        {
            var signature = ReadSignature(image, reader);
            if (readToEnd)
                signature.ExtraData = reader.ReadToEnd();
            return signature;
        }

        private static CallingConventionSignature ReadSignature(MetadataImage image, IBinaryStreamReader reader)
        {
            var flag = reader.ReadByte();
            reader.Position--;

            switch ((CallingConventionAttributes) flag & SignatureTypeMask)
            {
                case CallingConventionAttributes.Default:
                case CallingConventionAttributes.C:
                case CallingConventionAttributes.ExplicitThis:
                case CallingConventionAttributes.FastCall:
                case CallingConventionAttributes.StdCall:
                case CallingConventionAttributes.ThisCall:
                case CallingConventionAttributes.VarArg:
                    return MethodSignature.FromReader(image, reader);
                case CallingConventionAttributes.Property:
                    return PropertySignature.FromReader(image, reader);
                case CallingConventionAttributes.Local:
                    return LocalVariableSignature.FromReader(image, reader);
                case CallingConventionAttributes.GenericInstance:
                    return GenericInstanceMethodSignature.FromReader(image, reader);
                case CallingConventionAttributes.Field:
                    return FieldSignature.FromReader(image, reader);
            }

            throw new NotSupportedException();
        }

        protected CallingConventionSignature()
        {
        }

        protected CallingConventionSignature(CallingConventionAttributes attributes)
        {
            Attributes = attributes;
        }

        public CallingConventionAttributes Attributes
        {
            get;
            set;
        }

        public bool IsMethod
        {
            get { return (int)(Attributes & SignatureTypeMask) <= 0x5; }
        }

        public bool IsField
        {
            get { return (Attributes & SignatureTypeMask) == CallingConventionAttributes.Field; }
        }

        public bool IsLocal
        {
            get { return (Attributes & SignatureTypeMask) == CallingConventionAttributes.Local; }
        }

        public bool IsGenericInstance
        {
            get { return (Attributes & SignatureTypeMask) == CallingConventionAttributes.GenericInstance; }
        }

        public bool IsGeneric
        {
            get { return Attributes.HasFlag(CallingConventionAttributes.Generic); }
            set { Attributes = Attributes.SetFlag(CallingConventionAttributes.Generic, value); }
        }

        public bool HasThis
        {
            get { return Attributes.HasFlag(CallingConventionAttributes.HasThis); }
            set { Attributes = Attributes.SetFlag(CallingConventionAttributes.HasThis, value); }
        }

        public bool ExplicitThis
        {
            get { return Attributes.HasFlag(CallingConventionAttributes.ExplicitThis); }
            set { Attributes = Attributes.SetFlag(CallingConventionAttributes.ExplicitThis, value); }
        }

        public bool IsSentinel
        {
            get { return Attributes.HasFlag(CallingConventionAttributes.Sentinel); }
            set { Attributes = Attributes.SetFlag(CallingConventionAttributes.Sentinel, value); }
        }
    }
}

using System;
using System.Collections.Generic;
using AsmResolver.Collections;
using AsmResolver.PE.DotNet.Metadata.Tables;

namespace AsmResolver.DotNet.Builder
{
    /// <summary>
    /// Provides a default implementation of the <see cref="ITokenMapping"/> interface.
    /// </summary>
    public class TokenMapping : ITokenMapping
    {
        private readonly OneToOneRelation<TypeDefinition, MetadataToken> _typeDefTokens = new();
        private readonly Dictionary<FieldDefinition, MetadataToken> _fieldTokens = new();
        private readonly Dictionary<MethodDefinition, MetadataToken> _methodTokens = new();
        private readonly Dictionary<ParameterDefinition, MetadataToken> _parameterTokens = new();
        private readonly Dictionary<PropertyDefinition, MetadataToken> _propertyTokens = new();
        private readonly Dictionary<EventDefinition, MetadataToken> _eventTokens = new();
        private readonly Dictionary<IMetadataMember, MetadataToken> _remainingTokens = new();

        /// <inheritdoc />
        public MetadataToken this[IMetadataMember member] => member.MetadataToken.Table switch
        {
            TableIndex.TypeDef => _typeDefTokens.GetValue((TypeDefinition) member),
            TableIndex.Field => _fieldTokens[(FieldDefinition) member],
            TableIndex.Method => _methodTokens[(MethodDefinition) member],
            TableIndex.Param => _parameterTokens[(ParameterDefinition) member],
            TableIndex.Event => _eventTokens[(EventDefinition) member],
            TableIndex.Property => _propertyTokens[(PropertyDefinition) member],
            _ => _remainingTokens[member]
        };

        /// <summary>
        /// Maps a single member to a new metadata token.
        /// </summary>
        /// <param name="member">The member to assign a token to.</param>
        /// <param name="newToken">The new token.</param>
        public void Register(IMetadataMember member, MetadataToken newToken)
        {
            if (member.MetadataToken.Table != newToken.Table)
                throw new ArgumentException($"Cannot assign a {newToken.Table} metadata token to a {member.MetadataToken.Table}.");

            switch (member.MetadataToken.Table)
            {
                case TableIndex.TypeDef:
                    _typeDefTokens.Add((TypeDefinition) member, newToken);
                    break;
                case TableIndex.Field:
                    _fieldTokens.Add((FieldDefinition) member, newToken);
                    break;
                case TableIndex.Method:
                    _methodTokens.Add((MethodDefinition) member, newToken);
                    break;
                case TableIndex.Param:
                    _parameterTokens.Add((ParameterDefinition) member, newToken);
                    break;
                case TableIndex.Event:
                    _eventTokens.Add((EventDefinition) member, newToken);
                    break;
                case TableIndex.Property:
                    _propertyTokens.Add((PropertyDefinition) member, newToken);
                    break;
                default:
                    _remainingTokens.Add(member, newToken);
                    break;
            }
        }

        /// <summary>
        /// Gets the type assigned to the provided metadata token.
        /// </summary>
        /// <param name="newToken">The new token.</param>
        /// <returns>The type, or <c>null</c> if no type is assigned to the provided token.</returns>
        public TypeDefinition GetType(MetadataToken newToken) => _typeDefTokens.GetKey(newToken);
    }
}

﻿#region

using Confuser.Core;
using dnlib.DotNet;

#endregion

namespace Confuser.Renamer.References
{
    public class TypeRefReference : INameReference<TypeDef>
    {
        private readonly TypeDef typeDef;
        private readonly TypeRef typeRef;

        public TypeRefReference(TypeRef typeRef, TypeDef typeDef)
        {
            this.typeRef = typeRef;
            this.typeDef = typeDef;
        }

        public bool UpdateNameReference(ConfuserContext context, INameService service)
        {
            typeRef.Namespace = typeDef.Namespace;
            typeRef.Name = typeDef.Name;
            return true;
        }

        public bool ShouldCancelRename()
        {
            return false;
        }
    }
}
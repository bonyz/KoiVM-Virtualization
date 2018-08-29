﻿#region

using Confuser.Core;

#endregion

namespace Confuser.Protections.AntiTamper
{
    internal interface IModeHandler
    {
        void HandleInject(AntiTamperProtection parent, ConfuserContext context, ProtectionParameters parameters);
        void HandleMD(AntiTamperProtection parent, ConfuserContext context, ProtectionParameters parameters);
    }
}
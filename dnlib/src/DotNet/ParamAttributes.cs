﻿#region

using System;

#endregion

namespace dnlib.DotNet
{
    /// <summary>
    ///     Parameter flags. See CorHdr.h/CorParamAttr
    /// </summary>
    [Flags]
    public enum ParamAttributes : ushort
    {
        /// <summary>Param is [In]</summary>
        In = 0x0001,

        /// <summary>Param is [out]</summary>
        Out = 0x0002,

        /// <summary>Param is optional</summary>
        Optional = 0x0010,

        /// <summary>Param has default value.</summary>
        HasDefault = 0x1000,

        /// <summary>Param has FieldMarshal.</summary>
        HasFieldMarshal = 0x2000
    }
}
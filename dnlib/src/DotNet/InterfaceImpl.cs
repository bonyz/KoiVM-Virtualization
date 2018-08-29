#region

using System;
using System.Diagnostics;
using System.Threading;
using dnlib.DotNet.MD;

#endregion

namespace dnlib.DotNet
{
    /// <summary>
    ///     A high-level representation of a row in the InterfaceImpl table
    /// </summary>
    [DebuggerDisplay("{Interface}")]
    public abstract class InterfaceImpl : IHasCustomAttribute, IContainsGenericParameter
    {
        /// <summary />
        protected CustomAttributeCollection customAttributes;

        /// <summary />
        protected ITypeDefOrRef @interface;

        /// <summary>
        ///     The row id in its table
        /// </summary>
        protected uint rid;

        /// <summary>
        ///     From column InterfaceImpl.Interface
        /// </summary>
        public ITypeDefOrRef Interface
        {
            get { return @interface; }
            set { @interface = value; }
        }

        bool IContainsGenericParameter.ContainsGenericParameter => TypeHelper.ContainsGenericParameter(this);

        /// <inheritdoc />
        public MDToken MDToken => new MDToken(Table.InterfaceImpl, rid);

        /// <inheritdoc />
        public uint Rid
        {
            get { return rid; }
            set { rid = value; }
        }

        /// <inheritdoc />
        public int HasCustomAttributeTag => 5;

        /// <summary>
        ///     Gets all custom attributes
        /// </summary>
        public CustomAttributeCollection CustomAttributes
        {
            get
            {
                if(customAttributes == null)
                    InitializeCustomAttributes();
                return customAttributes;
            }
        }

        /// <inheritdoc />
        public bool HasCustomAttributes => CustomAttributes.Count > 0;

        /// <summary>Initializes <see cref="customAttributes" /></summary>
        protected virtual void InitializeCustomAttributes()
        {
            Interlocked.CompareExchange(ref customAttributes, new CustomAttributeCollection(), null);
        }
    }

    /// <summary>
    ///     An InterfaceImpl row created by the user and not present in the original .NET file
    /// </summary>
    public class InterfaceImplUser : InterfaceImpl
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public InterfaceImplUser()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="interface">The interface the type implements</param>
        public InterfaceImplUser(ITypeDefOrRef @interface)
        {
            this.@interface = @interface;
        }
    }

    /// <summary>
    ///     Created from a row in the InterfaceImpl table
    /// </summary>
    internal sealed class InterfaceImplMD : InterfaceImpl, IMDTokenProviderMD
    {
        /// <summary>The module where this instance is located</summary>
        private readonly ModuleDefMD readerModule;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="readerModule">The module which contains this <c>InterfaceImpl</c> row</param>
        /// <param name="rid">Row ID</param>
        /// <param name="gpContext">Generic parameter context</param>
        /// <exception cref="ArgumentNullException">If <paramref name="readerModule" /> is <c>null</c></exception>
        /// <exception cref="ArgumentException">If <paramref name="rid" /> is invalid</exception>
        public InterfaceImplMD(ModuleDefMD readerModule, uint rid, GenericParamContext gpContext)
        {
#if DEBUG
            if(readerModule == null)
                throw new ArgumentNullException("readerModule");
            if(readerModule.TablesStream.InterfaceImplTable.IsInvalidRID(rid))
                throw new BadImageFormatException(string.Format("InterfaceImpl rid {0} does not exist", rid));
#endif
            OrigRid = rid;
            this.rid = rid;
            this.readerModule = readerModule;
            var @interface = readerModule.TablesStream.ReadInterfaceImplRow2(OrigRid);
            this.@interface = readerModule.ResolveTypeDefOrRef(@interface, gpContext);
        }

        /// <inheritdoc />
        public uint OrigRid
        {
            get;
        }

        /// <inheritdoc />
        protected override void InitializeCustomAttributes()
        {
            var list = readerModule.MetaData.GetCustomAttributeRidList(Table.InterfaceImpl, OrigRid);
            var tmp = new CustomAttributeCollection((int) list.Length, list, (list2, index) => readerModule.ReadCustomAttribute(((RidList) list2)[index]));
            Interlocked.CompareExchange(ref customAttributes, tmp, null);
        }
    }
}
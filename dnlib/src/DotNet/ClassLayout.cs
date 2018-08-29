#region

using System;
using dnlib.DotNet.MD;

#endregion

namespace dnlib.DotNet
{
    /// <summary>
    ///     A high-level representation of a row in the ClassLayout table
    /// </summary>
    public abstract class ClassLayout : IMDTokenProvider
    {
        /// <summary />
        protected uint classSize;

        /// <summary />
        protected ushort packingSize;

        /// <summary>
        ///     The row id in its table
        /// </summary>
        protected uint rid;

        /// <summary>
        ///     From column ClassLayout.PackingSize
        /// </summary>
        public ushort PackingSize
        {
            get { return packingSize; }
            set { packingSize = value; }
        }

        /// <summary>
        ///     From column ClassLayout.ClassSize
        /// </summary>
        public uint ClassSize
        {
            get { return classSize; }
            set { classSize = value; }
        }

        /// <inheritdoc />
        public MDToken MDToken => new MDToken(Table.ClassLayout, rid);

        /// <inheritdoc />
        public uint Rid
        {
            get { return rid; }
            set { rid = value; }
        }
    }

    /// <summary>
    ///     A ClassLayout row created by the user and not present in the original .NET file
    /// </summary>
    public class ClassLayoutUser : ClassLayout
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public ClassLayoutUser()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="packingSize">PackingSize</param>
        /// <param name="classSize">ClassSize</param>
        public ClassLayoutUser(ushort packingSize, uint classSize)
        {
            this.packingSize = packingSize;
            this.classSize = classSize;
        }
    }

    /// <summary>
    ///     Created from a row in the ClassLayout table
    /// </summary>
    internal sealed class ClassLayoutMD : ClassLayout, IMDTokenProviderMD
    {
        /// <summary>The module where this instance is located</summary>
        private readonly ModuleDefMD readerModule;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="readerModule">The module which contains this <c>ClassLayout</c> row</param>
        /// <param name="rid">Row ID</param>
        /// <exception cref="ArgumentNullException">If <paramref name="readerModule" /> is <c>null</c></exception>
        /// <exception cref="ArgumentException">If <paramref name="rid" /> is invalid</exception>
        public ClassLayoutMD(ModuleDefMD readerModule, uint rid)
        {
#if DEBUG
            if(readerModule == null)
                throw new ArgumentNullException("readerModule");
            if(readerModule.TablesStream.ClassLayoutTable.IsInvalidRID(rid))
                throw new BadImageFormatException(string.Format("ClassLayout rid {0} does not exist", rid));
#endif
            OrigRid = rid;
            this.rid = rid;
            this.readerModule = readerModule;
            classSize = readerModule.TablesStream.ReadClassLayoutRow(OrigRid, out packingSize);
        }

        /// <inheritdoc />
        public uint OrigRid
        {
            get;
        }
    }
}
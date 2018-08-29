﻿namespace dnlib.Utils
{
    internal delegate T MFunc<T>();

    internal delegate U MFunc<T, U>(T t);

    /// <summary>
    ///     Same as Func delegate
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="t"></param>
    /// <param name="u"></param>
    /// <returns></returns>
    public delegate V MFunc<T, U, V>(T t, U u);
}
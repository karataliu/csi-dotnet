using System;
using System.Collections.Generic;
using System.Linq;

namespace Csi.V0.Server
{
    [Flags]
    public enum CsiRpcServiceType
    {
        Identity = 1,
        Controller = 2,
        Node = 4,
    }

    static class EnumHelper
    {
        public static IEnumerable<T> AllValues<T>() => Enum.GetValues(typeof(T)).Cast<T>();
    }
}

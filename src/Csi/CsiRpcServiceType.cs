﻿using System;
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

    static class CsiRpcServiceTypeHelper
    {
        public static CsiRpcServiceType ParseDisables(string disables)
        {
            var type = default(CsiRpcServiceType);
            var collections = disables != null ? disables.Split(',') : new string[0];
            foreach (var flag in EnumHelper.AllValues<CsiRpcServiceType>())
            {
                if (!collections.Contains(flag.ToString(), StringComparer.OrdinalIgnoreCase)) type |= flag;
            }
            return type;
        }
    }

    static class EnumHelper
    {
        public static IEnumerable<T> AllValues<T>() => Enum.GetValues(typeof(T)).Cast<T>();
    }
}

// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Xml.Linq;
using Microsoft.PowerFx.Core.Binding;
using Microsoft.PowerFx.Core.Binding.BindInfo;
using Microsoft.PowerFx.Core.Entities;
using Microsoft.PowerFx.Core.Functions;
using Microsoft.PowerFx.Core.Types;
using Microsoft.PowerFx.Core.Utils;
using Microsoft.PowerFx.Syntax;

namespace Microsoft.PowerFx.Core.Logging.Trackers
{
    internal sealed class DelegationTelemetryInfo
    {
        private DelegationTelemetryInfo(string info, string dataSourceName = null)
        {
            Contracts.AssertValue(info);

            Info = info;
            DataSourceName = dataSourceName;
        }

        public string Info { get; }

        public string DataSourceName { get; }

        public static DelegationTelemetryInfo CreateEmptyDelegationTelemetryInfo()
        {
            return new DelegationTelemetryInfo(string.Empty);
        }

        public static DelegationTelemetryInfo CreateBinaryOpNoSupportedInfoTelemetryInfo(BinaryOp op)
        {
            return new DelegationTelemetryInfo(op.ToString());
        }

        public static DelegationTelemetryInfo CreateUnaryOpNoSupportedInfoTelemetryInfo(UnaryOp op)
        {
            return new DelegationTelemetryInfo(op.ToString());
        }

        public static DelegationTelemetryInfo CreateDataSourceNotDelegatableTelemetryInfo(IExternalDataSource ds)
        {
            Contracts.AssertValue(ds);

            return new DelegationTelemetryInfo(ds.Name);
        }

        public static DelegationTelemetryInfo CreateUndelegatableFunctionTelemetryInfo(TexlFunction func)
        {
            Contracts.AssertValueOrNull(func);

            if (func == null)
            {
                return CreateEmptyDelegationTelemetryInfo();
            }

            return new DelegationTelemetryInfo(func.Name);
        }

        public static DelegationTelemetryInfo CreateNoDelSupportByColumnTelemetryInfo(FirstNameInfo info)
        {
            Contracts.AssertValue(info);

            return new DelegationTelemetryInfo(info.Name);
        }

        public static DelegationTelemetryInfo CreateNoDelSupportByColumnTelemetryInfo(string columnName)
        {
            Contracts.AssertNonEmpty(columnName);

            return new DelegationTelemetryInfo(columnName);
        }

        public static DelegationTelemetryInfo CreateImpureNodeTelemetryInfo(TexlNode node, TexlBinding binding = null)
        {
            Contracts.AssertValue(node);
            Contracts.AssertValueOrNull(binding);

            switch (node.Kind)
            {
                case NodeKind.Call:
                    var callNode = node.AsCall();
                    var funcName = binding?.GetInfo(callNode)?.Function?.Name ?? string.Empty;
                    return new DelegationTelemetryInfo(funcName);
                default:
                    return new DelegationTelemetryInfo(node.ToString());
            }
        }

        public static DelegationTelemetryInfo CreateDelegationSuccessfulTelemetryInfo(IExternalDataSource externalDataSource)
        {
            return new DelegationTelemetryInfo(string.Empty, externalDataSource?.Name);
        }

        public static DelegationTelemetryInfo CreateUnSupportedSortArgTelemetryInfo(TexlNode arg, IExternalDataSource externalDataSource)
        {
            Contracts.AssertValue(arg);
            Contracts.AssertValueOrNull(externalDataSource);

            return new DelegationTelemetryInfo(arg.Kind.ToString(), externalDataSource?.Name);
        }

        public static DelegationTelemetryInfo CreateInvalidArgTypeTelemetryInfo(DType argType, IExternalDataSource externalDataSource)
        {
            Contracts.AssertValue(argType);
            Contracts.AssertValueOrNull(externalDataSource);

            return new DelegationTelemetryInfo(argType.Kind.ToString(), externalDataSource?.Name);


        }
    }
}

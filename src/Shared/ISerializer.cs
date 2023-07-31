//
// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
//

using System.Web.SessionState;

namespace Microsoft.Web.Redis
{
    public interface ISerializer
    {
        byte[] SerializeSessionStateItemCollection(SessionStateItemCollection sessionStateItemCollection);
        SessionStateItemCollection DeserializeSessionStateItemCollection(byte[] serializedSessionStateItemCollection);
    }
}

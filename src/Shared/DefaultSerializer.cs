//
// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
//

using System.IO;
using System.Web.SessionState;

namespace Microsoft.Web.Redis
{
    internal class DefaultSerializer : ISerializer
    {
        public byte[] SerializeSessionStateItemCollection(SessionStateItemCollection sessionStateItemCollection)
        {
            if (sessionStateItemCollection is null)
            {
                return null;
            }

            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);
            sessionStateItemCollection.Serialize(writer);
            writer.Close();
            return ms.ToArray();
        }

        public SessionStateItemCollection DeserializeSessionStateItemCollection(byte[] serializedSessionStateItemCollection)
        {
            if (serializedSessionStateItemCollection is null)
            {
                return null;
            }

            MemoryStream ms = new MemoryStream(serializedSessionStateItemCollection);
            BinaryReader reader = new BinaryReader(ms);
            return SessionStateItemCollection.Deserialize(reader);
        }
    }
}

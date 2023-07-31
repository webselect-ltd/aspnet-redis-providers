//
// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
//

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;

namespace Microsoft.Web.Redis.Tests
{
    /// <summary>
    /// This is not performant! It's just a naive implementation for testing.
    /// </summary>
    internal class TestSerializer : ISerializer
    {
        public byte[] SerializeSessionStateItemCollection(SessionStateItemCollection sessionStateItemCollection)
        {
            if (sessionStateItemCollection is null)
            {
                return null;
            }

            var d = new DictionaryWrapper {
                Items = sessionStateItemCollection.Keys.OfType<string>().ToDictionary(key => key, key => sessionStateItemCollection[key]),
                NullItem = sessionStateItemCollection[null]
            };

            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(d));
        }

        public SessionStateItemCollection DeserializeSessionStateItemCollection(byte[] serializedSessionStateItemCollection)
        {
            if (serializedSessionStateItemCollection is null)
            {
                return null;
            }

            var d = JsonConvert.DeserializeObject<DictionaryWrapper>(Encoding.UTF8.GetString(serializedSessionStateItemCollection, 0, serializedSessionStateItemCollection.Length));

            var sessionItems = new SessionStateItemCollection();

            if (d?.NullItem != null)
            {
                sessionItems[null] = d.NullItem;
            }

            if (d?.Items != null)
            {
                foreach (var item in d.Items)
                {
                    sessionItems[item.Key] = item.Value;
                }
            }

            return sessionItems;
        }

        // Found in this answer: https://stackoverflow.com/a/30673807
        internal class DictionaryWrapper
        {
            public DictionaryWrapper()
            {
                Items = new Dictionary<string, object>();
            }

            // Holds the value of the item with a null key, if any.
            [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto, DefaultValueHandling = DefaultValueHandling.Ignore)]
            public object NullItem { get; set; }

            [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto)]
            public Dictionary<string, object> Items { get; set; }
        }
    }
}

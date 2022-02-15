using AccountDomain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDomain
{
    public class MetatagMsgs
    {
        public class MetaTagAdded : IEvent
        {
            public readonly Guid TagId;
            public readonly string Tag;
            public MetaTagAdded(Guid tagId, string tag)
            {
                TagId = tagId;
                Tag = tag;
            }
        }
        public class MetaTagRenamed : IEvent
        {
            public readonly Guid TagId;
            public readonly string NewTag;
            public MetaTagRenamed(Guid tagId, string newTag)
            {
                TagId = tagId;
                NewTag = newTag;
            }
        }
        public class MetaTagRetired : IEvent
        {
            public readonly Guid TagId;
            public MetaTagRetired(Guid tagId)
            {
                TagId = tagId;
            }
        }
        public class MetaTagRestored : IEvent
        {
            public readonly Guid TagId;
            public MetaTagRestored(Guid tagId)
            {
                TagId = tagId;
            }
        }
    }
}

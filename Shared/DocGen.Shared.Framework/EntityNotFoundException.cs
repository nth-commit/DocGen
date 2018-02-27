using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Shared.Framework
{
    public class EntityNotFoundException : Exception
    {
        public string Entity { get; set; }

        public string EntityId { get; set; }

        public EntityNotFoundException(string entity, string entityId)
        {
            Entity = entity;
            EntityId = entityId;
        }
    }
}

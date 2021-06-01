using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace pvhellouser
{
    public class RedisUserProfile {
        public string username { get; set; }
        public string image { get; set; }
        public string desc { get; set; }
        public string email { get; set; }
        public bool bind_qq { get; set; }
    }
    public class RedisUserAccessControl {
        /// One of 'normal', 'admin'
        public string status { get; set; }
        /// One of 'blacklist', 'whitelist'
        public string access_mode { get; set; }
        public string[] allowed_ops { get; set; }
        public string[] denied_ops { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class RedisUser
    {   
        [BsonElement("_id")]
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("profile")]
        public RedisUserProfile profile { get; set;  }
        [BsonElement("access_control")]
        public RedisUserAccessControl access_control { get; set;  }

    }
}

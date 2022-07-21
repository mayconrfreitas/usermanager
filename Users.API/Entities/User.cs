using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Users.API.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [BsonElement("BirthDate")]
        [Required(AllowEmptyStrings = false)]
        public string BirthDate { get; set; }

        [BsonElement("Active")]
        public bool Active { get; set; } = true;
    }
}

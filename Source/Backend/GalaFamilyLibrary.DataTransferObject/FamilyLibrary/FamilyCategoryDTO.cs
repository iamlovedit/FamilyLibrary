﻿using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.DataTransferObject.FamilyLibrary
{
    public class FamilyCategoryDTO
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long ParentId { get; set; }

        public FamilyCategoryDTO? Parent { get; set; }
        
        public List<FamilyCategoryDTO>? Children { get; set; }
    }
}
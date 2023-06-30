﻿using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.FamilyService.DataTransferObjects
{
    public class ParameterDefinitionDTO
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string Name { get; set; }

        public ParameterGroupDTO Group { get; set; }

        public ParameterTypeDTO ParameterType { get; set; }

        public UnitTypeDTO UnitType { get; set; }
    }
}

﻿using SqlSugar;

namespace GalaFamilyLibrary.DataTransferObject.FamilyParameter
{
    public class ParameterCreationDTO
    {
        public string? Name { get; set; }

        public string? Value { get; set; }

        public StorageType StorageType { get; set; }

        public long DefinitionId { get; set; }

        public long FamilySymbolId { get; set; }

        public bool UserModifiable { get; set; }
    }
}


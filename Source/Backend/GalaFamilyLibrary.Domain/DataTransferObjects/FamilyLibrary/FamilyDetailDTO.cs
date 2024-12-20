﻿using GalaFamilyLibrary.Domain.DataTransferObjects.Identity;
using Newtonsoft.Json;
using SqlSugar;

namespace GalaFamilyLibrary.Domain.DataTransferObjects.FamilyLibrary
{
    public class FamilyDetailDTO
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string Name { get; set; }

        public ApplicationUserDTO Uploader { get; set; }

        public FamilyCategoryBasicDTO Category { get; set; }

        public List<FamilySymbolDTO> Symbols { get; set; }

        public List<ushort> Versions { get; set; }

        public string FileId { get; set; }

        public int Stars { get; set; }

        public uint Downloads { get; set; }

        public DateTime CreateTime { get; set; }

        public string? Description { get; set; }

    }
}

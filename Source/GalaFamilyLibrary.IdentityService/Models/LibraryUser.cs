﻿using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.IdentityService.Models
{
    [SugarTable("users")]
    public class LibraryUser:IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(IsNullable = false, Length = 5)]
        public string Name { get; set; }

        public string Email { get; set; }

        [SugarColumn(IsNullable = false, Length = 16)]
        public string Username { get; set; }

        [SugarColumn(IsNullable = false)]
        public string Password { get; set; }

        [SugarColumn(IsNullable = false)]
        public string Salt { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime LastLoginTime { get; set; }

        public bool IsDeleted { get; set; }

        public int ErrorCount { get; set; }

    }
}
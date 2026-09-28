using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cpa.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PharmacyGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Sort = table.Column<int>(type: "integer", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PharmacyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Sort = table.Column<int>(type: "integer", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prefixs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", maxLength: 20, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prefixs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProvinceGroups",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvinceGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Sort = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RunningConfigs",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsCode = table.Column<bool>(type: "boolean", maxLength: 1, nullable: false),
                    IsRef = table.Column<bool>(type: "boolean", maxLength: 1, nullable: false),
                    DigitCount = table.Column<int>(type: "integer", nullable: false),
                    TemplateCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunningConfigs", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Runnings",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    RefCode = table.Column<int>(type: "integer", nullable: false),
                    LastRunning = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runnings", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(510)", maxLength: 510, nullable: false),
                    NameEnglish = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    ProvinceGroupId = table.Column<int>(type: "integer", nullable: true),
                    ProvinceGroupId1 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Provinces_ProvinceGroups_ProvinceGroupId1",
                        column: x => x.ProvinceGroupId1,
                        principalTable: "ProvinceGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PositionName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PharmacyId = table.Column<int>(type: "integer", nullable: false),
                    LastLog = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NameEnglish = table.Column<string>(type: "text", nullable: false),
                    ProvinceId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubDistricts",
                columns: table => new
                {
                    SubDistrictId = table.Column<string>(type: "text", nullable: false),
                    ProvinceId = table.Column<string>(type: "text", nullable: false),
                    DistrictId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NameEnglish = table.Column<string>(type: "text", nullable: false),
                    ZipCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDistricts", x => x.SubDistrictId);
                    table.ForeignKey(
                        name: "FK_SubDistricts_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Prefix = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    NationId = table.Column<string>(type: "text", nullable: true),
                    BloodGroup = table.Column<string>(type: "text", nullable: true),
                    AddressNo = table.Column<string>(type: "text", nullable: true),
                    SubDistrictId = table.Column<string>(type: "text", nullable: true),
                    DistrictId = table.Column<string>(type: "text", nullable: true),
                    ProvinceId = table.Column<string>(type: "text", nullable: true),
                    ZipCode = table.Column<string>(type: "text", nullable: true),
                    TelephoneNumber = table.Column<string>(type: "text", nullable: true),
                    DrugAllergy = table.Column<string>(type: "text", nullable: true),
                    ChronicDisease = table.Column<string>(type: "text", nullable: true),
                    MainClaim = table.Column<string>(type: "text", nullable: true),
                    Education = table.Column<string>(type: "text", nullable: true),
                    Occupation = table.Column<string>(type: "text", nullable: true),
                    IsSmoke = table.Column<bool>(type: "boolean", nullable: true),
                    SmokeRemark = table.Column<string>(type: "text", nullable: true),
                    IsDrink = table.Column<bool>(type: "boolean", nullable: true),
                    DrinkRemark = table.Column<string>(type: "text", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patients_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patients_SubDistricts_SubDistrictId",
                        column: x => x.SubDistrictId,
                        principalTable: "SubDistricts",
                        principalColumn: "SubDistrictId");
                });

            migrationBuilder.CreateTable(
                name: "Pharmacys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    LicenseNo = table.Column<string>(type: "text", nullable: true),
                    NhsoCode = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Name2 = table.Column<string>(type: "text", nullable: true),
                    PharmacyGroupId = table.Column<string>(type: "text", nullable: true),
                    PharmacyGroupId1 = table.Column<int>(type: "integer", nullable: true),
                    PharmacyTypeId = table.Column<string>(type: "text", nullable: true),
                    PharmacyTypeId1 = table.Column<int>(type: "integer", nullable: true),
                    AddressNo = table.Column<string>(type: "text", nullable: true),
                    DistrictId = table.Column<string>(type: "text", nullable: true),
                    SubDistrictId = table.Column<string>(type: "text", nullable: true),
                    ProvinceId = table.Column<string>(type: "text", nullable: true),
                    ZipCode = table.Column<string>(type: "text", nullable: true),
                    Fda_Province = table.Column<string>(type: "text", nullable: true),
                    Office_Tel = table.Column<string>(type: "text", nullable: true),
                    Office_Fax = table.Column<string>(type: "text", nullable: true),
                    Office_Mail = table.Column<string>(type: "text", nullable: true),
                    LineID = table.Column<string>(type: "text", nullable: true),
                    Co_Name = table.Column<string>(type: "text", nullable: true),
                    Co_Mail = table.Column<string>(type: "text", nullable: true),
                    Co_Tel = table.Column<string>(type: "text", nullable: true),
                    PharmacyTypeOther = table.Column<string>(type: "text", nullable: true),
                    RegisYear = table.Column<string>(type: "text", nullable: true),
                    Lat = table.Column<string>(type: "text", nullable: true),
                    Lng = table.Column<string>(type: "text", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pharmacys_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pharmacys_PharmacyGroups_PharmacyGroupId1",
                        column: x => x.PharmacyGroupId1,
                        principalTable: "PharmacyGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pharmacys_PharmacyTypes_PharmacyTypeId1",
                        column: x => x.PharmacyTypeId1,
                        principalTable: "PharmacyTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pharmacys_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pharmacys_SubDistricts_SubDistrictId",
                        column: x => x.SubDistrictId,
                        principalTable: "SubDistricts",
                        principalColumn: "SubDistrictId");
                });

            migrationBuilder.CreateTable(
                name: "Pharmacists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    LicenseNo = table.Column<string>(type: "text", nullable: true),
                    WorkTime = table.Column<string>(type: "text", nullable: true),
                    WorkType = table.Column<string>(type: "text", nullable: true),
                    PositionName = table.Column<string>(type: "text", nullable: true),
                    PharmacyId = table.Column<int>(type: "integer", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pharmacists_Pharmacys_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacys",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ProvinceId",
                table: "Districts",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_DistrictId",
                table: "Patients",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ProvinceId",
                table: "Patients",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_SubDistrictId",
                table: "Patients",
                column: "SubDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacists_PharmacyId",
                table: "Pharmacists",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacys_DistrictId",
                table: "Pharmacys",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacys_PharmacyGroupId1",
                table: "Pharmacys",
                column: "PharmacyGroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacys_PharmacyTypeId1",
                table: "Pharmacys",
                column: "PharmacyTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacys_ProvinceId",
                table: "Pharmacys",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacys_SubDistrictId",
                table: "Pharmacys",
                column: "SubDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_ProvinceGroupId1",
                table: "Provinces",
                column: "ProvinceGroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_SubDistricts_DistrictId",
                table: "SubDistricts",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Pharmacists");

            migrationBuilder.DropTable(
                name: "Prefixs");

            migrationBuilder.DropTable(
                name: "RunningConfigs");

            migrationBuilder.DropTable(
                name: "Runnings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Pharmacys");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "PharmacyGroups");

            migrationBuilder.DropTable(
                name: "PharmacyTypes");

            migrationBuilder.DropTable(
                name: "SubDistricts");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "ProvinceGroups");
        }
    }
}

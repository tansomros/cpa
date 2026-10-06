using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BigLion.CPA.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BehaviorProblemItems",
                columns: table => new
                {
                    UID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descriptions = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    StatusFlag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Sort = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviorProblemItems", x => x.UID);
                });

            migrationBuilder.CreateTable(
                name: "Deseases",
                columns: table => new
                {
                    UID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ParentUID = table.Column<int>(type: "integer", nullable: true),
                    IsChapter = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    ICD = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Sort = table.Column<int>(type: "integer", nullable: true),
                    StatusFlag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    isICD = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deseases", x => x.UID);
                    table.ForeignKey(
                        name: "FK_Deseases_Deseases_ParentUID",
                        column: x => x.ParentUID,
                        principalTable: "Deseases",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugMasters",
                columns: table => new
                {
                    UID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TMTID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AliasName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Manufacturer = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    FSN = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Sort = table.Column<int>(type: "integer", nullable: true),
                    StatusFlag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    OUID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugMasters", x => x.UID);
                });

            migrationBuilder.CreateTable(
                name: "DrugProblemGroups",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Descriptions = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Sort = table.Column<int>(type: "integer", nullable: true),
                    StatusFlag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugProblemGroups", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "LabUOMs",
                columns: table => new
                {
                    UID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descriptions = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabUOMs", x => x.UID);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    NewsID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NewsDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NewsOrder = table.Column<int>(type: "integer", nullable: true),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    FirstPage = table.Column<int>(type: "integer", nullable: true),
                    NewsType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LinkPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    isPublic = table.Column<int>(type: "integer", nullable: true),
                    ContentNews = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.NewsID);
                });

            migrationBuilder.CreateTable(
                name: "PharmacyGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                    Id = table.Column<int>(type: "integer", nullable: false)
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
                    Id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
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
                    IsCode = table.Column<bool>(type: "boolean", nullable: false),
                    IsRef = table.Column<bool>(type: "boolean", nullable: false),
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
                name: "ServiceTypes",
                columns: table => new
                {
                    ServiceTypeID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ServiceName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Descriptions = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ProjectID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.ServiceTypeID);
                });

            migrationBuilder.CreateTable(
                name: "DrugProblemItems",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Descriptions = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DrugProblemGroupUID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Sort = table.Column<int>(type: "integer", nullable: true),
                    StatusFlag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugProblemItems", x => x.Code);
                    table.ForeignKey(
                        name: "FK_DrugProblemItems_DrugProblemGroups_DrugProblemGroupUID",
                        column: x => x.DrugProblemGroupUID,
                        principalTable: "DrugProblemGroups",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LabItems",
                columns: table => new
                {
                    UID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    AliasName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    UOMUID = table.Column<int>(type: "integer", nullable: true),
                    NormalRange = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Sort = table.Column<int>(type: "integer", nullable: true),
                    StatusFlag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabItems", x => x.UID);
                    table.ForeignKey(
                        name: "FK_LabItems_LabUOMs_UOMUID",
                        column: x => x.UOMUID,
                        principalTable: "LabUOMs",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(510)", maxLength: 510, nullable: false),
                    NameEnglish = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProvinceGroupId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Provinces_ProvinceGroups_ProvinceGroupId",
                        column: x => x.ProvinceGroupId,
                        principalTable: "ProvinceGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PaymentName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Amount = table.Column<double>(type: "double precision", nullable: true),
                    EffectiveTo = table.Column<long>(type: "bigint", nullable: true),
                    StatusFlag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ServiceTypeID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_PaymentMethods_ServiceTypes_ServiceTypeID",
                        column: x => x.ServiceTypeID,
                        principalTable: "ServiceTypes",
                        principalColumn: "ServiceTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NameEnglish = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ProvinceId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    HospitalUID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HospitalName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    HospitalGroupID = table.Column<int>(type: "integer", nullable: true),
                    HospitalTypeID = table.Column<int>(type: "integer", nullable: true),
                    DepartmentID = table.Column<int>(type: "integer", nullable: true),
                    DepartmentName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LevelID = table.Column<int>(type: "integer", nullable: true),
                    Bed = table.Column<int>(type: "integer", nullable: true),
                    Branch = table.Column<int>(type: "integer", nullable: true),
                    Office_hours = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Officer_Count = table.Column<int>(type: "integer", nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ProvinceID = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ProvinceName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ZipCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Office_Tel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Office_Fax = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Co_Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Co_Position = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Co_Mail = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Co_Tel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StatusFlag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Bill_Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    WorkDayID = table.Column<int>(type: "integer", nullable: true),
                    WorkDayDesc = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    WorkTimeID = table.Column<int>(type: "integer", nullable: true),
                    WorkTimeDesc = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ConfirmHold = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    isBranch = table.Column<int>(type: "integer", nullable: true),
                    BranchRemark = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    WorkList = table.Column<string>(type: "text", nullable: true),
                    WorkSpec = table.Column<string>(type: "text", nullable: true),
                    WorkTop = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Remark = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Informant = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    InfoPosition = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    InfoDate = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LetterTo = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ZoneID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    OfficeID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Facebook = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Lat = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Lng = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    MWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.HospitalUID);
                    table.ForeignKey(
                        name: "FK_Hospitals_Provinces_ProvinceID",
                        column: x => x.ProvinceID,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Registers",
                columns: table => new
                {
                    UID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LicenseNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LocationName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    LocationName2 = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    NHSOCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LocationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LocationGroupID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ProvinceID = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ProvinceName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ZipCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Office_Tel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Office_Mail = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Office_Hour = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LineID = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Co_Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Co_LicenseNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Co_Mail = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Co_Tel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RegisYear = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Lat = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Lng = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RegisterStatus = table.Column<int>(type: "integer", nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MUser = table.Column<int>(type: "integer", nullable: true),
                    MWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registers", x => x.UID);
                    table.ForeignKey(
                        name: "FK_Registers_Provinces_ProvinceID",
                        column: x => x.ProvinceID,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    itemID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RefID = table.Column<int>(type: "integer", nullable: true),
                    SeqNo = table.Column<int>(type: "integer", nullable: true),
                    LocationID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BYear = table.Column<int>(type: "integer", nullable: false),
                    PatientID = table.Column<long>(type: "bigint", nullable: false),
                    ServiceTypeID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EducateCount = table.Column<int>(type: "integer", nullable: true),
                    ServiceDate = table.Column<long>(type: "bigint", nullable: true),
                    ServiceTime = table.Column<int>(type: "integer", nullable: true),
                    PersonID = table.Column<int>(type: "integer", nullable: true),
                    CustName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    BirthDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Ages = table.Column<int>(type: "integer", nullable: true),
                    CardID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Telephone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Mobile = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AddressType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AddressNo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Road = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    District = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ProvinceID = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ProvinceName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MainClaim = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    CloseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ChildName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    ChildBirthDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ChildAges = table.Column<int>(type: "integer", nullable: true),
                    VaccineComplete = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    isEducate = table.Column<int>(type: "integer", nullable: true),
                    isReview = table.Column<int>(type: "integer", nullable: true),
                    EducateName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    isPapSmear = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    isFollow = table.Column<int>(type: "integer", nullable: true),
                    HospitalName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    DateCheck = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    MedicinceDesc = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    DocFile = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    isNormal = table.Column<int>(type: "integer", nullable: true),
                    Remark = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    isProblem1 = table.Column<int>(type: "integer", nullable: true),
                    isProblem2 = table.Column<int>(type: "integer", nullable: true),
                    isProblem3 = table.Column<int>(type: "integer", nullable: true),
                    isProblem4 = table.Column<int>(type: "integer", nullable: true),
                    isProblem5 = table.Column<int>(type: "integer", nullable: true),
                    isProblem6 = table.Column<int>(type: "integer", nullable: true),
                    isProblem7 = table.Column<int>(type: "integer", nullable: true),
                    isProblem8 = table.Column<int>(type: "integer", nullable: true),
                    isProblem9 = table.Column<int>(type: "integer", nullable: true),
                    SubProblem9 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    isProblem10 = table.Column<int>(type: "integer", nullable: true),
                    SubProblem10 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    isProblemOther = table.Column<int>(type: "integer", nullable: true),
                    ProblemRemark = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    isEducate1 = table.Column<int>(type: "integer", nullable: true),
                    isEducate2 = table.Column<int>(type: "integer", nullable: true),
                    isEducate3 = table.Column<int>(type: "integer", nullable: true),
                    isEducate4 = table.Column<int>(type: "integer", nullable: true),
                    isEducate5 = table.Column<int>(type: "integer", nullable: true),
                    isEducate6 = table.Column<int>(type: "integer", nullable: true),
                    isEducate7 = table.Column<int>(type: "integer", nullable: true),
                    isEducate8 = table.Column<int>(type: "integer", nullable: true),
                    isEducate9 = table.Column<int>(type: "integer", nullable: true),
                    isEducate10 = table.Column<int>(type: "integer", nullable: true),
                    isEducateOther = table.Column<int>(type: "integer", nullable: true),
                    EducateRemark = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ProbMed1 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ProbPro1 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ProbMed2 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ProbPro2 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ProbMed3 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ProbPro3 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EduMed1 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EduPro1 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EduMed2 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EduPro2 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EduMed3 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EduPro3 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    vct_Follow1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    vct_Follow2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    vct_FollowDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ServicePlan = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PayDate = table.Column<long>(type: "bigint", nullable: true),
                    CreateDate = table.Column<long>(type: "bigint", nullable: true),
                    InvoiceNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Follow_Channel = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Hospital_Type = table.Column<int>(type: "integer", nullable: true),
                    NextDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FollowDateSave = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsAbNormal = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    AbNormalRemark = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ChildGender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    isNHSO1 = table.Column<int>(type: "integer", nullable: true),
                    isNHSO2 = table.Column<int>(type: "integer", nullable: true),
                    isNHSO3 = table.Column<int>(type: "integer", nullable: true),
                    isNHSO4 = table.Column<int>(type: "integer", nullable: true),
                    isNHSO5 = table.Column<int>(type: "integer", nullable: true),
                    isNHSO6 = table.Column<int>(type: "integer", nullable: true),
                    isNHSO7 = table.Column<int>(type: "integer", nullable: true),
                    isNHSO8 = table.Column<int>(type: "integer", nullable: true),
                    isEducate11 = table.Column<int>(type: "integer", nullable: true),
                    isEducate12 = table.Column<int>(type: "integer", nullable: true),
                    CreateBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ActiveStatus = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TimeAfter = table.Column<double>(type: "double precision", nullable: true),
                    PatientFrom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isNotResponse = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CauseResponse = table.Column<int>(type: "integer", nullable: true),
                    OtherCause = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PayRecordBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.itemID);
                    table.ForeignKey(
                        name: "FK_Services_Provinces_ProvinceID",
                        column: x => x.ProvinceID,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_ServiceTypes_ServiceTypeID",
                        column: x => x.ServiceTypeID,
                        principalTable: "ServiceTypes",
                        principalColumn: "ServiceTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentConfigs",
                columns: table => new
                {
                    itemID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProvinceID = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    PaymentID = table.Column<int>(type: "integer", nullable: true),
                    EffectiveTo = table.Column<long>(type: "bigint", nullable: true),
                    StatusFlag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ProjectID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentConfigs", x => x.itemID);
                    table.ForeignKey(
                        name: "FK_PaymentConfigs_PaymentMethods_PaymentID",
                        column: x => x.PaymentID,
                        principalTable: "PaymentMethods",
                        principalColumn: "PaymentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentConfigs_Provinces_ProvinceID",
                        column: x => x.ProvinceID,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ForeName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Surname = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CardId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Telephone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TimeContact = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AddressType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AddressNo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Road = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SubDistrictId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    DistrictId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ProvinceId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ZipCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    MainClaim = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Education = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Occupation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    isAllergy = table.Column<string>(type: "text", nullable: true),
                    DrugAllergy = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    isSmoke = table.Column<string>(type: "text", nullable: true),
                    Smoke = table.Column<int>(type: "integer", nullable: true),
                    SmokeYear = table.Column<int>(type: "integer", nullable: true),
                    SmokeCigarette = table.Column<int>(type: "integer", nullable: true),
                    CigaretteType = table.Column<int>(type: "integer", nullable: true),
                    SmokingQuit = table.Column<string>(type: "text", nullable: true),
                    SmokingRemark = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Drinking = table.Column<int>(type: "integer", nullable: true),
                    DrinkFrequency = table.Column<int>(type: "integer", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_Districts_SubDistrictId",
                        column: x => x.SubDistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patients_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubDistricts",
                columns: table => new
                {
                    SubDistrictId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ProvinceId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    DistrictId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NameEnglish = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ZipCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDistricts", x => x.SubDistrictId);
                    table.ForeignKey(
                        name: "FK_SubDistricts_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BehaviorProblems",
                columns: table => new
                {
                    UID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceUID = table.Column<int>(type: "integer", nullable: true),
                    ProblemUID = table.Column<int>(type: "integer", nullable: true),
                    ProblemOther = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Interventions = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    FinalResult = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FinalResultOther = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FatFollow = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TasteFallow = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ResultBegin = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ResultEnd = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Remark = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    isFollow = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CUser = table.Column<int>(type: "integer", nullable: true),
                    CWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MUser = table.Column<int>(type: "integer", nullable: true),
                    MWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ServiceTypeID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PatientID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviorProblems", x => x.UID);
                    table.ForeignKey(
                        name: "FK_BehaviorProblems_BehaviorProblemItems_ProblemUID",
                        column: x => x.ProblemUID,
                        principalTable: "BehaviorProblemItems",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BehaviorProblems_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BehaviorProblems_ServiceTypes_ServiceTypeID",
                        column: x => x.ServiceTypeID,
                        principalTable: "ServiceTypes",
                        principalColumn: "ServiceTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LabResults",
                columns: table => new
                {
                    UID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RefUID = table.Column<int>(type: "integer", nullable: true),
                    ResultDate = table.Column<int>(type: "integer", nullable: true),
                    PatientID = table.Column<int>(type: "integer", nullable: true),
                    LabUID = table.Column<int>(type: "integer", nullable: true),
                    ResultValue = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsNormal = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    StatusFlag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CUser = table.Column<int>(type: "integer", nullable: true),
                    CWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MUser = table.Column<int>(type: "integer", nullable: true),
                    MWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabResults", x => x.UID);
                    table.ForeignKey(
                        name: "FK_LabResults_LabItems_LabUID",
                        column: x => x.LabUID,
                        principalTable: "LabItems",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LabResults_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MTMs",
                columns: table => new
                {
                    UID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    xBYear = table.Column<int>(type: "integer", nullable: true),
                    PatientID = table.Column<int>(type: "integer", nullable: false),
                    SEQ = table.Column<int>(type: "integer", nullable: true),
                    MTMTYPE = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PFROM = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FROMTXT = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ServiceDate = table.Column<int>(type: "integer", nullable: true),
                    ServiceTime = table.Column<int>(type: "integer", nullable: true),
                    PersonID = table.Column<int>(type: "integer", nullable: true),
                    Smoke = table.Column<int>(type: "integer", nullable: true),
                    SmokeYear = table.Column<int>(type: "integer", nullable: true),
                    SmokeCigarette = table.Column<int>(type: "integer", nullable: true),
                    CigaretteType = table.Column<int>(type: "integer", nullable: true),
                    Alcohol = table.Column<int>(type: "integer", nullable: true),
                    AlcoholFQ = table.Column<int>(type: "integer", nullable: true),
                    HospitalType = table.Column<int>(type: "integer", nullable: true),
                    HospitalName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    PayDate = table.Column<int>(type: "integer", nullable: true),
                    CloseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreateBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MedicationUsed1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MedicationUsed2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MedicationUsed3 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MedicationUsed4 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MedicationUsed5 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MedicationUsed6 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MedicationUsed7 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MedicationUsed8 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MedicationUsed9 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MedicationUsed10 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MedicationUsed11 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MedicationUsed12 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MedicationUsed13 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MedicationUsed14 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MedicationUsed15 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Frequency1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Frequency2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Frequency3 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Frequency4 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Frequency5 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Frequency6 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Frequency7 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Frequency8 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Frequency9 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Frequency10 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Frequency11 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Frequency12 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Frequency13 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Frequency14 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Frequency15 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isPitting = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    isWound = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    isPeripheral = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Pitting = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Wound = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Peripheral = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PayRecordBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReferStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    MTMService = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ServiceRemark = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ServiceRef = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TelepharmacyMethod = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RecordMethod = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RecordLocation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TelepharmacyRemark = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MTMs", x => x.UID);
                    table.ForeignKey(
                        name: "FK_MTMs_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pharmacy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LicenseNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NhsoCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Name2 = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    PharmacyGroupId = table.Column<int>(type: "integer", nullable: true),
                    PharmacyTypeId = table.Column<int>(type: "integer", nullable: true),
                    AddressNo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DistrictId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    SubDistrictId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ProvinceId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ZipCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Fda_Province = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Office_Tel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Office_Fax = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Office_Mail = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LineID = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Co_Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Co_Mail = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Co_Tel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PharmacyTypeOther = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RegisYear = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Lat = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Lng = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    DeleteFlag = table.Column<bool>(type: "boolean", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pharmacy_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pharmacy_PharmacyGroups_PharmacyGroupId",
                        column: x => x.PharmacyGroupId,
                        principalTable: "PharmacyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pharmacy_PharmacyTypes_PharmacyTypeId",
                        column: x => x.PharmacyTypeId,
                        principalTable: "PharmacyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pharmacy_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pharmacy_SubDistricts_SubDistrictId",
                        column: x => x.SubDistrictId,
                        principalTable: "SubDistricts",
                        principalColumn: "SubDistrictId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dispenses",
                columns: table => new
                {
                    UID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RefID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RefillDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PatientID = table.Column<int>(type: "integer", nullable: true),
                    LocationID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Remark = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    MTMUID = table.Column<int>(type: "integer", nullable: true),
                    DrugUID = table.Column<int>(type: "integer", nullable: true),
                    TMTID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    QTY = table.Column<double>(type: "double precision", nullable: true),
                    UOM = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UsedRemark = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispenses", x => x.UID);
                    table.ForeignKey(
                        name: "FK_Dispenses_DrugMasters_DrugUID",
                        column: x => x.DrugUID,
                        principalTable: "DrugMasters",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dispenses_MTMs_MTMUID",
                        column: x => x.MTMUID,
                        principalTable: "MTMs",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dispenses_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MTMBehaviors",
                columns: table => new
                {
                    UID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MTMUID = table.Column<int>(type: "integer", nullable: true),
                    ProblemUID = table.Column<int>(type: "integer", nullable: true),
                    ProblemOther = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Interventions = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CUser = table.Column<int>(type: "integer", nullable: true),
                    CWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MUser = table.Column<int>(type: "integer", nullable: true),
                    MWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinalResult = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FinalResultOther = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FatFollow = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TasteFollow = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ResultBegin = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ResultEnd = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Remark = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    isFollow = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    PatientID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MTMBehaviors", x => x.UID);
                    table.ForeignKey(
                        name: "FK_MTMBehaviors_BehaviorProblemItems_ProblemUID",
                        column: x => x.ProblemUID,
                        principalTable: "BehaviorProblemItems",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MTMBehaviors_MTMs_MTMUID",
                        column: x => x.MTMUID,
                        principalTable: "MTMs",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MTMBehaviors_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MTMDeseases",
                columns: table => new
                {
                    UID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MTMUID = table.Column<int>(type: "integer", nullable: true),
                    ServiceTypeID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DeseaseUID = table.Column<int>(type: "integer", nullable: true),
                    DeseaseOther = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CUser = table.Column<int>(type: "integer", nullable: true),
                    CWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PatientID = table.Column<int>(type: "integer", nullable: true),
                    ICDCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DeseaseName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MTMDeseases", x => x.UID);
                    table.ForeignKey(
                        name: "FK_MTMDeseases_Deseases_DeseaseUID",
                        column: x => x.DeseaseUID,
                        principalTable: "Deseases",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MTMDeseases_MTMs_MTMUID",
                        column: x => x.MTMUID,
                        principalTable: "MTMs",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MTMDeseases_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MTMDeseases_ServiceTypes_ServiceTypeID",
                        column: x => x.ServiceTypeID,
                        principalTable: "ServiceTypes",
                        principalColumn: "ServiceTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MTMDrugProblems",
                columns: table => new
                {
                    UID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MTMUID = table.Column<int>(type: "integer", nullable: true),
                    ServiceTypeID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ProblemGroupUID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ProblemUID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ProblemOther = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DrugUID = table.Column<int>(type: "integer", nullable: true),
                    Interventions = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    FinalResult = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FinalResultOther = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PatientID = table.Column<int>(type: "integer", nullable: true),
                    TMTID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DrugUID_Old = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MTMDrugProblems", x => x.UID);
                    table.ForeignKey(
                        name: "FK_MTMDrugProblems_DrugMasters_DrugUID",
                        column: x => x.DrugUID,
                        principalTable: "DrugMasters",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MTMDrugProblems_DrugProblemGroups_ProblemGroupUID",
                        column: x => x.ProblemGroupUID,
                        principalTable: "DrugProblemGroups",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MTMDrugProblems_DrugProblemItems_ProblemUID",
                        column: x => x.ProblemUID,
                        principalTable: "DrugProblemItems",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MTMDrugProblems_MTMs_MTMUID",
                        column: x => x.MTMUID,
                        principalTable: "MTMs",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MTMDrugProblems_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MTMDrugProblems_ServiceTypes_ServiceTypeID",
                        column: x => x.ServiceTypeID,
                        principalTable: "ServiceTypes",
                        principalColumn: "ServiceTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MTMDrugRemains",
                columns: table => new
                {
                    UID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MTMUID = table.Column<int>(type: "integer", nullable: true),
                    ServiceTypeID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DrugUID = table.Column<int>(type: "integer", nullable: true),
                    QTY = table.Column<double>(type: "double precision", nullable: true),
                    UOM = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MWhen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PatientID = table.Column<int>(type: "integer", nullable: true),
                    RemainFrom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ReasonUID = table.Column<int>(type: "integer", nullable: true),
                    ReasonRemark = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TMTID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MTMDrugRemains", x => x.UID);
                    table.ForeignKey(
                        name: "FK_MTMDrugRemains_DrugMasters_DrugUID",
                        column: x => x.DrugUID,
                        principalTable: "DrugMasters",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MTMDrugRemains_MTMs_MTMUID",
                        column: x => x.MTMUID,
                        principalTable: "MTMs",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MTMDrugRemains_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MTMDrugRemains_ServiceTypes_ServiceTypeID",
                        column: x => x.ServiceTypeID,
                        principalTable: "ServiceTypes",
                        principalColumn: "ServiceTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MTMRefers",
                columns: table => new
                {
                    UID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MTMUID = table.Column<int>(type: "integer", nullable: true),
                    HospitalType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    HospitalName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    PatientID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MTMRefers", x => x.UID);
                    table.ForeignKey(
                        name: "FK_MTMRefers_MTMs_MTMUID",
                        column: x => x.MTMUID,
                        principalTable: "MTMs",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MTMRefers_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pharmacists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LicenseNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    WorkTime = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    WorkType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PositionName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                        name: "FK_Pharmacists_Pharmacy_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PharmacyId = table.Column<int>(type: "integer", nullable: true),
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
                        name: "FK_Users_Pharmacy_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLogFiles",
                columns: table => new
                {
                    LogID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserID = table.Column<int>(type: "integer", nullable: true),
                    Work_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Act_Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DB_Effective = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Descrp = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Remark = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogFiles", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_UserLogFiles_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "integer", nullable: false),
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    isActive = table.Column<int>(type: "integer", nullable: true),
                    UpdBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.RoleID, x.UserID });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Banks_Code",
                table: "Banks",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BehaviorProblems_PatientID",
                table: "BehaviorProblems",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_BehaviorProblems_ProblemUID",
                table: "BehaviorProblems",
                column: "ProblemUID");

            migrationBuilder.CreateIndex(
                name: "IX_BehaviorProblems_ServiceTypeID",
                table: "BehaviorProblems",
                column: "ServiceTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Deseases_Code",
                table: "Deseases",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Deseases_ICD",
                table: "Deseases",
                column: "ICD");

            migrationBuilder.CreateIndex(
                name: "IX_Deseases_ParentUID",
                table: "Deseases",
                column: "ParentUID");

            migrationBuilder.CreateIndex(
                name: "IX_Dispenses_DrugUID",
                table: "Dispenses",
                column: "DrugUID");

            migrationBuilder.CreateIndex(
                name: "IX_Dispenses_MTMUID",
                table: "Dispenses",
                column: "MTMUID");

            migrationBuilder.CreateIndex(
                name: "IX_Dispenses_PatientID",
                table: "Dispenses",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Dispenses_RefID",
                table: "Dispenses",
                column: "RefID");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ProvinceId",
                table: "Districts",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugMasters_Name",
                table: "DrugMasters",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DrugMasters_TMTID",
                table: "DrugMasters",
                column: "TMTID");

            migrationBuilder.CreateIndex(
                name: "IX_DrugProblemItems_DrugProblemGroupUID",
                table: "DrugProblemItems",
                column: "DrugProblemGroupUID");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_HospitalName",
                table: "Hospitals",
                column: "HospitalName");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_ProvinceID",
                table: "Hospitals",
                column: "ProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_LabItems_Name",
                table: "LabItems",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_LabItems_UOMUID",
                table: "LabItems",
                column: "UOMUID");

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_LabUID",
                table: "LabResults",
                column: "LabUID");

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_PatientID",
                table: "LabResults",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMBehaviors_MTMUID",
                table: "MTMBehaviors",
                column: "MTMUID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMBehaviors_PatientID",
                table: "MTMBehaviors",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMBehaviors_ProblemUID",
                table: "MTMBehaviors",
                column: "ProblemUID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMDeseases_DeseaseUID",
                table: "MTMDeseases",
                column: "DeseaseUID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMDeseases_MTMUID",
                table: "MTMDeseases",
                column: "MTMUID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMDeseases_PatientID",
                table: "MTMDeseases",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMDeseases_ServiceTypeID",
                table: "MTMDeseases",
                column: "ServiceTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMDrugProblems_DrugUID",
                table: "MTMDrugProblems",
                column: "DrugUID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMDrugProblems_MTMUID",
                table: "MTMDrugProblems",
                column: "MTMUID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMDrugProblems_PatientID",
                table: "MTMDrugProblems",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMDrugProblems_ProblemGroupUID",
                table: "MTMDrugProblems",
                column: "ProblemGroupUID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMDrugProblems_ProblemUID",
                table: "MTMDrugProblems",
                column: "ProblemUID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMDrugProblems_ServiceTypeID",
                table: "MTMDrugProblems",
                column: "ServiceTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMDrugRemains_DrugUID",
                table: "MTMDrugRemains",
                column: "DrugUID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMDrugRemains_MTMUID",
                table: "MTMDrugRemains",
                column: "MTMUID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMDrugRemains_PatientID",
                table: "MTMDrugRemains",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMDrugRemains_ServiceTypeID",
                table: "MTMDrugRemains",
                column: "ServiceTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMRefers_MTMUID",
                table: "MTMRefers",
                column: "MTMUID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMRefers_PatientID",
                table: "MTMRefers",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMs_LocationID",
                table: "MTMs",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_MTMs_PatientID",
                table: "MTMs",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_News_NewsDate",
                table: "News",
                column: "NewsDate");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CardId",
                table: "Patients",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ProvinceId",
                table: "Patients",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_SubDistrictId",
                table: "Patients",
                column: "SubDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentConfigs_PaymentID",
                table: "PaymentConfigs",
                column: "PaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentConfigs_ProvinceID",
                table: "PaymentConfigs",
                column: "ProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_ServiceTypeID",
                table: "PaymentMethods",
                column: "ServiceTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacists_LicenseNo",
                table: "Pharmacists",
                column: "LicenseNo");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacists_PharmacyId",
                table: "Pharmacists",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacy_Code",
                table: "Pharmacy",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacy_DistrictId",
                table: "Pharmacy",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacy_PharmacyGroupId",
                table: "Pharmacy",
                column: "PharmacyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacy_PharmacyTypeId",
                table: "Pharmacy",
                column: "PharmacyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacy_ProvinceId",
                table: "Pharmacy",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacy_SubDistrictId",
                table: "Pharmacy",
                column: "SubDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyGroups_Code",
                table: "PharmacyGroups",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyTypes_Code",
                table: "PharmacyTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_ProvinceGroupId",
                table: "Provinces",
                column: "ProvinceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_LicenseNo",
                table: "Registers",
                column: "LicenseNo");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_LocationID",
                table: "Registers",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_ProvinceID",
                table: "Registers",
                column: "ProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_CardID",
                table: "Services",
                column: "CardID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_LocationID",
                table: "Services",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_PatientID",
                table: "Services",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ProvinceID",
                table: "Services",
                column: "ProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceTypeID",
                table: "Services",
                column: "ServiceTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SubDistricts_DistrictId",
                table: "SubDistricts",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDistricts_ZipCode",
                table: "SubDistricts",
                column: "ZipCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogFiles_UserID",
                table: "UserLogFiles",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogFiles_Work_Date",
                table: "UserLogFiles",
                column: "Work_Date");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserID",
                table: "UserRoles",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PharmacyId",
                table: "Users",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "BehaviorProblems");

            migrationBuilder.DropTable(
                name: "Dispenses");

            migrationBuilder.DropTable(
                name: "Hospitals");

            migrationBuilder.DropTable(
                name: "LabResults");

            migrationBuilder.DropTable(
                name: "MTMBehaviors");

            migrationBuilder.DropTable(
                name: "MTMDeseases");

            migrationBuilder.DropTable(
                name: "MTMDrugProblems");

            migrationBuilder.DropTable(
                name: "MTMDrugRemains");

            migrationBuilder.DropTable(
                name: "MTMRefers");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "PaymentConfigs");

            migrationBuilder.DropTable(
                name: "Pharmacists");

            migrationBuilder.DropTable(
                name: "Prefixs");

            migrationBuilder.DropTable(
                name: "Registers");

            migrationBuilder.DropTable(
                name: "RunningConfigs");

            migrationBuilder.DropTable(
                name: "Runnings");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "UserLogFiles");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "LabItems");

            migrationBuilder.DropTable(
                name: "BehaviorProblemItems");

            migrationBuilder.DropTable(
                name: "Deseases");

            migrationBuilder.DropTable(
                name: "DrugProblemItems");

            migrationBuilder.DropTable(
                name: "DrugMasters");

            migrationBuilder.DropTable(
                name: "MTMs");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "LabUOMs");

            migrationBuilder.DropTable(
                name: "DrugProblemGroups");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "Pharmacy");

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

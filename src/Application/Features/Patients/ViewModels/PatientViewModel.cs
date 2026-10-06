using System.Text.Json.Serialization;
using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Features.Districts.ViewModels;
using BigLion.CPA.Application.Features.Provinces.ViewModels;
using BigLion.CPA.Domain.Entities;

namespace BigLion.CPA.Application.Features.Patients.ViewModels;

public class PatientViewModel : IMapFrom<Patient>
{
    public int Id { get; set; }   
    public string? ForeName { get; set; }
    public string? Surname { get; set; }
    public string? FullName { get; set; }
    public string? Gender { get; set; }
    public DateOnly? BirthDate { get; set; }
    public int? Age
    {
        get
        {
            if (BirthDate is null)
                return null;

            var today = DateOnly.FromDateTime(DateTime.Today);

            var age = today.Year - BirthDate.Value.Year;

            if (BirthDate.Value > today.AddYears(-age))
                age--;

            return age;
        }
    }
    public string? CardId { get; set; }

    public string? Telephone { get; set; }
    public string? TimeContact { get; set; }

    public string? AddressType { get; set; }
    public string? AddressNo { get; set; }
    public string? Road { get; set; }
    public string? DistrictId { get; set; }
    public string? City { get; set; }
    public string? ProvinceId { get; set; }
    public string? ProvinceName { get; set; }
    public string? ZipCode { get; set; }

    public string? MainClaim { get; set; }
    public bool IsActive { get; set; }
    public string? Education { get; set; }
    public string? Occupation { get; set; }

    public bool? IsAllergy { get; set; }
    public string? DrugAllergy { get; set; }

    public bool? IsSmoke { get; set; }
    public int? Smoke { get; set; }
    public int? SmokeYear { get; set; }
    public int? SmokeCigarette { get; set; }
    public int? CigaretteType { get; set; }
    public bool? SmokingQuit { get; set; }
    public string? SmokingRemark { get; set; }

    public int? Alcohol { get; set; }
    public int? AlcoholFQ { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProvinceViewModel? Province { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DistrictViewModel? District { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Patient, PatientViewModel>()
            .ForMember(d => d.FullName, opt => opt.MapFrom(s =>
                ((s.ForeName ?? string.Empty) + " " + (s.Surname ?? string.Empty)).Trim()))
            .ForMember(d => d.DistrictId, opt => opt.MapFrom(s => s.SubDistrictId))
            .ForMember(d => d.City, opt => opt.MapFrom(s => s.DistrictId))
            .ForMember(d => d.ProvinceName, opt => opt.MapFrom(s => s.Province != null ? s.Province.Name : null))
            .ForMember(d => d.Alcohol, opt => opt.MapFrom(s => s.Drinking))
            .ForMember(d => d.AlcoholFQ, opt => opt.MapFrom(s => s.DrinkFrequency));
    }
}

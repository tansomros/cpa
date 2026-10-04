using System.Text.Json.Serialization;
using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Features.Districts.ViewModels;
using BigLion.CPA.Application.Features.Provinces.ViewModels;
using BigLion.CPA.Application.Features.SubDistricts.ViewModels;
using BigLion.CPA.Domain.Entities;

namespace BigLion.CPA.Application.Features.Patients.ViewModels
{
    public class PatientViewModel : IMapFrom<Patient>
    {
        public int Id { get; set; }
        public string? HospitalNumber { get; set; }
        public string? Prefix { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? NationId { get; set; }
        public string? Nationality { get; set; }
        public string? Religious { get; set; }
        public string? BloodGroup { get; set; }
        public string? EmployeeId { get; set; }
        //public int? CompanyId { get; set; }
        //public virtual Company? Company { get; set; }
        public string? Address { get; set; }
        public string? SubDistrictId { get; set; }
        public string? DistrictId { get; set; }
        public string? ProvinceId { get; set; }
        public string? ZipCode { get; set; }
        public string? TelephoneNumber { get; set; }
        public string? DrugAllergy { get; set; }
        public string? ChronicDisease { get; set; }
        public string? PrefixEnglish { get; set; }
        public string? FirstNameEnglish { get; set; }
        public string? LastNameEnglish { get; set; }
        public string? MiddleNameEnglish { get; set; }
        public string? NationalityEnglish { get; set; }
        public string? ReligiousEnglish { get; set; }
        public string? AddressEnglish { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ProvinceViewModel? Province { get; set; } = null;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DistrictViewModel? District { get; set; } = null;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SubDistrictViewModel? SubDistrict { get; set; } = null;
         
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Patient, PatientViewModel>()
                  .ForMember(d => d.FullName, opt => opt.MapFrom(s => $"{s.Prefix}{s.FirstName} {s.LastName}"));
                  //.ForSourceMember(s => s.Company, opt => opt.DoNotValidate());
        }
        //class DateResolver : IValueResolver<Patient, PatientViewModel, DateTimeOffset>
        //{
        //    public DateTimeOffset Resolve(Patient patient, PatientViewModel destination, DateTimeOffset destMember,ResolutionContext context)
        //    {
        //        DateTime dateTimeUtc = patient.BirthDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        //        return new DateTimeOffset(dateTimeUtc);                
        //    }
        //}
    }
}

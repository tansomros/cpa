namespace Cpa.Domain.Entities
{
    public class District
    {     
        public string DistrictId { get; set; }
        public string Name { get; set; }
        public string NameEnglish { get; set; }     
        public string ProvinceId { get; set; }
        public virtual Province? Province { get; set; }
        public ICollection<SubDistrict> SubDistricts { get; set; }
                
        public District(string provinceId, string districtId, string name, string nameEnglish)
        {
            ProvinceId = provinceId;
            DistrictId = districtId;
            Name = name;
            NameEnglish = nameEnglish;
            SubDistricts = [];
        }
    }
}

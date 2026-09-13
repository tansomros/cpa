namespace Cpa.Domain.Entities
{
  
    public class SubDistrict
    {
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public virtual District? District { get; set; }
        public string SubDistrictId { get; set; }
        public string Name { get; set; }
        public string NameEnglish { get; set; }
        public string ZipCode { get; set; }

        public SubDistrict(
            string provinceId,
            string districtId,
            string subDistrictId, 
            string name, 
            string nameEnglish, 
            string zipCode)
        {
            ProvinceId = provinceId;
            SubDistrictId = subDistrictId;
            Name = name;
            NameEnglish = nameEnglish;
            ZipCode = zipCode;
            DistrictId = districtId;
        }
    }
}

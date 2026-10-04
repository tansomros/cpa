namespace BigLion.CPA.Domain.Entities
{
    public class District
    {     
        public string Id { get; set; }
        public string Name { get; set; }
        public string NameEnglish { get; set; }     
        public string ProvinceId { get; set; }
        public virtual Province? Province { get; set; }
        public ICollection<SubDistrict> SubDistricts { get; set; }
                
        public District(string provinceId, string Id, string name, string nameEnglish)
        {
            ProvinceId = provinceId;
            this.Id = Id;
            Name = name;
            NameEnglish = nameEnglish;
            SubDistricts = [];
        }
    }
}

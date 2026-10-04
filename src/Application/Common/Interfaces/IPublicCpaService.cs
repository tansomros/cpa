namespace BigLion.CPA.Application.Common.Interfaces
{

    /// <summary>
    /// บริการสำหรับ
    /// </summary>
    public interface IPublicBigLionService
    {
        Task GetCheckupResult(string hospitalNumber);
    }
}

namespace TrainsOnline.Desktop.Interfaces
{
    using System.Threading.Tasks;
    using TrainsOnline.Desktop.Services.File;

    public interface IFileService
    {
        Task<FileSavingResults> SaveToPdfFile(byte[] content, string suggestedName);
    }
}

using RestWithASPNETUdemy.Data.VO;

namespace RestWithASPNETUdemy.Services.Interface
{
    public interface IFileService
    {
        public byte[] GetFile(string fileName);

        public Task<FileDetailVO> SaveFileToDisk(IFormFile file);

        public Task<List<FileDetailVO>> SaveFilesToDisk(IList<IFormFile> files);
    }
}

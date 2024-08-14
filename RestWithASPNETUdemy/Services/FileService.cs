using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Services.Interface;

namespace RestWithASPNETUdemy.Services
{
    public sealed class FileService : IFileService
    {
        private readonly string _baseFilePath;
        private readonly IHttpContextAccessor _context;

        public FileService(IHttpContextAccessor context)
        {
            _context = context;

            _baseFilePath = Directory.GetCurrentDirectory() + "\\UploadDir\\";
        }

        public byte[] GetFile(string fileName)
        {
            var filePath = _baseFilePath + fileName;
            return File.ReadAllBytes(filePath);
        }

        public async Task<List<FileDetailVO>> SaveFilesToDisk(IList<IFormFile> files)
        {
            List<FileDetailVO> list = [];
            foreach (IFormFile file in files)
            {
                list.Add(await SaveFileToDisk(file));
            }

            return list;
        }

        public async Task<FileDetailVO> SaveFileToDisk(IFormFile file)
        {
            FileDetailVO fileDetail = new()
            {
                DocumentType = Path.GetExtension(file.FileName),
            };

            var baseUrl = _context.HttpContext.Request.Host;

            if (fileDetail.DocumentType.ToLower() == ".pdf"
                || fileDetail.DocumentType.ToLower() == ".jpg"
                || fileDetail.DocumentType.ToLower() == ".png"
                || fileDetail.DocumentType.ToLower() == ".jpeg")
            {
                string docName = Path.GetFileName(file.FileName);

                if (file != null && file.Length > 0)
                {
                    var destination = Path.Combine(_baseFilePath, "", docName);

                    fileDetail.DocumentName = docName;
                    fileDetail.DocumentURL = Path.Combine(baseUrl + "/api/file/v1" + fileDetail.DocumentName);

                    using (var stream = new FileStream(destination, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }

            return fileDetail;
        }
    }
}

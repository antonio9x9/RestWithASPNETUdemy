using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Services.Interface;

namespace RestWithASPNETUdemy.Controllers.V1
{
    [ApiVersion("1")]
    [ApiController]
    //[Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public sealed class FileController : Controller
    {
        private readonly IFileService _fIleService;

        public FileController(IFileService fIleService)
        {
            _fIleService = fIleService;
        }


        [HttpPost("uploadFile")]
        [ProducesResponseType(200, Type = typeof(FileDetailVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/Json")]
        public async Task<IActionResult> UploadOneFile(IFormFile file)
        {

            FileDetailVO detail = await _fIleService.SaveFileToDisk(file);

            return new OkObjectResult(detail);
        }

        [HttpPost("uploadMultipleFiles")]
        [ProducesResponseType(200, Type = typeof(FileDetailVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/Json")]
        public async Task<IActionResult> UploadMultipleFiles(List<IFormFile> files)
        {

            List<FileDetailVO> detail = await _fIleService.SaveFilesToDisk(files);
            return new OkObjectResult(detail);
        }

        [HttpGet("downloadFile/{filename}")]
        [ProducesResponseType(200, Type = typeof(byte[]))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> GetFileAsync(string fileName)
        {
            byte[] buffer = _fIleService.GetFile(fileName);
            if (buffer != null)
            {
                HttpContext.Response.ContentType = $"application/{Path.GetExtension(fileName).Replace(".", "")}";
                HttpContext.Response.Headers.Add("content-lentgh", buffer.Length.ToString());
                
                await HttpContext.Response.Body.WriteAsync(buffer, 0, buffer.Length);
            }

            return new ContentResult();
        }
    }
}

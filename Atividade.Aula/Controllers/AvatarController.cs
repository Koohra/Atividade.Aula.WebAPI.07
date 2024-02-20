using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Atividade.Aula.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvatarController : ControllerBase
    {
        private readonly IContentTypeProvider _contentTypeProvider;

        public AvatarController(IContentTypeProvider contentTypeProvider)
        {
            _contentTypeProvider = contentTypeProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var path = $"wwwroot/{file.FileName}";

            if (ValidadeFile(file) == false) return BadRequest();

            using (var fileStream = System.IO.File.Create(path))
            {
                await file.CopyToAsync(fileStream);
            }

            return Ok(file.FileName);

        }

        [HttpGet]
        public IActionResult Get(string nomeArquivo)
        {
            var path = $"wwwroot/{nomeArquivo}";

            if (!System.IO.File.Exists(path)) 
            { 
                return BadRequest(); 
            }

            var fileBytes = System.IO.File.ReadAllBytes(path);

            var contentType = "application/octet-stram";

            if(_contentTypeProvider.TryGetContentType(path, out var contentTypeProvided))
            {
                contentType = contentTypeProvided;
            }

            return File(fileBytes, contentType);

        }

        private bool ValidadeFile(IFormFile file)
        {
            string[] extensions = [".doc", ".pdf", ".docx", ".txt"];
            var extensionFile = Path.GetExtension(file.FileName);

            if (file == null) { return false; }

            if (file.Length > 5 * 1024 * 1024) { return false; }

            if (!extensions.Contains(extensionFile)) { return false; }

            return true;

        }
    }
}

using AdminSenyun.Data.Service;
using Microsoft.AspNetCore.Mvc;

namespace AdminSenyun.Server.Controllers;

[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
[ApiController]
public class FileController(SysFileService sysFileService) : Controller
{
    [HttpGet("{id}")]
    public IActionResult GetFile(long id)
    {
        //获取SysFile
        var file = sysFileService.GetSysFile(id);

        if (file == null)
        {
            return Json(new { code = "20000", message = "没有找到文件" });
        }

        //获取文件
        var bytes = sysFileService.GetFileBytes(file);

        if (file.IsShow())
        {
            return File(bytes, file.ContentType);
        }
        else
        {
            return File(bytes, file.ContentType, file.Name + file.Ext);
        }
    }
}

namespace AdminSenyun.Models;

[SugarTable(null, "文件表")]
public class SysFile : EntityBaseTree
{
    [Description("名称")]
    public string Name { get; set; } = "";

    [Description("文件后缀")]
    public string Ext { get; set; } = "";

    [Description("注释")]
    public string Description { get; set; } = "";

    [Description("文件大小(kb)")]
    public long SizeKb { get; set; }

    [Description("文件类型")]
    public string ContentType { get; set; } = "application/octet-stream";

    [Description("目录")]
    public string Folder { get; set; } = DateTime.Now.ToString("yyyy/MM/dd");

    [Description("文件地址")]
    [SugarColumn(IsIgnore = true)]
    public string Url => GetUrl();

    /// <summary>
    /// 获取文件名称
    /// </summary>
    /// <returns></returns>
    public string GetFileName() => Name + Ext;
    /// <summary>
    /// 获取存储名称
    /// </summary>
    /// <returns></returns>
    public string GetSaveFileName() => Id + Ext;

    /// <summary>
    /// 获取文件Url地址
    /// </summary>
    /// <returns></returns>
    public string GetUrl() => "api/file/" + Id;

    /// <summary>
    /// 是否支持预览
    /// </summary>
    /// <returns></returns>
    public bool IsShow()
    {
        List<string> mimeTypes =
        [
            "text/plain", // 文本文件 (.txt)
            "image/jpeg", // JPEG 图像文件 (.jpg, .jpeg)
            "image/png", // PNG 图像文件 (.png)
            "image/gif", // gif 图像文件 (.gif)
            "application/pdf", // PDF 文件 (.pdf)
            "audio/mpeg", // MP3 音频文件 (.mp3)
            "audio/ogg", // Ogg 音频文件 (.ogg)
            "audio/wav", // WAV 音频文件 (.wav)
            "video/mp4", // MP4 视频文件 (.mp4)
            "video/webm", // WebM 视频文件 (.webm)
            "video/ogg", // Ogg 视频文件 (.ogg)
            //"video/quicktime", // QuickTime 视频文件 (.mov)
            //"video/x-msvideo", // AVI 视频文件 (.avi)
            "text/html", // HTML 文件 (.html, .htm)
            "text/css", // CSS 样式表文件 (.css)
            "application/javascript", // JavaScript 文件 (.js)
            "application/json", // JSON 数据文件 (.json)
            "application/xml", // XML 数据文件 (.xml)
            //"text/markdown", // Markdown 文件 (.md)
            //"application/msword", // Microsoft Word 文档 (.doc, .docx)
            //"application/vnd.ms-excel", // Microsoft Excel 表格 (.xls, .xlsx)
            //"application/vnd.ms-powerpoint", // Microsoft PowerPoint 演示文稿 (.ppt, .pptx)
            //"image/vnd.adobe.photoshop", // Adobe Photoshop 图像文件 (.psd)
            //"application/postscript", // Adobe Illustrator 矢量图形文件 (.ai)
            "image/svg+xml", // SVG 图形文件 (.svg)
            //"font/ttf", // TrueType 字体文件 (.ttf)
            //"font/otf", // OpenType 字体文件 (.otf)
            //"font/woff", // Web Open Font Format 字体文件 (.woff)
            //"font/woff2", // Web Open Font Format 2.0 字体文件 (.woff2)
            //"application/wasm", // WebAssembly 二进制文件 (.wasm)
            //"application/zip" // ZIP 压缩文件 (.zip)
        ];

        return mimeTypes.Contains(ContentType, StringComparer.OrdinalIgnoreCase);
    }
}

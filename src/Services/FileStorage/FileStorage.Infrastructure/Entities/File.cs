using FileStorage.Infrastructure.Abstractions;

namespace FileStorage.Infrastructure.Entities;

public class File : DeletableEntity<Guid>
{
    public string FilePath { get; set; }
    public string ContentType { get; set; }
    public string ThumbnailPath { get; set; }
}
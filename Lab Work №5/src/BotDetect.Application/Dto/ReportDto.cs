namespace BotDetect.Application.Dto;

public record ReportDto(
    int ReportId,
    int UserId,
    DateTime ReportDate,
    string FileFormat,
    IEnumerable<ExportedResultDto> Exports);

public record ExportedResultDto(
    int ExportId,
    string FileLocation,
    string Format,
    DateTime ExportDate);

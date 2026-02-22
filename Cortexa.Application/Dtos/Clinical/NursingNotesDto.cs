using System;

namespace Cortexa.Application.Dtos.Clinical
{
    public record NursingNotesDto(
        string Id,
        string NoteText,
        DateTime NoteDateTime,
        string AdmissionId,
        string NurseId
    );
}

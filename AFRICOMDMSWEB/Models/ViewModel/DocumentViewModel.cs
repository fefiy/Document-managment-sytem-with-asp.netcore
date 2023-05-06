
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AFRICOMDMSWEB.Models.ViewModel
{
    public class DocumentViewModel
    {
        public string path { get; set; }
        [ValidateNever]
        public Document Documents { get; set; }
        [ValidateNever]
        public Folder Folders { get; set; }
        [ValidateNever]
        public IEnumerable<Folder> folder { get; set; }
        [ValidateNever]

        public IEnumerable<Document> documents { get; set; }
        [ValidateNever]
        public IEnumerable<Folder> folderfromDms { get; set; }

        [ValidateNever]
        public FileShared sharedfiles { get; set; }
        [ValidateNever]
        public IEnumerable<Folder> pathFolders { get; set; }
        [ValidateNever]
        public IEnumerable<FileShared> FileRecived { get; set; }
        [ValidateNever]
        public IEnumerable<shareFolder> FolderRecived { get; set; }
        [ValidateNever]
        public Folder_User folder_User { get; set; }
        [ValidateNever]
        public string pathhh { get; set; }
        [ValidateNever]
        public IEnumerable<Folder_User> folder_Users { get; set; }
    }
}

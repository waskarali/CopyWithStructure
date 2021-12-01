using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CopyWithStructure
{
    [Command(PackageIds.CopyWithStructureCommand)]
    internal sealed class CopyWithStructureCommand : BaseCommand<CopyWithStructureCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            var destinationFolder = selectDestinationFolder();

            if (!string.IsNullOrWhiteSpace(destinationFolder))
            {
                var selectedItens = await VS.Solutions.GetActiveItemsAsync();

                var soluctionFolder = new FileInfo((await VS.Solutions.GetCurrentSolutionAsync()).FullPath).Directory.FullName;

                foreach (var item in selectedItens.OfType<PhysicalFile>())
                {
                    var destinationFile = new FileInfo(item.FullPath.Replace(soluctionFolder, destinationFolder));

                    createFolder(destinationFile.Directory);

                    File.Copy(item.FullPath, destinationFile.FullName, true);
                }

            }
        }

        private void createFolder(DirectoryInfo directory)
        {
            if (directory != null && !Directory.Exists(directory.FullName))
                directory.Create();
        }

        private string selectDestinationFolder()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select destination folder";

                DialogResult result = fbd.ShowDialog();                

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    return fbd.SelectedPath;
            }

            return null;
        }
    }
}

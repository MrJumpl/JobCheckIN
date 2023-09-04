using Mlok.Core.Data;
using System.IO;
using System.Threading.Tasks;
using Umbraco.Core.IO;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class StudentPhotoController
    {
        private readonly DbScopeProvider scopeProvider;

        public StudentPhotoController(DbScopeProvider scopeProvider)
        {
            this.scopeProvider = scopeProvider;
        }

        public async Task<byte[]> GetPhoto(int uco)
        {
            Osoby_Fotografie photo;
            using (var scope = scopeProvider.CreateReadOnlyScope())
            {
                photo = Osoby_Fotografie.SelectFromDB()
                   .FilterByOscisToUco(f => f.Osobni_Cislo, uco)
                   .SingleOrDefault();

            }
            if (photo != null)
                return photo.Fotografie;
            else
                return await GetNoPhoto();
        }

        public async Task<byte[]> GetNoPhoto()
        { 
            var file = IOHelper.MapPath("/Css/_Shared/personal-nophoto.jpg");
            byte[] result;
            using (FileStream stream = File.Open(file, FileMode.Open))
            {
                result = new byte[stream.Length];
                await stream.ReadAsync(result, 0, (int)stream.Length);
            }
            return result;
        }
    }
}
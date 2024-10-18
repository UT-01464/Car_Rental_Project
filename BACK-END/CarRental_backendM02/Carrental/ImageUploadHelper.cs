//using System.IO;
//using Microsoft.AspNetCore.Http;

//namespace Carrental.Helpers
//{
//    public class ImageUploadHelper
//    {
//        private readonly string _imageFolder;

//        public ImageUploadHelper(string imageFolder)
//        {
//            _imageFolder = imageFolder;
//        }

//        public string UploadImage(IFormFile imageFile)
//        {
//            if (imageFile == null || imageFile.Length == 0)
//            {
//                return null; // Or throw an exception, depending on your requirements
//            }

//            // Generate a unique filename
//            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
//            var filePath = Path.Combine(_imageFolder, fileName);

//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                imageFile.CopyTo(stream);
//            }

//            return fileName; // Return the filename to store in the database
//        }
//    }
//}

using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Carrental.Helpers
{
    public class ImageUploadHelper
    {
        private readonly string _uploadsFolder;

        public ImageUploadHelper(string uploadsFolder)
        {
            _uploadsFolder = uploadsFolder;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null; // Handle the case where no file is uploaded
            }

            var filePath = Path.Combine(_uploadsFolder, imageFile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return filePath; // Return the image URL or path as needed
        }
    }
}


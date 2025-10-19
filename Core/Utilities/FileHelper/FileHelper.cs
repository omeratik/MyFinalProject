using Core.Utilities.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.FileHelper
{
    public class FileHelper
    {
        public static string AddAsync(IFormFile file)
        {
            var result = newPath(file);
            try
            {
                var sourcepath = Path.GetTempFileName();
                if (file.Length > 0)
                    using (var stream = new FileStream(sourcepath, FileMode.Create))
                        file.CopyTo(stream);

                File.Move(sourcepath, result.newPath);
            }
            catch (Exception exception)
            {

                return exception.Message;
            }

            return result.Path2;
        }

        public static string UpdateAsync(string sourcePath, IFormFile file)
        {
            var result = newPath(file);

            try
            {
                //File.Copy(sourcePath,result);

                if (sourcePath.Length > 0)
                {
                    using (var stream = new FileStream(result.newPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                File.Delete(sourcePath);
            }
            catch (Exception excepiton)
            {
                return excepiton.Message;
            }

            return result.Path2;
        }

        public static IResult DeleteAsync(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception exception)
            {
                return new ErrorResult(exception.Message);
            }

            return new SuccessResult();
        }

        public static (string newPath, string Path2) newPath(IFormFile file)
        {
            try
            {
                FileInfo ff = new FileInfo(file.FileName);
                string fileExtension = ff.Extension;

                var creatingUniqueFilename = Guid.NewGuid().ToString("N")
                   + "_" + DateTime.Now.Month + "_"
                   + DateTime.Now.Day + "_"
                   + DateTime.Now.Year + fileExtension;

                // Projenin wwwroot klasörünün yolunu al
                string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string imagesPath = Path.Combine(webRootPath, "Images");

                // Images klasörü yoksa oluştur
                if (!Directory.Exists(imagesPath))
                {
                    Directory.CreateDirectory(imagesPath);
                }

                string resultPath = Path.Combine(imagesPath, creatingUniqueFilename);
                return (resultPath, $"/Images/{creatingUniqueFilename}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Dosya yolu oluşturma hatası: {ex.Message}");
            }
        }
    }
}
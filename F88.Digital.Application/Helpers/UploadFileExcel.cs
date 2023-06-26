using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Helpers
{
    public class UploadFileExcel
    {
        public static async Task<object> ReadFileExcel(IFormFile file,int numberSheet, Func<ISheet, object> func, IHostingEnvironment _env)
        {
            string fullPath = "";           
            try
            {
                if (file.Length > 0)
                {
                    string filename = Path.GetExtension(file.FileName).ToLower();
                    if (filename == ".xls" || filename == ".xlsx")
                    {
                        string folderName = $"UploadFiles/Excels/{DateTime.Now:dd-MM-yyyy}";
                        string webRootPath = _env.ContentRootPath;
                        string newPath = Path.Combine(webRootPath, folderName);
                        if (!Directory.Exists(newPath))
                        {
                            Directory.CreateDirectory(newPath);
                        }
                        string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                        ISheet sheet;
                        fullPath = Path.Combine(newPath, $"{DateTime.Now.ToString("ddMMyyyy")}_{file.FileName}");

                        var fileExcels = Directory.GetFiles(newPath);
                        foreach(var item in fileExcels)
                        {                         
                            if(item.Equals(fullPath))
                            {
                                System.IO.File.Delete(item);
                            }    
                            
                        }    
                        using (var input = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(input);
                            input.Position = 0;
                            if (filename == ".xls")
                            {
                                HSSFWorkbook hssfwb = new HSSFWorkbook(input);
                                sheet = hssfwb.GetSheetAt(numberSheet);
                            }
                            else
                            {
                                XSSFWorkbook hssfwb = new XSSFWorkbook(input);
                                sheet = hssfwb.GetSheetAt(numberSheet);
                            }
                            var rs = func(sheet);
                            //System.IO.File.Delete(newPath);
                            return rs;

                        }
                    }
                    else
                    {
                        return "Vui lòng chọn file khác";
                    }
                }
                return "Thất Bại";
            }
            catch(Exception e)
            {

                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
                return "Thất Bại";
            }
        }
    }
}

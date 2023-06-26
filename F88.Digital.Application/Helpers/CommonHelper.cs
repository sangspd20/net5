using F88.Digital.Application.Generics;
using F88.Digital.Application.Objects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Helpers
{
    public static class CommonHelper
    {
        public static void UploadPassportAsync(IFormFile file, out string fileName)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.

            var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Passport");

            if (!Directory.Exists(pathBuilt))
            {
                Directory.CreateDirectory(pathBuilt);
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Passport", fileName);

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(stream);
            }
        }

        public static void UploadAvatarAsync(IFormFile file, out string fileName)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.

            var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Avatar");

            if (!Directory.Exists(pathBuilt))
            {
                Directory.CreateDirectory(pathBuilt);
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Avatar", fileName);

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(stream);
            }
        }

        public static void UploadImageAsync(IFormFile file, string pathDirectory, out string fileName)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.

            var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), pathDirectory);

            if (!Directory.Exists(pathBuilt))
            {
                Directory.CreateDirectory(pathBuilt);
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), pathDirectory, fileName);

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(stream);
            }
        }

        public static PaginationSet<T> SetPagination<T>(IEnumerable<T> items, int page, int pageSize, int totalCount)
        {

            var paginationSet = new PaginationSet<T>()
            {
                StartNum = (page - 1) * pageSize + 1,
                EndNum = ((page) * pageSize > totalCount) ? totalCount : (page) * pageSize,
                Items = items,
                Page = page,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((decimal)totalCount / pageSize)
            };

            return paginationSet;
        }

        public static TimeObject GetPreviousMonthAndYearOfMonthAndYear(int month, int year)
        {
            var result = new TimeObject();

            if (month == 1)
            {
                result.Month = 12;
                result.Year = year - 1;
            }
            else
            {
                result.Month = month - 1;
                result.Year = year;
            }
            return result;
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}

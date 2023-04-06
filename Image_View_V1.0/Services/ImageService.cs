﻿using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_View_V1._0.Services
{
    public class ImageService
    {
        public ImageService()
        {
        
        }

        ImageSource imageSource;
        public async Task<ImageSource> GetImage()
        {


            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
              {
                  {DevicePlatform.WinUI, new[] {".png"} }
                    //{DevicePlatform.MacCatalyst, new[] { "public.image"} }
              })
            });

            SaveImage(result);

            using var stream = await result.OpenReadAsync();

            // Tworzenie MemoryStream z oryginalnego strumienia
            MemoryStream memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0; // Ustawienie pozycji na początek strumienia

            // Użycie MemoryStream zamiast oryginalnego strumienia
            imageSource = new StreamImageSource { Stream = cancellationToken => Task.FromResult((Stream)memoryStream) };

            return imageSource;
        }

        private async void SaveImage(FileResult file_result)
        {
            using var stream = await file_result.OpenReadAsync();
            using var memory_stream = new MemoryStream();

            stream.CopyTo(memory_stream);

            stream.Position = 0;
            memory_stream.Position = 0;

            //string folderPath = Environment.CurrentDirectory;
            //string projectFolderPath = Path.Combine(folderPath, "Images");

            //Debug.WriteLine(folderPath);
            await System.IO.File.WriteAllBytesAsync(
                @"C:\Users\marek\source\repos\Image_View_V1.0\Image_View_V1.0\Resources\Images\photo_processed.png", memory_stream.ToArray());
        }
    }

}

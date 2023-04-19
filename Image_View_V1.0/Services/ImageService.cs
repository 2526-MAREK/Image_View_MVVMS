﻿using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Maui.Controls;
using System.Reflection;

namespace Image_View_V1._0.Services
{
    public class ImageService
    {
        public ImageService()
        {
        
        }

        private async Task<ImageSource> GetImageSource(System.IO.Stream stream)
        {
            // Tworzenie MemoryStream z oryginalnego strumienia
            MemoryStream memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0; // Ustawienie pozycji na początek strumienia

            ImageSource imageSourceTemp;
            // Użycie MemoryStream zamiast oryginalnego strumienia
            imageSourceTemp = new StreamImageSource { Stream = cancellationToken => Task.FromResult((Stream)memoryStream) };

            return imageSourceTemp;

        }
        public async Task<ImageToProcess> GetImage()
        {
            ImageToProcess imageToProcess = new();

            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
              {
                  {DevicePlatform.WinUI, new[] {".png"} }
                    //{DevicePlatform.MacCatalyst, new[] { "public.image"} }
              })
            });

            if(result == null)
                return imageToProcess;

            using var stream1 = await result.OpenReadAsync();
            using var stream2 = await result.OpenReadAsync();

            SaveImage(stream1);

            imageToProcess.ImageSrcMain = await GetImageSource(stream1);

            imageToProcess.ImageSrcThumbnail = await GetImageSource(stream2);

            return imageToProcess;
        }

        private async void SaveImage(System.IO.Stream stream)
        {
            using var memory_stream = new MemoryStream();

            stream.CopyTo(memory_stream);

            stream.Position = 0;
            memory_stream.Position = 0;

            await System.IO.File.WriteAllBytesAsync(
                @"C:\Users\marek\OneDrive\Dokumenty\GitHub\Image_Viewer\Image_View_MVVC\Image_View_V1.0\Model\PythonScripts\photo_processed.png", memory_stream.ToArray());
        }

        public async Task<ChunkIHDR> GetChunkIHDR()
        {
            ChunkIHDR ChTemp = new();

            using var stream = await FileSystem.OpenAppPackageFileAsync("IHDR.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            JObject jObject = JObject.Parse(contents);

            ChTemp.Width = (int)jObject["width"];
            ChTemp.Height = (int)jObject["height"];
            ChTemp.BitDepth = (int)jObject["bit_depth"];

            return ChTemp;

        }

        private ImageSource LoadImageFromResource(string imagePath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            //ImageSource source = new FileImageSource { File = $"resource://{assembly.GetName().Name}.{imagePath}" };
            ImageSource source = new FileImageSource { File = imagePath };

            return source;
        }

        public ImageSource GetImageFFTFromProcess()
        {
             string imagePath = @"C:\Users\marek\OneDrive\Dokumenty\GitHub\Image_Viewer\Image_View_MVVC\Image_View_V1.0\Model\PythonScripts\fft.png"; // Ścieżka do zdjęcia w folderze projektu

            ImageSource source =  LoadImageFromResource(imagePath); //new FileImageSource { File = @"C:\Users\marek\source\repos\Image_View_V1.0\Image_View_V1.0\Resources\Images\fft.png" };

            return source;
        }

        public ImageSource GetImageMiniatureFromProcess()
        {
            string imagePath = @"C:\Users\marek\OneDrive\Dokumenty\GitHub\Image_Viewer\Image_View_MVVC\Image_View_V1.0\Resources\Images\miniature.png"; // Ścieżka do zdjęcia w folderze projektu

            ImageSource source = LoadImageFromResource(imagePath); //new FileImageSource { File = @"C:\Users\marek\source\repos\Image_View_V1.0\Image_View_V1.0\Resources\Images\fft.png" };

            return source;
        }

        public void RunPythonToImageProcess()
        {
            // 1) Create Process Info
            var psi = new ProcessStartInfo();
            psi.FileName = @"C:\Users\marek\AppData\Local\Programs\Python\Python311\python.exe";

            // 2) Provide script and arguments
            var script = @"C:\Users\marek\OneDrive\Dokumenty\GitHub\Image_Viewer\Image_View_MVVC\Image_View_V1.0\Model\PythonScripts\ImageProcess.py";
            var start = "2019-1-1";
            var end = "2019-1-22";

            psi.Arguments = $"\"{script}\" \"{start}\" \"{end}\"";

            // 3) Process configuration
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            // 4) Execute process and get output
            var errors = "";
            var results = "";

            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }

            // 5) Display output
            Debug.WriteLine("ERRORS:");
            Debug.WriteLine(errors);
            Debug.WriteLine("\n");
            Debug.WriteLine("Results:");
            Debug.WriteLine(results);

        }
    }

    

    }

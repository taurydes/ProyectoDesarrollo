using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Storage;
using SkiaSharp;
using UCABPagaloTodoMS.Application.Exceptions;

namespace UCABPagaloTodoMS.Application.Services;

/// <summary>
/// Clase que maneja la subida de archivos a Firebase Storage.
/// </summary>
public class FirebaseStorageUploader
{
    private readonly string _apiKey = "AIzaSyDUCpffWEHgBHChFFyPyzwN9qXQO8SgRd8";
    private readonly string _bucket = "ucab2023-taury.appspot.com";
    private readonly string _authEmail = "desarrollo2023@gmail.com";
    private readonly string _authPassword = "desarrollo2023";

    public FirebaseStorageUploader()
    {
    }

    /// <summary>
    /// Sube un archivo a Firebase Storage y devuelve su URL de descarga.
    /// </summary>
    /// <param name="fileName">El nombre del archivo a subir.</param>
    /// <param name="firebaseStoragePath">La ruta en Firebase Storage donde se guardará el archivo.</param>
    /// <param name="csvContent">El contenido del archivo CSV que se va a subir.</param>
    /// <returns>La URL de descarga del archivo subido.</returns>
    public async Task<string> UploadFileAsync(string firebaseStoragePath, string csvContent)
    {
        try
        {
          var FirebaseStorage = new FirebaseStorage(_bucket);
          var dowloadURL = "";

            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent.ToString())))
            {
                var firebaseStoragePathBase = firebaseStoragePath + "-" + DateTime.Now.ToString("dd-MM-yyyy_HH:mm:ss") + ".csv";
                var load = await FirebaseStorage.Child(firebaseStoragePathBase).PutAsync(memoryStream);
                dowloadURL = await FirebaseStorage.Child(firebaseStoragePathBase).GetDownloadUrlAsync();
            }
            if (dowloadURL == "")
            {
                throw new CustomException("Error en guardado de archivo en firebase");
            }

            return dowloadURL;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al subir el archivo: {ex.Message}");
            return null;
        }
    }
}

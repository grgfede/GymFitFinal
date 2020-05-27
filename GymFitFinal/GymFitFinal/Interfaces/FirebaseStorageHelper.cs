using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Firebase.Storage;


namespace GymFitFinal.Interfaces
{
   
    class FirebaseStorageHelper
    {
        FirebaseStorage firebaseStorage = new FirebaseStorage("xamarinfirebase-gymfitt-2b845.appspot.com");
        public async Task<string> UploadFile(Stream fileStream, string fileName)
        {
            var imageUrl = await firebaseStorage
                .Child("XamarinMonkeys")
                .Child(fileName)
                .PutAsync(fileStream);
            return imageUrl;
        }
    }
}

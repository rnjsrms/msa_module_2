using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using faceanalysis.Model;

namespace faceanalysis
{
    public partial class Analysis : ContentPage
    {
        private readonly IFaceServiceClient faceServiceClient = new FaceServiceClient("c40be061c66f45309e9d3e903fef98f7", "https://westcentralus.api.cognitive.microsoft.com/face/v1.0");

        Face[] faces;
        String faceDescription;
        List<object> descriptionList = new List<object>();

        public Analysis()
        {
            InitializeComponent();
        }

        private async void postButton(object sender, EventArgs e)
        {
            post.Text = "Posting...";
            post.IsEnabled = false;

            FaceAnalysisModel model = new FaceAnalysisModel()
            {
                Gender = (string)descriptionList[0],
                Age = (float)descriptionList[1],
                Smile = (float)descriptionList[2],
                Anger = (float)descriptionList[3],
                Contempt = (float)descriptionList[4],
                Disgust = (float)descriptionList[5],
                Fear = (float)descriptionList[6],
                Happiness = (float)descriptionList[7],
                Neutral = (float)descriptionList[8],
                Sadness = (float)descriptionList[9],
                Surprise = (float)descriptionList[10],
                Glasses = (string)descriptionList[11],
                Bald = (float)descriptionList[12],
                Black = (float)descriptionList[13],
                Other = (float)descriptionList[14],
                Red = (float)descriptionList[15],
                Brown = (float)descriptionList[16],
                Gray = (float)descriptionList[17],
                Blonde = (float)descriptionList[18]
            };

            await AzureManager.AzureManagerInstance.PostFaceAnalysisInformation(model);
            await DisplayAlert("Done", "Result has been successfully uploaded!", "OK");
            post.Text = "Posted";
        }

        private async void loadCamera(object sender, EventArgs e)
        {
            descriptionList.Clear();
            post.IsEnabled = false;
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                return file.GetStream();
            });

            description.Text = "UPLOADING PICTURE TO SERVER...";
            faces = await UploadAndDetectFaces(file);

            if (faces.Length > 0)
            {
                Face face = faces[0];
                faceDescription = FaceDescription(face);

                description.Text = faceDescription;
                post.IsEnabled = true;
                post.Text = "POST RESULT TO SERVER";
            }
            else
            {
                description.Text = "FACE NOT FOUND!";
            }

            file.Dispose();
        }

        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        private async Task<Face[]> UploadAndDetectFaces(MediaFile file)
        {
            IEnumerable<FaceAttributeType> faceAttributes =
                new FaceAttributeType[] { FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile, FaceAttributeType.Emotion, FaceAttributeType.Glasses, FaceAttributeType.Hair };


            byte[] byteData = GetImageAsByteArray(file);
            try
            {
                using (Stream imageFileStream = new MemoryStream(byteData))
                {
                    Face[] faces = await faceServiceClient.DetectAsync(imageFileStream, returnFaceId: true, returnFaceLandmarks: false, returnFaceAttributes: faceAttributes);
                    return faces;
                }
            }
            catch (FaceAPIException f)
            {
                return new Face[0];
            }
            catch (Exception e)
            {
                return new Face[0];
            }
        }

        private string FaceDescription(Face face)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Face Analysis");
            sb.Append("\n\n\nGender: ");
            sb.Append(face.FaceAttributes.Gender);
            sb.Append("\n\nAge: ");
            sb.Append(face.FaceAttributes.Age);
            sb.Append("\n\nSmile: ");
            sb.Append(String.Format("{0:F1}%", face.FaceAttributes.Smile * 100));

            sb.Append("\n\nEmotion:\n");
            EmotionScores emotionScores = face.FaceAttributes.Emotion;
            sb.Append(String.Format("Anger {0:F1}%\n", emotionScores.Anger * 100));
            sb.Append(String.Format("Contempt {0:F1}%\n", emotionScores.Contempt * 100));
            sb.Append(String.Format("Disgust {0:F1}%\n", emotionScores.Disgust * 100));
            sb.Append(String.Format("Fear {0:F1}%\n", emotionScores.Fear * 100));
            sb.Append(String.Format("Happiness {0:F1}%\n", emotionScores.Happiness * 100));
            sb.Append(String.Format("Neutral {0:F1}%\n", emotionScores.Neutral * 100));
            sb.Append(String.Format("Sadness {0:F1}%\n", emotionScores.Sadness * 100));
            sb.Append(String.Format("Surprise {0:F1}%", emotionScores.Surprise * 100));

            sb.Append("\n\nGlasses: ");
            sb.Append(face.FaceAttributes.Glasses);
            sb.Append("\n\nHair:\n");
            sb.Append(String.Format("Bald {0:F1}%\n", face.FaceAttributes.Hair.Bald * 100));

            descriptionList.Add((string)face.FaceAttributes.Gender);
            descriptionList.Add((float)face.FaceAttributes.Age);
            descriptionList.Add((float)face.FaceAttributes.Smile * 100);
            descriptionList.Add((float)emotionScores.Anger * 100);
            descriptionList.Add((float)emotionScores.Contempt * 100);
            descriptionList.Add((float)emotionScores.Disgust * 100);
            descriptionList.Add((float)emotionScores.Fear * 100);
            descriptionList.Add((float)emotionScores.Happiness * 100);
            descriptionList.Add((float)emotionScores.Neutral * 100);
            descriptionList.Add((float)emotionScores.Sadness * 100);
            descriptionList.Add((float)emotionScores.Surprise * 100);
            descriptionList.Add((string)face.FaceAttributes.Glasses.ToString());
            descriptionList.Add((float)face.FaceAttributes.Hair.Bald * 100);

            HairColor[] hairColors = face.FaceAttributes.Hair.HairColor;
            foreach (HairColor hairColor in hairColors)
            {
                sb.Append(hairColor.Color.ToString());
                sb.Append(String.Format(" {0:F1}%\n", hairColor.Confidence * 100));
                descriptionList.Add((float)hairColor.Confidence * 100);
            }

            return sb.ToString();
        }
    }
}

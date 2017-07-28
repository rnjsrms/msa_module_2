using faceanalysis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace faceanalysis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AzureTable : ContentPage
    {
        public AzureTable()
        {
            InitializeComponent();
        }


        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            status.Text = "Fetching data from server...";
            List<FaceAnalysisModel> FaceAnalysisInformation = await AzureManager.AzureManagerInstance.GetFaceAnalysisInformation();

            FaceAnalysisList.ItemsSource = FaceAnalysisInformation;
            status.Text = "Done!";
        }
    }
}
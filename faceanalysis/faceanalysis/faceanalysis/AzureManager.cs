using faceanalysis.Model;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace faceanalysis
{
    public class AzureManager
    {

        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<FaceAnalysisModel> FaceAnalysisTable;

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://faceanalysis.azurewebsites.net/");
            this.FaceAnalysisTable = this.client.GetTable<FaceAnalysisModel>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }

        public async Task<List<FaceAnalysisModel>> GetFaceAnalysisInformation()
        {
            return await this.FaceAnalysisTable.ToListAsync();
        }

        public async Task PostFaceAnalysisInformation(FaceAnalysisModel faceAnalysisModel)
        {
            await this.FaceAnalysisTable.InsertAsync(faceAnalysisModel);
        }
    }
}

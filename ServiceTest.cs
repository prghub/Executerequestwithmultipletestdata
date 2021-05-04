using Cascade_Functional_Tests.Authentication;
using Cascade_Functional_Tests.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductService.Models.Request;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Cascade_Functional_Tests.Base
{
    [TestClass]
    public class ServiceTest
    {
        protected RestClient restClient;
        protected String access;
        protected RestRequest restRequest;
        protected IRestResponse response;

        protected String scope;
        protected String url;

        protected IConfigurationRoot configuration;

        public ServiceTest()
        {
            configuration = new ConfigurationBuilder()
           .AddJsonFile("ConfigProperties.json")
           .Build();
        }

        public List<ProductCategory> CreateProductCategory(int numberOfObjects)
        {
            List<ProductCategory> listProductCategory = new List<ProductCategory>();
            Boolean badUserId = true;

            Audit audit = new Audit(1, 130, null, null, false);


            for (int i = 1; i <= numberOfObjects; i++)
            {
                int createdByUserId;
                if (badUserId)
                {
                    createdByUserId = 999;
                }
                else
                {
                    createdByUserId = 1;
                    badUserId = false;
                }

                String title = Faker.Lorem.Sentence(1);

                ProductCategory prodCat = new ProductCategory(Faker.Lorem.Sentence(1), Faker.Lorem.Sentence(5), 1, false, null, audit);
                //productCategory.Description = Faker.Lorem.Sentences(5).ToString();

                if (new Random().Next(0, 1) != 0)
                {
                    prodCat.Description = Faker.Lorem.Sentence(new Random().Next(1, 5));
                }

                listProductCategory.Add(prodCat);
            }
            return listProductCategory;
        }

        [TestInitialize]
        public void PrepareTest()
        {

            restClient = new RestClient(url);

            //GET Security Token
            access = GetAccessToken();
            restRequest = new RestRequest();
            restRequest.AddHeader("Cache-Control", "no-cache");
            restRequest.AddHeader("Authorization", "Bearer " + access + "");
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "application/json");
        }

        [TestCleanup]
        public void TearDownTest()
        {

        }


        private String GetAccessToken()
        {
            String accessToken = null;
            //Get Url for authorisation token from config file
            string fedUrl = configuration["FedTokenUrl"];

            RestClient client = new RestClient(fedUrl);
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("client_id", "cascadeDevTest");
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("client_secret", "yYHX6v4OPl8b");
            request.AddParameter("scope", scope);

            IRestResponse response = client.Execute(request);
            APIHelper<AccessTokenResponseDTO> token = new APIHelper<AccessTokenResponseDTO>();
            AccessTokenResponseDTO content = token.GetContent<AccessTokenResponseDTO>(response);

            if (content.Access_token != null)
            {
                accessToken = content.Access_token;

            }
            return accessToken;
        }

    }
}

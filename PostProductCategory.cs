using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductService.Models.Request;
using RestSharp;
using System;
using ProductService.Models.Request;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace ProductService.TestProductCategory.Tests
{
    /// <summary>
    /// Tests for the endpoints in ProductService
    /// https://cascadetest-productservice.azurewebsites.net/swagger/index.html
    /// </summary>
    [TestClass]
    public class PostProductCategory : ProductServiceTest
    {
        /// <summary>
        /// Post Request for create product category
        /// Tests the status code, response body and schema
        /// </summary>
        [TestMethod]
        [TestCategory("ProductCategory")]
        public void TestCreateProductCategory()
        {
            System.Diagnostics.Debug.WriteLine("ProductCategory");

            string path = @"C:\Users\preethy.ramamoorthy\source\repos\cascade-api-test-framework\ProductService\ProductCategory\TestData1.json";
            string readText = File.ReadAllText(path);
            restRequest.Resource = "productcategories";
            restRequest.Method = Method.POST;
            //restRequest.AddParameter("application/json", "C:/Users/preethy.ramamoorthy/source/repos/cascade-api-test-framework/ProductService/ProductCategory/TestData2.json", ParameterType.RequestBody);
            restRequest.AddJsonBody(readText);

            IRestResponse response = restClient.Execute(restRequest);
            System.Diagnostics.Debug.WriteLine("1.." + response.Content);
            System.Diagnostics.Debug.WriteLine("2.." + response.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }

        /// <summary>
        /// Post Request for create product category
        /// Tests the status code, response body and schema
        /// </summary>
        [TestMethod]
        [TestCategory("ProductCategory")]
        public void TestCreateProductCategoryFakeData()
        {
            restRequest.Resource = "productcategories";
            restRequest.Method = Method.POST;

            List<ProductCategory> lstProdCat = CreateProductCategory(100);

            List<IRestResponse> responseList = new List<IRestResponse>();

            foreach (ProductCategory prodCat in lstProdCat)
            {
                restRequest.AddJsonBody(prodCat);
                System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(prodCat));


                IRestResponse response = restClient.Execute(restRequest);
                responseList.Add(response);
            }

            foreach (IRestResponse response in responseList)
            {
                System.Diagnostics.Debug.WriteLine(response.StatusCode + "----" + response.Content);

            }

        }


        /// <summary>
        /// Post Request for create product category
        /// Tests the status code, response body and schema
        /// </summary>
        [TestMethod]
        [TestCategory("ProductCategory")]
        public void TestCreateProductCategoryValid()
        {
            restRequest.Resource = "productcategories";
            restRequest.Method = Method.POST;

            Audit audit = new Audit(1, 130, null, null, false);

            ProductCategory prodCat = new ProductCategory("PRTEST" + Faker.Lorem.Sentence(1), Faker.Lorem.Sentence(5), 11, false, null, audit);

            // Add OrderDTO to Rest Request
            restRequest.AddJsonBody(prodCat);

            //Exucute the API
            IRestResponse response = restClient.Execute(restRequest);

            String responseString = response.Content;

            System.Diagnostics.Debug.WriteLine("Response body is-------" + responseString);

            //To test the response status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
    }
}

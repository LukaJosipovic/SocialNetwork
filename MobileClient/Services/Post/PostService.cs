using Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Services.Post
{
    public class PostService : IPostService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PostService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GeneralResponse> DeletePostAdmin(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.DeleteAsync($"DeletePostAdmin?postId={id}");
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<PostDetailsResponse>> GetAllPosts()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync("GetAllPosts");
                var responseObject = await response.Content.ReadFromJsonAsync<List<PostDetailsResponse>>();
                return responseObject;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<PostDetailsResponse> GetPostById(int id)
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.GetAsync($"GetPostById?id={id}");
            var responseObject = await response.Content.ReadFromJsonAsync<PostDetailsResponse>();
            return responseObject;
        }

        public async Task<List<PostDetailsResponse>> GetReportedPosts()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync("GetReportedPosts");
                var responseObject = await response.Content.ReadFromJsonAsync<List<PostDetailsResponse>>();
                return responseObject;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<GeneralResponse> RemoveReport(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.DeleteAsync($"RemoveReport?postId={id}");
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<GeneralResponse> ReportPost(int id)
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.PostAsync($"ReportPost?id={id}", null);
            var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return responseObject;
        }

        public async Task<LikeResponse> LikePost(int postId)
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.PostAsync($"LikePost?postId={postId}", null);
            var responseObject = await response.Content.ReadFromJsonAsync<LikeResponse>();
            return responseObject;
        }

        public async Task<LikeResponse> DislikePost(int postId)
        {
            var client = _httpClientFactory.CreateClient("BaseApi");
            var response = await client.DeleteAsync($"DeletePost?postId={postId}");
            var responseObject = await response.Content.ReadFromJsonAsync<LikeResponse>();
            return responseObject;
        }
    }
}

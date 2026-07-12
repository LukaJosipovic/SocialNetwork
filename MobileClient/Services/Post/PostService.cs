using Application.DTO.Request;
using Application.DTO.Response;
using Application.Exceptions;
using Application.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
                var response = await client.DeleteAsync($"api/Post/DeletePostAdmin?postId={id}");
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            //catch (AccountBannedException ex)
            //{
            //    return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            //}
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<PostDetailsResponse> GetAllPosts(PageSettingsRequest model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Post/GetAllPosts?PageNumber={model.PageNumber}&PageSize={model.PageSize}");
                var responseObject = await response.Content.ReadFromJsonAsync<PostDetailsResponse>();
                return responseObject;
            }
            //catch (AccountBannedException ex)
            //{
            //    return ResponseHelper.CreatePostDetailsResponse(false, ex.Message, null);
            //}
            catch (Exception ex)
            {
                return ResponseHelper.CreatePostDetailsResponse(false, "Something went wrong", null);
            }
        }

        public async Task<PostDetailsDTO> GetPostById(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync($"api/Post/GetPostById?id={id}");
                var responseObject = await response.Content.ReadFromJsonAsync<PostDetailsDTO>();
                return responseObject;
            }
            //catch (AccountBannedException ex)
            //{
            //    return new PostDetailsDTO
            //    {
            //        IsSuccess = false,
            //        Message = ex.Message
            //    };
            //}
            catch (Exception ex)
            {
                return new PostDetailsDTO
                {
                    IsSuccess = false,
                    Message = "Something went wrong"
                };
            }
        }

        public async Task<PostDetailsResponse> GetReportedPosts()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.GetAsync("api/Post/GetReportedPosts");
                var responseObject = await response.Content.ReadFromJsonAsync<PostDetailsResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreatePostDetailsResponse(false, ex.Message, null);
            }
        }

        public async Task<GeneralResponse> RemoveReport(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.DeleteAsync($"api/Post/RemoveReport?postId={id}");
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<GeneralResponse> ReportPost(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PostAsync($"api/Post/ReportPost?id={id}", null);
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            //catch (AccountBannedException ex)
            //{
            //    return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            //}
            catch (Exception)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<LikeResponse> LikePost(int postId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.PostAsync($"api/Post/LikePost?postId={postId}", null);
                var responseObject = await response.Content.ReadFromJsonAsync<LikeResponse>();
                return responseObject;
            }
            //catch (AccountBannedException ex)
            //{
            //    return ResponseHelper.CreateLikeResponse(false, ex.Message, 0, postId);
            //}
            catch (Exception ex)
            {
                return ResponseHelper.CreateLikeResponse(false, "Something went wrong", 0, postId);
            }
        }

        public async Task<LikeResponse> DislikePost(int postId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.DeleteAsync($"api/Post/DislikePost?postId={postId}");
                var responseObject = await response.Content.ReadFromJsonAsync<LikeResponse>();
                return responseObject;
            }
            //catch (AccountBannedException ex)
            //{
            //    return ResponseHelper.CreateLikeResponse(false, ex.Message, 0, postId);
            //}
            catch (Exception ex)
            {
                return ResponseHelper.CreateLikeResponse(false, "Something went wrong", 0, postId);
            }
        }

        public async Task<GeneralResponse> DeletePost(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("BaseApi");
                var response = await client.DeleteAsync($"api/Post/DeletePost?postId={id}");
                var responseObject = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return responseObject;
            }
            //catch (AccountBannedException ex)
            //{
            //    return ResponseHelper.CreateGeneralResponse(false, ex.Message);
            //}
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }
    }
}

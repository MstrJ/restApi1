using Application.Dto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllPosts();
        Task<PostDto> GetPostById(int id);

        PostDto AddNewPost(CreatePostDto newpost);

        void UpdatePost(UpdatePostDto updatePost);

        void DeletePost(int id);

        public Task KafkaProducer(PostDto post);
    }
}

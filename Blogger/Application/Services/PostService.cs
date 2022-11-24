using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        public PostService(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public Task<IEnumerable<PostDto>> GetAllPosts()
        {
            var posts = _postRepository.GetAll();
            return Task.FromResult(_mapper.Map<IEnumerable<PostDto>>(posts));
        }

        public async Task<PostDto> GetPostById(int id)
        {
            var post =  Task.FromResult(_postRepository.GetById(id));
            return _mapper.Map<PostDto>(post);
        }
        public PostDto AddNewPost(CreatePostDto newpost)
        {
            if (string.IsNullOrEmpty(newpost.Title))
            {
                throw new Exception("Post can not have an empty title");
            }
            var post = _mapper.Map<Post>(newpost);

            return _mapper.Map<PostDto>(_postRepository.Add(post));
        }

        public async void UpdatePost(UpdatePostDto updatePost)
        {
            var existingPost =await _postRepository.GetById(updatePost.Id);
            var post = _mapper.Map(updatePost, existingPost);   
            _postRepository.Update(post);
        }

        public async void DeletePost(int id)
        {
            var post = await _postRepository.GetById(id);
            _postRepository.Delete(post);
        }

        public Task KafkaProducer(PostDto post)
        {
            throw new NotImplementedException();
        }
    }
}

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
    public class MongoService : IPostService
    {
        private readonly IPostRepository _mongoRepository;
        private readonly IMapper _mapper;
        public MongoService(IPostRepository mongo, IMapper mapper)
        {
            _mongoRepository = mongo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostDto>> GetAllPosts()
        {
            return _mapper.Map<IEnumerable<PostDto>>(await _mongoRepository.GetAll());

        }

        public async Task<PostDto> GetPostById(int id)
        {
            Post post  = await _mongoRepository.GetById(id);
            return _mapper.Map<PostDto>(post);
        }
        public PostDto AddNewPost(CreatePostDto newpost)
        {
            if (string.IsNullOrEmpty(newpost.Title))
            {
                throw new Exception("Post can not have an empty title");
            }
            var post = _mapper.Map<Post>(newpost);
            //_mongoRepository.Add(post);
            return _mapper.Map<PostDto>(_mongoRepository.Add(post));
        }

        public async void UpdatePost(UpdatePostDto updatePost)
        {
            var existingPost = await _mongoRepository.GetById(updatePost.Id);
            var post = _mapper.Map(updatePost, existingPost);
            _mongoRepository.Update(post);
        }

        public async void DeletePost(int id)
        {
            var post = await _mongoRepository.GetById(id);
            _mongoRepository.Delete(post);
        }
    }
}

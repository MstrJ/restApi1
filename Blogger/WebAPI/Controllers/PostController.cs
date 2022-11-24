using Application.Dto;
using Application.Interfaces;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [SwaggerOperation(Summary = "Retrieves all posts")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = await _postService.GetAllPosts();
            return Ok(posts);
        }

        [SwaggerOperation(Summary = "Retrieves a specific post by unique id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var post =await _postService.GetPostById(id);
            if(post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [SwaggerOperation(Summary ="Create a new post")]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostDto newPost)
        {
            if(newPost.Title.Equals("string") || newPost.Content.Equals("string"))
            {
                return BadRequest();
            }
            var post = _postService.AddNewPost(newPost);

            await _postService.KafkaProducer(post);

            return Created($"api/posts/{post.Id}",post);
        }


        [SwaggerOperation(Summary ="Update a existing post")]
        [HttpPut]
        public IActionResult Update(UpdatePostDto updatePost)
        {
            _postService.UpdatePost(updatePost);
            return NoContent();
        }
        [SwaggerOperation(Summary ="Delete a specific post")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _postService.DeletePost(id);
            return NoContent(); // 204
        }
    }
}

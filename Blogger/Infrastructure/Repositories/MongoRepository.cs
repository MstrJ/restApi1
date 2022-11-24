using Domain.Entities;
using Domain.Interfaces;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace Infrastructure.Repositories
{
    public class MongoRepository : IPostRepository
    {
        //connect to mongodb
        private IMongoCollection<Post> _mongoPosts;

        public MongoRepository(IMongoClient client)
        {
            var database = client.GetDatabase("Blogger");
            _mongoPosts = database.GetCollection<Post>("Posts");

        }


        public Post Add(Post post)
        {
            int lastId = _mongoPosts.AsQueryable().ToList().Last().Id;
            int id = lastId+1;
            for (int i = lastId; i < _mongoPosts.AsQueryable().Count(); i++)
            {
                bool located = _mongoPosts.AsQueryable().ToList().FirstOrDefault(x => x.Id == i) == null ? false : true;
                if (!located)
                {
                    id = i;
                    break;
                }
            }
            post.Id = id;
            post.Created = DateTime.UtcNow;
            _mongoPosts.InsertOneAsync(post);
            return post;
        }

        public void Delete(Post post)
        {
            _mongoPosts.FindOneAndDelete(x => x.Id == post.Id);
        }

        public async Task<IEnumerable<Post>> GetAll()
        {
            var list = await _mongoPosts.Find(_ => true).ToListAsync();
            return list;

        }

        public async Task<Post> GetById(int id)
        {
            var r = await _mongoPosts.FindAsync(x => x.Id == id);
            return r.ToList().First();
        }

        public void Update(Post post)
        {
            var options = new FindOneAndUpdateOptions<Post> { ReturnDocument = ReturnDocument.Before };
            var filter = Builders<Post>.Filter.Eq(x => x.Id, post.Id);
            var update = Builders<Post>.Update.Set(x => x.Content ,post.Content);
            _mongoPosts.FindOneAndUpdate(filter, update, options);
            //_mongoPosts.update(filter,update,options);
        }
    }
}

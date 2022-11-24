using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    // ZMIENIC
    public class PostRepository /*: IPostRepository*/
    {
        private static readonly ISet<Post> _posts = new HashSet<Post>()
        {
            new Post(1, "Dzien 1","Dzisiaj na swojej drodze spotkałem Łosia! Jest on ogromyn zwierzęciem"),
            new Post(2, "Dzien 2","Po wczorajszej walce z Łosiem, ide w strone szpitala"),
            new Post(3, "Dzien 3","Wróciłem ze szpitala, i poszedłem się zaopatrzyć w broń")
        };
        private static int _postsCount = _posts.Count();
        public IEnumerable<Post> GetAll()
        {
            List<Post> SortedList = new List<Post>();
            SortedList = _posts.OrderBy(x => x.Id).ToList();
            return SortedList;
        }

        public Post GetById(int id)
        {
            return _posts.SingleOrDefault(x => x.Id == id);
        }
        public Post Add(Post post)
        {
            int lastId = _posts.Last().Id;
            int id = lastId + 1;
            for (int i = lastId; i < _posts.Count(); i++)
            {
                bool located = _posts.AsQueryable().ToList().FirstOrDefault(x => x.Id == i) == null ? false : true;
                if (!located)
                {
                    id = i;
                    break;
                }
            }
            post.Id = id;
            post.Created = DateTime.UtcNow;
            _posts.Add(post);
            return post;


            //int lastId = _postsCount+=1;
            //post.Id = lastId;
            //post.Created = DateTime.UtcNow;
            //_posts.Add(post);
            //_postsCount += 1;
            //return post;
        }
        public void Update(Post post)
        {
            post.LastModified = DateTime.UtcNow;
        }

        public void Delete(Post post)
        {
            _posts.Remove(post);
        }


    }
}

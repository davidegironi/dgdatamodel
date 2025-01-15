using DG.Data.Model.Helpers;
#if NETFRAMEWORK
using DG.DataModelSample.Model.Entity;
#else
using DG.DataModelSample.Model.Entity.Models;
#endif
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Serialization;

namespace DG.DataModelSample.Model.Test
{
    [TestFixture]
    public partial class DGDataModelSampleModel
    {
        [Test]
        public void DGPredicateBuilder1()
        {
            List<posts> posts = null;
            posts _posts = null;
            Expression<Func<posts, bool>> predicate = null;
            Expression<Func<posts, bool>> predicateor = null;

            //add post
            _posts = new posts();
            _posts.blogs_id = samplemodel.Blogs.FirstOrDefault(a => a.blogs_title == "Blog1").blogs_id;
            _posts.posts_title = "Post3";
            _posts.posts_text = "test text 1";
            _posts.posts_username = "username";
            _posts.posts_email = "username@email.com";
            samplemodel.Posts.Add(_posts);
            _posts = new posts();
            _posts.blogs_id = samplemodel.Blogs.FirstOrDefault(a => a.blogs_title == "Blog1").blogs_id;
            _posts.posts_title = "Post4";
            _posts.posts_text = "test text 4";
            _posts.posts_username = "username";
            _posts.posts_email = "username@email.com";
            samplemodel.Posts.Add(_posts);

            predicate = DGPredicateBuilder.True<posts>();
            predicate = predicate.And(r => r.posts_title == "Post3");
            predicate = predicate.And(r => r.posts_username == "username");
            posts = samplemodel.Posts.List(predicate).ToList();
            Assert.That(posts.Count, Is.EqualTo(1));

            predicate = DGPredicateBuilder.True<posts>();
            predicate = predicate.And(r => r.posts_title == "Post3");
            predicate = predicate.And(r => r.posts_username == "username");
            predicate = predicate.And(r => r.posts_email == "username@email.com0");
            posts = samplemodel.Posts.List(predicate).ToList();
            Assert.That(posts.Count, Is.EqualTo(0));

            predicate = DGPredicateBuilder.True<posts>();
            predicateor = DGPredicateBuilder.False<posts>();
            predicateor = predicateor.Or(r => r.posts_title == "Post4");
            predicateor = predicateor.Or(r => r.posts_title == "Post3");
            predicate = predicate.And(predicateor);
            posts = samplemodel.Posts.List(predicate).ToList();
            Assert.That(posts.Count, Is.EqualTo(2));

            predicate = DGPredicateBuilder.True<posts>();
            predicateor = DGPredicateBuilder.False<posts>();
            predicateor = predicateor.Or(r => r.posts_title == "Post4");
            predicateor = predicateor.Or(r => r.posts_title == "Post3");
            predicate = predicate.And(predicateor);
            predicate = predicate.And(r => r.posts_title == "Post3");
            posts = samplemodel.Posts.List(predicate).ToList();
            Assert.That(posts.Count, Is.EqualTo(1));
        }

        private class APost
        {
            public int posts_id { get; set; }
            public string posts_name { get; set; }
            public string posts_description { get; set; }
        }
        private class VAPost
        {
            public string posts_name { get; set; }
        }

        [Test]
        public void DGDataTableUtils_ToDataTable1()
        {
            List<APost> posts = new List<APost>();
            posts.Add(new APost()
            {
                posts_id = 1,
                posts_name = "Post1",
                posts_description = "test description"
            });
            posts.Add(new APost()
            {
                posts_id = 2,
                posts_name = "Post2",
                posts_description = "test description"
            });
            IEnumerable<VAPost> posts_enum = posts.Select(r =>
                new VAPost()
                {
                    posts_name = r.posts_name
                });

            DataTable dt = DGDataTableUtils.ToDataTable<VAPost>(posts_enum);

            XmlSerializer xmlSerializer = new XmlSerializer(dt.GetType());
            StringWriter textWriter = new StringWriter();
            xmlSerializer.Serialize(textWriter, dt);

            string expected = @"PAA/AHgAbQBsACAAdgBlAHIAcwBpAG8AbgA9ACIAMQAuADAAIgAgAGUAbgBjAG8AZABpAG4AZwA9ACIAdQB0AGYALQAxADYAIgA/AD4ADQAKADwARABhAHQAYQBUAGEAYgBsAGUAPgANAAoAIAAgADwAeABzADoAcwBjAGgAZQBtAGEAIABpAGQAPQAiAE4AZQB3AEQAYQB0AGEAUwBlAHQAIgAgAHgAbQBsAG4AcwA9ACIAIgAgAHgAbQBsAG4AcwA6AHgAcwA9ACIAaAB0AHQAcAA6AC8ALwB3AHcAdwAuAHcAMwAuAG8AcgBnAC8AMgAwADAAMQAvAFgATQBMAFMAYwBoAGUAbQBhACIAIAB4AG0AbABuAHMAOgBtAHMAZABhAHQAYQA9ACIAdQByAG4AOgBzAGMAaABlAG0AYQBzAC0AbQBpAGMAcgBvAHMAbwBmAHQALQBjAG8AbQA6AHgAbQBsAC0AbQBzAGQAYQB0AGEAIgA+AA0ACgAgACAAIAAgADwAeABzADoAZQBsAGUAbQBlAG4AdAAgAG4AYQBtAGUAPQAiAE4AZQB3AEQAYQB0AGEAUwBlAHQAIgAgAG0AcwBkAGEAdABhADoASQBzAEQAYQB0AGEAUwBlAHQAPQAiAHQAcgB1AGUAIgAgAG0AcwBkAGEAdABhADoATQBhAGkAbgBEAGEAdABhAFQAYQBiAGwAZQA9ACIAVgBBAFAAbwBzAHQAIgAgAG0AcwBkAGEAdABhADoAVQBzAGUAQwB1AHIAcgBlAG4AdABMAG8AYwBhAGwAZQA9ACIAdAByAHUAZQAiAD4ADQAKACAAIAAgACAAIAAgADwAeABzADoAYwBvAG0AcABsAGUAeABUAHkAcABlAD4ADQAKACAAIAAgACAAIAAgACAAIAA8AHgAcwA6AGMAaABvAGkAYwBlACAAbQBpAG4ATwBjAGMAdQByAHMAPQAiADAAIgAgAG0AYQB4AE8AYwBjAHUAcgBzAD0AIgB1AG4AYgBvAHUAbgBkAGUAZAAiAD4ADQAKACAAIAAgACAAIAAgACAAIAAgACAAPAB4AHMAOgBlAGwAZQBtAGUAbgB0ACAAbgBhAG0AZQA9ACIAVgBBAFAAbwBzAHQAIgA+AA0ACgAgACAAIAAgACAAIAAgACAAIAAgACAAIAA8AHgAcwA6AGMAbwBtAHAAbABlAHgAVAB5AHAAZQA+AA0ACgAgACAAIAAgACAAIAAgACAAIAAgACAAIAAgACAAPAB4AHMAOgBzAGUAcQB1AGUAbgBjAGUAPgANAAoAIAAgACAAIAAgACAAIAAgACAAIAAgACAAIAAgACAAIAA8AHgAcwA6AGUAbABlAG0AZQBuAHQAIABuAGEAbQBlAD0AIgBwAG8AcwB0AHMAXwBuAGEAbQBlACIAIAB0AHkAcABlAD0AIgB4AHMAOgBzAHQAcgBpAG4AZwAiACAAbQBpAG4ATwBjAGMAdQByAHMAPQAiADAAIgAgAC8APgANAAoAIAAgACAAIAAgACAAIAAgACAAIAAgACAAIAAgADwALwB4AHMAOgBzAGUAcQB1AGUAbgBjAGUAPgANAAoAIAAgACAAIAAgACAAIAAgACAAIAAgACAAPAAvAHgAcwA6AGMAbwBtAHAAbABlAHgAVAB5AHAAZQA+AA0ACgAgACAAIAAgACAAIAAgACAAIAAgADwALwB4AHMAOgBlAGwAZQBtAGUAbgB0AD4ADQAKACAAIAAgACAAIAAgACAAIAA8AC8AeABzADoAYwBoAG8AaQBjAGUAPgANAAoAIAAgACAAIAAgACAAPAAvAHgAcwA6AGMAbwBtAHAAbABlAHgAVAB5AHAAZQA+AA0ACgAgACAAIAAgADwALwB4AHMAOgBlAGwAZQBtAGUAbgB0AD4ADQAKACAAIAA8AC8AeABzADoAcwBjAGgAZQBtAGEAPgANAAoAIAAgADwAZABpAGYAZgBnAHIAOgBkAGkAZgBmAGcAcgBhAG0AIAB4AG0AbABuAHMAOgBtAHMAZABhAHQAYQA9ACIAdQByAG4AOgBzAGMAaABlAG0AYQBzAC0AbQBpAGMAcgBvAHMAbwBmAHQALQBjAG8AbQA6AHgAbQBsAC0AbQBzAGQAYQB0AGEAIgAgAHgAbQBsAG4AcwA6AGQAaQBmAGYAZwByAD0AIgB1AHIAbgA6AHMAYwBoAGUAbQBhAHMALQBtAGkAYwByAG8AcwBvAGYAdAAtAGMAbwBtADoAeABtAGwALQBkAGkAZgBmAGcAcgBhAG0ALQB2ADEAIgA+AA0ACgAgACAAIAAgADwARABvAGMAdQBtAGUAbgB0AEUAbABlAG0AZQBuAHQAPgANAAoAIAAgACAAIAAgACAAPABWAEEAUABvAHMAdAAgAGQAaQBmAGYAZwByADoAaQBkAD0AIgBWAEEAUABvAHMAdAAxACIAIABtAHMAZABhAHQAYQA6AHIAbwB3AE8AcgBkAGUAcgA9ACIAMAAiACAAZABpAGYAZgBnAHIAOgBoAGEAcwBDAGgAYQBuAGcAZQBzAD0AIgBpAG4AcwBlAHIAdABlAGQAIgA+AA0ACgAgACAAIAAgACAAIAAgACAAPABwAG8AcwB0AHMAXwBuAGEAbQBlAD4AUABvAHMAdAAxADwALwBwAG8AcwB0AHMAXwBuAGEAbQBlAD4ADQAKACAAIAAgACAAIAAgADwALwBWAEEAUABvAHMAdAA+AA0ACgAgACAAIAAgACAAIAA8AFYAQQBQAG8AcwB0ACAAZABpAGYAZgBnAHIAOgBpAGQAPQAiAFYAQQBQAG8AcwB0ADIAIgAgAG0AcwBkAGEAdABhADoAcgBvAHcATwByAGQAZQByAD0AIgAxACIAIABkAGkAZgBmAGcAcgA6AGgAYQBzAEMAaABhAG4AZwBlAHMAPQAiAGkAbgBzAGUAcgB0AGUAZAAiAD4ADQAKACAAIAAgACAAIAAgACAAIAA8AHAAbwBzAHQAcwBfAG4AYQBtAGUAPgBQAG8AcwB0ADIAPAAvAHAAbwBzAHQAcwBfAG4AYQBtAGUAPgANAAoAIAAgACAAIAAgACAAPAAvAFYAQQBQAG8AcwB0AD4ADQAKACAAIAAgACAAPAAvAEQAbwBjAHUAbQBlAG4AdABFAGwAZQBtAGUAbgB0AD4ADQAKACAAIAA8AC8AZABpAGYAZgBnAHIAOgBkAGkAZgBmAGcAcgBhAG0APgANAAoAPAAvAEQAYQB0AGEAVABhAGIAbABlAD4A";
            string actual = Convert.ToBase64String(Encoding.Unicode.GetBytes(textWriter.ToString()));

            Assert.That(actual, Is.EqualTo(expected));
        }

    }


}

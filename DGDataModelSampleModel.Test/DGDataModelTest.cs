using DG.Data.Model;
using DG.DataModelSample.Model.Entity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DG.DataModelSample.Model.Test
{
    [TestFixture]
    public partial class DGDataModelTest
    {
        DGDataModelSampleModel samplemodel = null;

        public DGDataModelTest()
        {
            Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);

            samplemodel = new DGDataModelSampleModel();

            //load language from file
            samplemodel.LanguageHelper.LoadFromFile(GenericDataModel.GenericDataModelLanguageHelper.DefaultLanguageFilename);

            //test the build of a language file
            samplemodel.LanguageHelper.WriteToFile(GenericDataModel.GenericDataModelLanguageHelper.DefaultLanguageFilename);

            AddTestData();
        }

        public void ClearData()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.AppSettings["dgdatamodeltestConnectionString"];
            sqlConnection.Open();

            new SqlCommand(@"DELETE FROM footertextdesc", sqlConnection).ExecuteNonQuery();
            new SqlCommand(@"DELETE FROM footertext", sqlConnection).ExecuteNonQuery();
            new SqlCommand(@"DELETE FROM poststotags", sqlConnection).ExecuteNonQuery();
            new SqlCommand(@"DELETE FROM tags", sqlConnection).ExecuteNonQuery();
            new SqlCommand(@"DELETE FROM comments", sqlConnection).ExecuteNonQuery();
            new SqlCommand(@"DELETE FROM posts", sqlConnection).ExecuteNonQuery();
            new SqlCommand(@"DELETE FROM blogs", sqlConnection).ExecuteNonQuery();
        }

        public void AddTestData()
        {
            ClearData();

            blogs _blogs = null;

            //add blog
            _blogs = new blogs();
            _blogs.blogs_title = "Blog1";
            _blogs.blogs_description = "test description";
            samplemodel.Blogs.Add(_blogs);
        }

        [Ignore("")]
        [Test]
        public void TimeData()
        {
            ClearData();
            AddTestData();

            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.AppSettings["dgdatamodeltestConnectionString"];
            sqlConnection.Open();

            List<blogs> listtest = new List<blogs>();
            int runnum = 200;

            List<TimeSpan> instanceElapsed = new List<TimeSpan>();
            List<TimeSpan> listElapsed = new List<TimeSpan>();
            List<TimeSpan> modelElapsed = new List<TimeSpan>();
            List<TimeSpan> efElapsed = new List<TimeSpan>();
            List<TimeSpan> datasetElapsed = new List<TimeSpan>();

            Stopwatch instanceStopWatch = new Stopwatch();
            Stopwatch listStopWatch = new Stopwatch();
            Stopwatch modelStopWatch = new Stopwatch();
            Stopwatch efStopWatch = new Stopwatch();
            Stopwatch datasetStopWatch = new Stopwatch();

            for (int i = 0; i < runnum; i++)
            {
                Console.WriteLine("Running CreateInstance time test {0} of {1}.", (i + 1), runnum);

                instanceStopWatch.Reset();
                listStopWatch.Reset();
                modelStopWatch.Reset();
                efStopWatch.Reset();
                datasetStopWatch.Reset();

                instanceStopWatch.Start();
                using (var context = (DbContext)Activator.CreateInstance(samplemodel.ContextType, samplemodel.ContextParameters))
                {
                    instanceStopWatch.Stop();
                    instanceElapsed.Add(instanceStopWatch.Elapsed);
                    listStopWatch.Start();
                    IQueryable<blogs> dbQuery = context.Set<blogs>();
                    listtest = dbQuery.ToList<blogs>();
                    listStopWatch.Stop();
                    listElapsed.Add(listStopWatch.Elapsed);
                }

                efStopWatch.Start();
                using (var context = new dgdatamodeltestEntities())
                {
                    listtest = context.blogs.ToList();
                    //listtest = context.blogs.ToList();
                }
                efStopWatch.Stop();
                efElapsed.Add(efStopWatch.Elapsed);

                modelStopWatch.Start();
                samplemodel.Blogs.List();
                modelStopWatch.Stop();
                modelElapsed.Add(modelStopWatch.Elapsed);

                datasetStopWatch.Start();
                new SqlCommand(@"SELECT * FROM blogs", sqlConnection).ExecuteNonQuery();
                datasetStopWatch.Stop();
                datasetElapsed.Add(datasetStopWatch.Elapsed);
            }
            sqlConnection.Close();

            Console.WriteLine("CreateInstance average time {0}ms.", (new TimeSpan(Convert.ToInt64(instanceElapsed.Average(r => r.Ticks))).TotalMilliseconds));
            Console.WriteLine("CreateInstance list average time {0}ms.", (new TimeSpan(Convert.ToInt64(listElapsed.Average(r => r.Ticks))).TotalMilliseconds));
            Console.WriteLine("Model list average time {0}ms.", (new TimeSpan(Convert.ToInt64(modelElapsed.Average(r => r.Ticks))).TotalMilliseconds));
            Console.WriteLine("EntityFramework list average time {0}ms.", (new TimeSpan(Convert.ToInt64(efElapsed.Average(r => r.Ticks))).TotalMilliseconds));
            Console.WriteLine("DataSet list average time {0}ms.", (new TimeSpan(Convert.ToInt64(datasetElapsed.Average(r => r.Ticks))).TotalMilliseconds));
        }

        [Test]
        public void LanguageTest1()
        {
            Assert.That(samplemodel.Blogs.languageBase.foreingKeyErrorRaised, Is.EqualTo("-Foreing keys constraint raised on: \"{0}\""));
            Assert.That(samplemodel.Posts.languageBase.foreingKeyErrorRaised, Is.EqualTo("-2Foreing keys constraint raised on: \"{0}\""));
            Assert.That(samplemodel.Posts.language.postRepositoryE002, Is.EqualTo("-Post title could not be empty."));
        }

        [Test]
        public void TestBasic()
        {
            AddTestData();

            //add a debug point here and profile using SQL Server profiler to look what's happens under the hood (i.e. looks at the SQL queries)

            posts _posts1 = new posts();
            _posts1.blogs_id = samplemodel.Blogs.FirstOrDefault(a => a.blogs_title == "Blog1").blogs_id;
            _posts1.posts_title = "Post1";
            _posts1.posts_text = "test text 1";
            _posts1.posts_username = "username 010";
            _posts1.posts_email = "username@email.com";
            samplemodel.Posts.Add(_posts1);
            Assert.That(samplemodel.Posts.List().Count, Is.EqualTo(1));

            posts _posts2 = new posts();
            _posts2.blogs_id = samplemodel.Blogs.FirstOrDefault(a => a.blogs_title == "Blog1").blogs_id;
            _posts2.posts_title = "Post2";
            _posts2.posts_text = "test text 2";
            _posts2.posts_username = "username 001";
            _posts2.posts_email = "username@email.com";
            samplemodel.Posts.Add(_posts2);
            Assert.That(samplemodel.Posts.List().Count, Is.EqualTo(2));

            posts _posts3 = new posts();
            _posts3.blogs_id = samplemodel.Blogs.FirstOrDefault(a => a.blogs_title == "Blog1").blogs_id;
            _posts3.posts_title = "Post3";
            _posts3.posts_text = "test text 3";
            _posts3.posts_username = "username 002";
            _posts3.posts_email = "username@email.com";
            samplemodel.Posts.Add(_posts3);
            Assert.That(samplemodel.Posts.List().Count, Is.EqualTo(3));

            samplemodel.Posts.Remove(_posts3);
            Assert.That(samplemodel.Posts.List().Count, Is.EqualTo(2));

            _posts3 = new posts();
            _posts3.blogs_id = samplemodel.Blogs.FirstOrDefault(a => a.blogs_title == "Blog1").blogs_id;
            _posts3.posts_title = "Post3";
            _posts3.posts_text = "test text 3";
            _posts3.posts_username = "username 002";
            _posts3.posts_email = "username@email.com";
            samplemodel.Posts.Add(_posts3);
            Assert.That(samplemodel.Posts.List().Count, Is.EqualTo(3));

            _posts1.posts_title = "Post2";
            samplemodel.Posts.Update(_posts1);
            Assert.That(samplemodel.Posts.List(samplemodel.Posts.OrderBy(r => r.posts_title).ThenByDescending(r => r.posts_username)).FirstOrDefault().posts_username, Is.EqualTo("username 010"));
            Assert.That(samplemodel.Posts.List(samplemodel.Posts.OrderBy(r => r.posts_title).ThenBy(r => r.posts_username)).FirstOrDefault().posts_username, Is.EqualTo("username 001"));
            Assert.That(samplemodel.Posts.List(samplemodel.Posts.OrderByDescending(r => r.posts_title).ThenBy(r => r.posts_username)).FirstOrDefault().posts_username, Is.EqualTo("username 002"));

            Assert.That(samplemodel.Posts.List(r => r.posts_title.StartsWith("Po"), samplemodel.Posts.OrderBy(r => r.posts_title).ThenByDescending(r => r.posts_username)).FirstOrDefault().posts_username, Is.EqualTo("username 010"));
            Assert.That(samplemodel.Posts.List(r => r.posts_title.StartsWith("Po"), samplemodel.Posts.OrderBy(r => r.posts_title).ThenBy(r => r.posts_username)).FirstOrDefault().posts_username, Is.EqualTo("username 001"));
            Assert.That(samplemodel.Posts.List(r => r.posts_title.StartsWith("Po"), samplemodel.Posts.OrderByDescending(r => r.posts_title).ThenBy(r => r.posts_username)).FirstOrDefault().posts_username, Is.EqualTo("username 002"));

            Assert.That(samplemodel.Posts.List(samplemodel.Posts.OrderBy(r => r.posts_title).ThenByDescending(r => r.posts_username), 1).FirstOrDefault().posts_username, Is.EqualTo("username 010"));
            Assert.That(samplemodel.Posts.List(samplemodel.Posts.OrderBy(r => r.posts_title).ThenByDescending(r => r.posts_username), 1, 1).FirstOrDefault().posts_username, Is.EqualTo("username 001"));

            Assert.That(samplemodel.Posts.FirstOrDefault(samplemodel.Posts.OrderBy(r => r.posts_title).ThenByDescending(r => r.posts_username)).posts_username, Is.EqualTo("username 010"));
            Assert.That(samplemodel.Posts.FirstOrDefault(samplemodel.Posts.OrderBy(r => r.posts_title).ThenBy(r => r.posts_username)).posts_username, Is.EqualTo("username 001"));
            Assert.That(samplemodel.Posts.FirstOrDefault(samplemodel.Posts.OrderByDescending(r => r.posts_title).ThenBy(r => r.posts_username)).posts_username, Is.EqualTo("username 002"));

            Assert.That(samplemodel.Posts.FirstOrDefault(r => r.posts_title.StartsWith("Po"), samplemodel.Posts.OrderBy(r => r.posts_title).ThenByDescending(r => r.posts_username)).posts_username, Is.EqualTo("username 010"));

            Assert.That(samplemodel.Posts.FirstOrDefault(r => r.posts_title.StartsWith("Po"), samplemodel.Posts.OrderBy(r => r.posts_title).ThenBy(r => r.posts_username)).posts_username, Is.EqualTo("username 001"));
            Assert.That(samplemodel.Posts.FirstOrDefault(r => r.posts_title.StartsWith("Po"), samplemodel.Posts.OrderByDescending(r => r.posts_title).ThenBy(r => r.posts_username)).posts_username, Is.EqualTo("username 002"));

            _posts1.posts_title = "Post1";
            samplemodel.Posts.Update(_posts1);

            Assert.That(samplemodel.Posts.Count(), Is.EqualTo(3));
            Assert.That(samplemodel.Posts.Count(r => r.posts_title == "Post1"), Is.EqualTo(1));
            Assert.That(samplemodel.Posts.LongCount(), Is.EqualTo(3));
            Assert.That(samplemodel.Posts.LongCount(r => r.posts_title == "Post1"), Is.EqualTo(1));

            Assert.That(samplemodel.Posts.FirstOrDefault(r => r.posts_id == _posts2.posts_id).posts_title, Is.EqualTo("Post2"));
            _posts2.posts_title = "Post4";
            samplemodel.Posts.Update(_posts2);
            Assert.That(samplemodel.Posts.FirstOrDefault(r => r.posts_id == _posts2.posts_id).posts_title, Is.EqualTo("Post4"));
            _posts2.posts_title = "Post2";
            samplemodel.Posts.Update(_posts2);
            Assert.That(samplemodel.Posts.FirstOrDefault(r => r.posts_id == _posts2.posts_id).posts_title, Is.EqualTo("Post2"));

            Assert.That(samplemodel.Posts.Find(_posts2.posts_id).posts_id, Is.EqualTo(_posts2.posts_id));

            Assert.That(samplemodel.Posts.Any(r => r.posts_id == _posts2.posts_id), Is.EqualTo(true));
            Assert.That(samplemodel.Posts.Any(r => r.posts_id == -1), Is.EqualTo(false));

            Assert.That((int)samplemodel.Posts.Sum(r => r.posts_title == "Post1", r => r.posts_id), Is.EqualTo(_posts1.posts_id));
            Assert.That((int)samplemodel.Posts.Sum(r => r.posts_id), Is.EqualTo(_posts1.posts_id + _posts2.posts_id + _posts3.posts_id));
            Assert.That(samplemodel.Posts.Sum(r => r.posts_title == "Post1", r => r.posts_id), Is.EqualTo(_posts1.posts_id));
            Assert.That(samplemodel.Posts.Sum(r => r.posts_id), Is.EqualTo(_posts1.posts_id + _posts2.posts_id + _posts3.posts_id));

            Assert.That((double)samplemodel.Posts.Average(r => r.posts_title == "Post1", r => r.posts_id), Is.EqualTo(_posts1.posts_id));
            Assert.That((double)samplemodel.Posts.Average(r => (double)r.posts_id), Is.EqualTo(((double)_posts1.posts_id + (double)_posts2.posts_id + (double)_posts3.posts_id) / 3));
            Assert.That(samplemodel.Posts.Average(r => r.posts_title == "Post1", r => r.posts_id), Is.EqualTo(_posts1.posts_id));
            Assert.That(samplemodel.Posts.Average(r => (double)r.posts_id), Is.EqualTo(((double)_posts1.posts_id + (double)_posts2.posts_id + (double)_posts3.posts_id) / 3));

            Assert.That((int)samplemodel.Posts.Min(r => r.posts_title == "Post1", r => r.posts_id), Is.EqualTo(_posts1.posts_id));
            Assert.That((int)samplemodel.Posts.Min(r => r.posts_id), Is.EqualTo(_posts1.posts_id));
            Assert.That(samplemodel.Posts.Min(r => r.posts_title == "Post1", r => r.posts_id), Is.EqualTo(_posts1.posts_id));
            Assert.That(samplemodel.Posts.Min(r => r.posts_id), Is.EqualTo(_posts1.posts_id));

            Assert.That((int)samplemodel.Posts.Max(r => r.posts_title == "Post1", r => r.posts_id), Is.EqualTo(_posts1.posts_id));
            Assert.That((int)samplemodel.Posts.Max(r => r.posts_id), Is.EqualTo(_posts3.posts_id));
            Assert.That(samplemodel.Posts.Max(r => r.posts_title == "Post1", r => r.posts_id), Is.EqualTo(_posts1.posts_id));
            Assert.That(samplemodel.Posts.Max(r => r.posts_id), Is.EqualTo(_posts3.posts_id));
        }

        [Test]
        public void Test1()
        {
            AddTestData();

            posts _posts = null;
            comments _comments = null;
            tags _tags = null;
            poststotags _poststotags = null;

            //add post
            _posts = new posts();
            Assert.IsFalse(samplemodel.Posts.CanAdd(_posts));

            _posts.blogs_id = samplemodel.Blogs.FirstOrDefault(a => a.blogs_title == "Blog1").blogs_id;
            _posts.posts_title = "Post1";
            _posts.posts_text = "test text 1";
            _posts.posts_username = "username";
            _posts.posts_email = "username@email.com";

            _posts.posts_title = "Post1<"; //invalid title
            Assert.IsFalse(samplemodel.Posts.CanAdd(_posts));

            _posts.posts_title = "Post1";
            Assert.IsTrue(samplemodel.Posts.CanAdd(_posts));
            samplemodel.Posts.Add(_posts);

            Assert.That(samplemodel.Posts.Count(), Is.EqualTo(1));

            //edit post1
            _posts = samplemodel.Posts.FirstOrDefault(a => a.posts_title == "Post1");
            _posts.blogs_id = samplemodel.Blogs.FirstOrDefault(a => a.blogs_title == "Blog1").blogs_id;
            _posts.posts_title = "Post1";
            _posts.posts_text = "test text 2";
            _posts.posts_username = "username";
            _posts.posts_email = "username@email.com";
            samplemodel.Posts.Update(_posts);

            Assert.That(samplemodel.Posts.FirstOrDefault(a => a.posts_title == "Post1").posts_text, Is.EqualTo("test text 2"));

            Assert.That(samplemodel.Posts.Find(samplemodel.Posts.FirstOrDefault(a => a.posts_title == "Post1").posts_id).posts_id, Is.EqualTo(samplemodel.Posts.FirstOrDefault(a => a.posts_title == "Post1").posts_id));

            //edit post1
            _posts = samplemodel.Posts.FirstOrDefault(a => a.posts_title == "Post1");
            _posts.blogs_id = samplemodel.Blogs.FirstOrDefault(a => a.blogs_title == "Blog1").blogs_id;
            _posts.posts_title = "Post1";
            _posts.posts_text = "test text 3";
            _posts.posts_username = "username";
            _posts.posts_email = "username@email.com";

            Assert.That(samplemodel.Posts.FirstOrDefault(a => a.posts_title == "Post1").posts_text, Is.Not.EqualTo("test text 3"));

            samplemodel.Posts.Update(_posts);

            Assert.That(samplemodel.Posts.FirstOrDefault(a => a.posts_title == "Post1").posts_text, Is.EqualTo("test text 3"));

            //add tags
            _tags = new tags();
            _tags.tags_name = "Tag1";
            samplemodel.Tags.Add(_tags);

            _tags = new tags();
            _tags.tags_name = "Tag2";
            samplemodel.Tags.Add(_tags);

            Assert.That(samplemodel.Tags.Count(), Is.EqualTo(2));

            //add poststotags
            _poststotags = new poststotags();
            _poststotags.posts_id = samplemodel.Posts.FirstOrDefault(a => a.posts_title == "Post1").posts_id;
            _poststotags.tags_id = samplemodel.Tags.FirstOrDefault(a => a.tags_name == "Tag1").tags_id;
            samplemodel.PostsToTags.Add(_poststotags);

            _poststotags = new poststotags();
            _poststotags.posts_id = samplemodel.Posts.FirstOrDefault(a => a.posts_title == "Post1").posts_id;
            _poststotags.tags_id = samplemodel.Tags.FirstOrDefault(a => a.tags_name == "Tag2").tags_id;
            samplemodel.PostsToTags.Add(_poststotags);

            //add comments
            _comments = new comments();
            _comments.posts_id = samplemodel.Posts.FirstOrDefault(a => a.posts_title == "Post1").posts_id;
            _comments.comments_text = "test text 1";
            _comments.comments_username = "username";
            _comments.comments_email = "username@email.com";
            samplemodel.Comments.Add(_comments);
        }

        [Test]
        public void TestPostsRemove1()
        {
            AddTestData();

            posts _posts = null;
            comments _comments = null;

            //add post
            _posts = new posts();
            _posts.blogs_id = samplemodel.Blogs.FirstOrDefault(a => a.blogs_title == "Blog1").blogs_id;
            _posts.posts_title = "Post3";
            _posts.posts_text = "test text 1";
            _posts.posts_username = "username";
            _posts.posts_email = "username@email.com";
            samplemodel.Posts.Add(_posts);

            //add comments
            _comments = new comments();
            _comments.posts_id = _posts.posts_id;
            _comments.comments_text = "test text 1";
            _comments.comments_username = "username";
            _comments.comments_email = "username@email.com";
            samplemodel.Comments.Add(_comments);

            Assert.IsFalse(samplemodel.Posts.CanRemove(_posts));
            string[] errors = new string[] { };
            Assert.IsTrue(samplemodel.Posts.CanRemove(false, ref errors, _posts));
            Assert.IsTrue(samplemodel.Posts.CanRemove(true, new string[] { "FK_comments_posts" }, ref errors, _posts));
            Assert.IsFalse(samplemodel.Posts.CanRemove(true, new string[] { "invalidnameFK_comments_posts" }, ref errors, _posts));

            samplemodel.Comments.Remove(_comments);
            Assert.IsTrue(samplemodel.Posts.CanRemove(_posts));

            _posts = samplemodel.Posts.Find(_posts.posts_id);
            samplemodel.Posts.Remove(_posts);
        }

        [Test]
        public void TestTagsRemove1()
        {
            AddTestData();

            footertext _footertext = null;
            footertextdesc _footertextdesc = null;

            _footertext = new footertext();
            _footertext.footertext_title = "FooterText1";
            samplemodel.FooterText.Add(_footertext);

            Assert.IsTrue(samplemodel.FooterText.CanRemove(_footertext));

            _footertextdesc = new footertextdesc();
            _footertextdesc.footertext_id = _footertext.footertext_id;
            _footertextdesc.footertext_desc = "text";
            samplemodel.FooterTextDesc.Add(_footertextdesc);

            string[] errors = new string[] { };
            Assert.IsTrue(samplemodel.FooterText.CanRemove(false, ref errors, _footertext));
            Assert.IsFalse(samplemodel.FooterText.CanRemove(_footertext));

            samplemodel.FooterTextDesc.Remove(_footertextdesc);
            Assert.IsTrue(samplemodel.FooterText.CanRemove(_footertext));

            _footertext = samplemodel.FooterText.Find(_footertext.footertext_id);
            samplemodel.FooterText.Remove(_footertext);
        }

        [Test]
        public void TestExceptionFree1()
        {
            AddTestData();

            string[] errors = null;
            string exceptionTrace = null;

            posts _posts = null;
            tags _tags = null;
            poststotags _poststotags = null;

            //add post
            _posts = new posts();
            _posts.posts_title = "Post1";
            _posts.posts_text = "test text 1";
            _posts.posts_username = "username";
            _posts.posts_email = "username@email.com";

            Assert.IsFalse(samplemodel.Posts.Add(ref errors, ref exceptionTrace, _posts));
            Assert.That(errors.Length, Is.EqualTo(0));
            Assert.IsTrue(!String.IsNullOrEmpty(exceptionTrace));

            _posts.blogs_id = samplemodel.Blogs.FirstOrDefault(a => a.blogs_title == "Blog1").blogs_id;
            Assert.IsTrue(samplemodel.Posts.Add(ref errors, ref exceptionTrace, _posts));
            Assert.That(errors.Length, Is.EqualTo(0));
            Assert.IsTrue(String.IsNullOrEmpty(exceptionTrace));

            //add tags
            _tags = new tags();
            _tags.tags_name = "Tag1";
            samplemodel.Tags.Add(_tags);

            _tags = new tags();
            _tags.tags_name = "Tag2";
            samplemodel.Tags.Add(_tags);

            //edit post1
            _posts = samplemodel.Posts.FirstOrDefault(a => a.posts_title == "Post1");
            _posts.blogs_id = -1;
            Assert.IsFalse(samplemodel.Posts.Update(ref errors, ref exceptionTrace, _posts));
            Assert.That(errors.Length, Is.EqualTo(0));
            Assert.IsTrue(!String.IsNullOrEmpty(exceptionTrace));

            _posts.blogs_id = samplemodel.Blogs.FirstOrDefault(a => a.blogs_title == "Blog1").blogs_id;
            Assert.IsTrue(samplemodel.Posts.Update(ref errors, ref exceptionTrace, _posts));
            Assert.That(errors.Length, Is.EqualTo(0));
            Assert.IsTrue(String.IsNullOrEmpty(exceptionTrace));

            //add poststotags
            _poststotags = new poststotags();
            _poststotags.posts_id = samplemodel.Posts.FirstOrDefault(a => a.posts_title == "Post1").posts_id;
            _poststotags.poststotags_comments = "123x";
            Assert.IsFalse(samplemodel.PostsToTags.Add(ref errors, ref exceptionTrace, _poststotags));
            Assert.That(errors.Length, Is.AtLeast(1));
            Assert.IsTrue(String.IsNullOrEmpty(exceptionTrace));

            _poststotags.tags_id = samplemodel.Tags.FirstOrDefault(a => a.tags_name == "Tag1").tags_id;
            Assert.IsTrue(samplemodel.PostsToTags.Add(ref errors, ref exceptionTrace, _poststotags));
            Assert.That(errors.Length, Is.EqualTo(0));
            Assert.IsTrue(String.IsNullOrEmpty(exceptionTrace));

            //edit poststotags
            _poststotags = samplemodel.PostsToTags.FirstOrDefault(a => a.poststotags_comments == "123x");
            _poststotags.tags_id = -1;
            Assert.IsFalse(samplemodel.PostsToTags.Update(ref errors, ref exceptionTrace, _poststotags));
            Assert.That(errors.Length, Is.AtLeast(1));
            Assert.IsTrue(String.IsNullOrEmpty(exceptionTrace));

            _poststotags.tags_id = samplemodel.Tags.FirstOrDefault(a => a.tags_name == "Tag1").tags_id;
            Assert.IsTrue(samplemodel.PostsToTags.Update(ref errors, ref exceptionTrace, _poststotags));
            Assert.That(errors.Length, Is.EqualTo(0));
            Assert.IsTrue(String.IsNullOrEmpty(exceptionTrace));
        }

        [Test]
        public void TestHelperConcurrency1()
        {
            AddTestData();

            posts _posts = null;
            SqlConnection sqlConnection = null;

            //add post
            _posts = new posts();
            _posts.blogs_id = samplemodel.Blogs.FirstOrDefault(a => a.blogs_title == "Blog1").blogs_id;
            _posts.posts_title = "Post4";
            _posts.posts_text = "test text 1";
            _posts.posts_username = "username";
            _posts.posts_email = "username@email.com";
            samplemodel.Posts.Add(_posts);

            //get post
            _posts = samplemodel.Posts.Find(_posts.posts_id);
            IDictionary<string, object> _postsoriginalvalues = samplemodel.Posts.Helper.GetPropertyValues(_posts);
            Assert.IsFalse(samplemodel.Posts.Helper.ArePropertyValuesChanged(_posts, _postsoriginalvalues));

            _posts.posts_text = "test text 3";
            Assert.IsFalse(samplemodel.Posts.Helper.ArePropertyValuesChanged(_posts, _postsoriginalvalues));

            //update post by third part
            sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.AppSettings["dgdatamodeltestConnectionString"];
            sqlConnection.Open();
            new SqlCommand(@"UPDATE posts SET posts_text = 'test text 2' WHERE posts_id = " + _posts.posts_id, sqlConnection).ExecuteNonQuery();
            sqlConnection.Close();
            Assert.IsTrue(samplemodel.Posts.Helper.ArePropertyValuesChanged(_posts, _postsoriginalvalues));

            _posts.posts_text = "test text 2";
            Assert.IsTrue(samplemodel.Posts.Helper.ArePropertyValuesChanged(_posts, _postsoriginalvalues));

            //update post by third part (take it back to original values)
            sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.AppSettings["dgdatamodeltestConnectionString"];
            sqlConnection.Open();
            new SqlCommand(@"UPDATE posts SET posts_text = 'test text 1' WHERE posts_id = " + _posts.posts_id, sqlConnection).ExecuteNonQuery();
            sqlConnection.Close();
            Assert.IsFalse(samplemodel.Posts.Helper.ArePropertyValuesChanged(_posts, _postsoriginalvalues));
        }

        [Test]
        public void TestHelperGetDatabaseName1()
        {
            string expected = "tst_dgdatamodeltest";
            string actual = samplemodel.Blogs.Helper.GetDatabaseName();

            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void TestHelperGetTableName1()
        {
            string expected = "blogs";
            string actual = samplemodel.Blogs.Helper.GetTableName();

            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void TestHelperGetEntityEntryKeyPairs1()
        {
            blogs _blogs = samplemodel.Blogs.FirstOrDefault(a => a.blogs_title == "Blog1");

            IDictionary<string, object> expected = new Dictionary<string, object>()
            {
                {"blogs_id", _blogs.blogs_id}
            };
            IDictionary<string, object> actual = samplemodel.Blogs.Helper.GetKeyPairs(_blogs);

            Assert.That(expected, Is.EqualTo(actual));
        }


    }


}

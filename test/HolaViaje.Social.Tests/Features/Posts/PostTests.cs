using HolaViaje.Social.Features.Posts;
using HolaViaje.Social.Shared;

namespace HolaViaje.Social.Tests.Features.Posts
{
    [TestClass]
    public class PostTests
    {
        [TestMethod]
        public void Constructor_ShouldInitializeControl()
        {
            var post = new Post();
            Assert.IsNotNull(post.Control);
            Assert.AreEqual(DateTime.UtcNow.Date, post.Control.CreatedAt.Date);
            Assert.AreEqual(DateTime.UtcNow.Date, post.Control.LastModifiedAt.Date);
        }

        [TestMethod]
        public void Constructor_WithParameters_ShouldSetUserIdAndPageId()
        {
            var post = new Post(1, 2);
            Assert.AreEqual(1, post.UserId);
            Assert.AreEqual(2, post.PageId);
        }

        [TestMethod]
        public void AllowedFileQuantity_ShouldReturnCorrectValue()
        {
            var post = new Post { Type = PostType.Short };
            Assert.AreEqual(1, post.AllowedFileQuantity);

            post.Type = PostType.Live;
            Assert.AreEqual(1, post.AllowedFileQuantity);

            post.Type = PostType.Publication;
            Assert.AreEqual(6, post.AllowedFileQuantity);
        }

        [TestMethod]
        public void UpdateLastModified_ShouldUpdateLastModifiedAt()
        {
            var post = new Post();
            var initialDate = post.Control.LastModifiedAt;
            post.UpdateLastModified();
            Assert.IsTrue(post.Control.LastModifiedAt > initialDate);
        }

        [TestMethod]
        public void Delete_ShouldSetIsDeleted()
        {
            var post = new Post();
            post.Delete();
            Assert.IsTrue(post.IsDeleted());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EnsureNotDeleted_ShouldThrowIfDeleted()
        {
            var post = new Post();
            post.Delete();
            post.UpdateLastModified();
        }

        [TestMethod]
        public void SetPlace_ShouldUpdatePlace()
        {
            var post = new Post();
            var newPlace = new PlaceInfo { Country = "Country" };
            post.SetPlace(newPlace);
            Assert.AreEqual(newPlace, post.Place);
        }

        [TestMethod]
        public void SetMembers_ShouldUpdateMembers()
        {
            var post = new Post();
            var members = new List<PostMember> { new PostMember(1) };
            post.SetMembers(members);
            Assert.AreEqual(1, post.Members.Count);
        }

        [TestMethod]
        public void IsVisibleFor_ShouldReturnCorrectValue()
        {
            var post = new Post { UserId = 1, Visibility = Visibility.Public };
            Assert.IsTrue(post.IsVisibleFor(1));
            Assert.IsTrue(post.IsVisibleFor(2));

            post.Visibility = Visibility.Custom;
            post.SetMembers([new PostMember(2)]);
            Assert.IsTrue(post.IsVisibleFor(2));
            Assert.IsFalse(post.IsVisibleFor(3));
        }

        [TestMethod]
        public void IsOwner_ShouldReturnTrueIfOwner()
        {
            var post = new Post { UserId = 1 };
            Assert.IsTrue(post.IsOwner(1));
            Assert.IsFalse(post.IsOwner(2));
        }

        [TestMethod]
        public void IsAssociatedWithPage_ShouldReturnTrueIfAssociated()
        {
            var post = new Post { PageId = 1 };
            Assert.IsTrue(post.IsAssociatedWithPage());
            post.PageId = 0;
            Assert.IsFalse(post.IsAssociatedWithPage());
        }

        [TestMethod]
        public void CanAddMediaFile_ShouldReturnTrueIfCanAdd()
        {
            var post = new Post { Type = PostType.Short };
            Assert.IsTrue(post.CanAddMediaFile());
            post.MediaFiles.Add(new MediaFile("1", "file", 1, "type", "container", "url"));
            Assert.IsFalse(post.CanAddMediaFile());
        }

        [TestMethod]
        public void AddMediaFile_ShouldAddFile()
        {
            var post = new Post();
            var mediaFile = new MediaFile("1", "file", 1, "type", "container", "url");
            post.AddMediaFile(mediaFile);
            Assert.AreEqual(1, post.MediaFiles.Count);
        }

        [TestMethod]
        public void RemoveMediaFile_ShouldRemoveFile()
        {
            var post = new Post();
            var mediaFile = new MediaFile("1", "file", 1, "type", "container", "url");
            post.AddMediaFile(mediaFile);
            post.RemoveMediaFile(mediaFile);
            Assert.AreEqual(0, post.MediaFiles.Count);
        }

        [TestMethod]
        public void GetMediaFile_ShouldReturnCorrectFile()
        {
            var post = new Post();
            var mediaFile = new MediaFile("1", "file", 1, "type", "container", "url");
            post.AddMediaFile(mediaFile);
            var result = post.GetMediaFile("1");
            Assert.AreEqual(mediaFile, result);
        }

        [TestMethod]
        public void Publish_ShouldUpdateStatus()
        {
            var post = new Post { IsDraft = true, EditMode = true, Status = PostStatus.Unpublished };
            post.Publish();
            Assert.IsFalse(post.IsDraft);
            Assert.IsFalse(post.EditMode);
            Assert.AreEqual(PostStatus.Published, post.Status);
        }

        [TestMethod]
        public void UploadsPending_ShouldReturnTrueIfPending()
        {
            var post = new Post();
            var mediaFile = new MediaFile("1", "file", 1, "type", "container", "url") { Uploaded = false };
            post.AddMediaFile(mediaFile);
            Assert.IsTrue(post.UploadsPending());
            mediaFile.Uploaded = true;
            Assert.IsFalse(post.UploadsPending());
        }
    }
}

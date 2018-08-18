namespace Forum.App.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Data;
	using DataModels;
	using ViewModels;
	using Contracts;

	public class PostService : IPostService
	{
		private ForumData forumData;
		private IUserService userService;

		public PostService(ForumData forumData, IUserService userService)
		{
			this.forumData = forumData;
			this.userService = userService;
		}

		public string GetCategoryName(int categoryId)
		{
			string categoryName = this.forumData.Categories.Find(c => c.Id == categoryId)?.Name;

			if (categoryName == null)
			{
				throw new ArgumentException($"Category with id {categoryId} not found!");
			}

			return categoryName;
		}

		public IPostViewModel GetPostViewModel(int postId)
		{
			var post = this.forumData.Posts.FirstOrDefault(p => p.Id == postId);
			IPostViewModel postView = new PostViewModel(post.Title, 
				this.userService.GetUserName(post.AuthorId), post.Content, this.GetPostReplies(postId));

			return postView;
		}

		private IEnumerable<IReplyViewModel> GetPostReplies(int postId)
		{
			IEnumerable<IReplyViewModel> replies = this.forumData.Replies
				.Where(r => r.PostId == postId)
				.Select(r => new ReplyViewModel(this.userService.GetUserName(r.AuthorId), r.Content));

			return replies;
		}

		public int AddPost(int userId, string postTitle, string postCategory, string postContent)
		{
			bool emptyCategory = string.IsNullOrWhiteSpace(postCategory);
			bool emptyTitle = string.IsNullOrWhiteSpace(postTitle);
			bool emptyContent = string.IsNullOrWhiteSpace(postContent);

			if (emptyCategory || emptyContent || emptyTitle)
			{
				throw new ArgumentException("All fields must be filled!");
			}

			Category category = this.EnsureCategory(postCategory);

			int postId = forumData.Posts.Any() ? forumData.Posts.Last().Id + 1 : 1;

			User author = this.userService.GetUserById(userId);

			Post post = new Post(postId, postTitle, postContent, category.Id, userId, new List<int>());

			forumData.Posts.Add(post);
			author.Posts.Add(post.Id);
			category.Posts.Add(post.Id);
			forumData.SaveChanges();

			return post.Id;
		}

		private Category EnsureCategory(string categoryName)
		{
			Category category = this.forumData.Categories.FirstOrDefault(x => x.Name == categoryName);
			if (category == null)
			{
				var categories = forumData.Categories;
				int categoryId = categories.Any() ? categories.Last().Id + 1 : 1;
				category = new Category(categoryId, categoryName, new List<int>());
				forumData.Categories.Add(category);
			}

			return category;
		}

		public IEnumerable<ICategoryInfoViewModel> GetAllCategories()
		{
			IEnumerable<ICategoryInfoViewModel> categories = this.forumData
				.Categories.Select(c => new CategoryInfoViewModel(c.Id, c.Name, c.Posts.Count));

			return categories;
		}

		public IEnumerable<IPostInfoViewModel> GetCategoryPostsInfo(int categoryId)
		{
			IEnumerable<IPostInfoViewModel> posts = this.forumData.Posts
				.Where(p => p.CategoryId == categoryId)
				.Select(p => new PostInfoViewModel(p.Id, p.Title, p.Replies.Count));

			return posts;
		}

		public void AddReplyToPost(int postId, string replyContent, int userId)
		{
			bool emptyReply = string.IsNullOrWhiteSpace(replyContent);

			if (emptyReply)
			{
				throw new ArgumentException("Cannot add an empty reply!");
			}

			int replyId = forumData.Replies.Any() ? forumData.Replies.Last().Id + 1 : 1;

			Reply reply = new Reply(replyId, replyContent, userId, postId);

			Post post = forumData.Posts.FirstOrDefault(p => p.Id == postId);

			forumData.Replies.Add(reply);
			post.Replies.Add(replyId);
			forumData.SaveChanges();
		}
	}
}

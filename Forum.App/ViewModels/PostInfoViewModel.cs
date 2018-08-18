namespace Forum.App.ViewModels
{
	using Contracts;

    public class PostInfoViewModel : IPostInfoViewModel
    {
		public PostInfoViewModel(int id, string name, int repliesCount)
		{
			this.Id = id;
			this.Title = name;
			this.ReplyCount = repliesCount;
		}

		public int Id { get; }

		public string Title { get; }

		public int ReplyCount { get; }
	}
}

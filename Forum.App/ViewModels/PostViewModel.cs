namespace Forum.App.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

	using Contracts;

    public class PostViewModel : ContentViewModel, IPostViewModel
    {
        private const int LINE_LENGHT = 37;

        public PostViewModel(string title, string author, string content, IEnumerable<IReplyViewModel> replies)
			: base(content)
        {
            this.Title = title;
			this.Author = author;
            this.Replies = replies.ToArray();
        }

		public string Title { get; }

        public string Author { get; }

        public IReplyViewModel[] Replies { get; }
    }
}

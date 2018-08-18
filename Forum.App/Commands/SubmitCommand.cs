namespace Forum.App.Commands
{
	using Contracts;

	public class SubmitCommand : ICommand
	{
		private IPostService postService;
		private ISession session;

		public SubmitCommand(IPostService postService, ISession session)
		{
			this.postService = postService;
			this.session = session;
		}

		public IMenu Execute(params string[] args)
		{
			int postId = int.Parse(args[0]);
			string replyContents = args[1];

			this.postService.AddReplyToPost(postId, replyContents, this.session.UserId);

			return this.session.Back();
		}
	}
}

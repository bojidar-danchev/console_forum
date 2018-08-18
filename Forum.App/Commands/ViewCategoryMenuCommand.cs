namespace Forum.App.Commands
{
	using Contracts;

	public class ViewCategoryMenuCommand : ICommand
	{
		private IMenuFactory menuFactory;
		private IPostService postService;

		public ViewCategoryMenuCommand(IMenuFactory menuFactory, IPostService postService)
		{
			this.menuFactory = menuFactory;
			this.postService = postService;
		}

		public IMenu Execute(params string[] args)
		{
			int categoryId = int.Parse(args[0]);

			string commandName = this.GetType().Name;
			string menuName = commandName.Substring(0, commandName.Length - "Command".Length);

			IIdHoldingMenu menu = (IIdHoldingMenu)this.menuFactory.CreateMenu(menuName);
			menu.SetId(categoryId);

			return menu;
		}
	}
}

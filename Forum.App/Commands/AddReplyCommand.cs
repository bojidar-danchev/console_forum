namespace Forum.App.Commands
{
	using Contracts;

	public class AddReplyCommand : ICommand
	{
		private IMenuFactory menuFactory;

		public AddReplyCommand(IMenuFactory menuFactory)
		{
			this.menuFactory = menuFactory;
		}

		public IMenu Execute(params string[] args)
		{
			string commandName = this.GetType().Name;
			string menuName = commandName.Substring(0, commandName.Length - "Command".Length) + "Menu";

			int postId = int.Parse(args[0]);
			IIdHoldingMenu menu = (IIdHoldingMenu)this.menuFactory.CreateMenu(menuName);
			menu.SetId(postId);
			return menu;
		}
	}
}

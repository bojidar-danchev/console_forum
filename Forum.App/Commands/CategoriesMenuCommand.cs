﻿namespace Forum.App.Commands
{
	using Contracts;

	public class CategoriesMenuCommand : ICommand
	{
		private IMenuFactory viewFactory;

		public CategoriesMenuCommand(IMenuFactory viewFactory)
		{
			this.viewFactory = viewFactory;
		}

		public IMenu Execute(params string[] args)
		{
			string commandName = this.GetType().Name;
			string viewName = commandName.Substring(0, commandName.Length - "Command".Length);

			IMenu view = this.viewFactory.CreateMenu(viewName);
			return view;
		}
	}
}

namespace Forum.App.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Contracts;
	using DataModels;

	public class Session : ISession
	{
		private User user;
		private Stack<IMenu> history;

		public Session()
		{
			this.history = new Stack<IMenu>();
		}

		public string Username => this.user?.Username;

		public int UserId => this.user?.Id ?? 0;

		public bool IsLoggedIn => this.user != null;

		public IMenu CurrentMenu => this.history.Peek();

		public IMenu Back()
		{
			if (this.history.Count > 1)
			{
				this.history.Pop();
			}

			IMenu previousMenu = this.history.Peek();
			previousMenu.Open();

			return previousMenu;
		}

		public bool PushView(IMenu view)
		{
			if (!history.Any() || view != history.Peek())
			{
				history.Push(view);
				return true;
			}

			return false;
		}

		public void LogIn(User user)
		{
			this.user = user;
		}

		public void LogOut()
		{
			this.user = null;
		}

		//Hard resets the session. To be used with caution.
		public void Reset()
		{
			this.history = new Stack<IMenu>();
			this.user = null;
		}
	}
}

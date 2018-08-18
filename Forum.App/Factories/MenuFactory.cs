namespace Forum.App.Factories
{
	using System;
	using System.Linq;
	using System.Reflection;

	using Contracts;

	public class MenuFactory : IMenuFactory
	{
		private IServiceProvider serviceProvider;

		public MenuFactory(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public IMenu CreateMenu(string menuName)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			Type menuType = assembly.GetTypes()
				.FirstOrDefault(t => t.Name == menuName);

			if (menuType == null)
			{
				throw new InvalidOperationException("Menu not found!");
			}

			if (!typeof(IMenu).IsAssignableFrom(menuType))
			{
				throw new InvalidOperationException($"{menuType} is not a menu!");
			}

			ParameterInfo[] @params = menuType.GetConstructors().First().GetParameters();
			object[] args = new object[@params.Length];

			for (int i = 0; i < @params.Length; i++)
			{
				args[i] = this.serviceProvider.GetService(@params[i].ParameterType);
			}

			IMenu menuInstance = (IMenu)Activator.CreateInstance(menuType, args);

			return menuInstance;
		}
	}
}

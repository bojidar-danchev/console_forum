namespace Forum.App.Factories
{
	using System;
	using System.Linq;
	using System.Reflection;

	using Contracts;

	public class CommandFactory : ICommandFactory
	{
		private IServiceProvider serviceProvider;

		public CommandFactory(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public ICommand CreateCommand(string commandName)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			Type commandType = assembly.GetTypes()
				.FirstOrDefault(t => t.Name == commandName + "Command");

			if (commandType == null)
			{
				throw new InvalidOperationException("Command not found!");
			}

			if (!typeof(ICommand).IsAssignableFrom(commandType))
			{
				throw new InvalidOperationException($"{commandType} is not a command!");
			}

			ParameterInfo[] @params = commandType.GetConstructors().First().GetParameters();
			object[] args = new object[@params.Length];

			for (int i = 0; i < @params.Length; i++)
			{
				args[i] = this.serviceProvider.GetService(@params[i].ParameterType);
			}

			ICommand commandInstance = (ICommand)Activator.CreateInstance(commandType, args);

			return commandInstance;
		}
	}
}

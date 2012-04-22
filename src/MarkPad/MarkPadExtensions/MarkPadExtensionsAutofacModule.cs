using Autofac;

namespace MarkPad.MarkPadExtensions
{
	public class MarkPadExtensionsAutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder
				.RegisterType<SpellCheck.SpellCheckExtension>()
				.As<SpellCheck.SpellCheckExtension>();
		}
	}
}

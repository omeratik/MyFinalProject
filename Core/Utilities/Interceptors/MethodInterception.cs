using Castle.DynamicProxy;

namespace Core.Utilities.Interceptors
{
	//Burası bütün metodların çatısıdır. Metodlar ilk baş buralardan geçecek.
	//invocation  = business methodlarıdır.
	//Business de sürekli olarak try-catch yazmak yerine temel bir try-catch yazarak bütün metodlarda ortak kullanıyoruz.
	public abstract class MethodInterception : MethodInterceptionBaseAttribute
	{
		protected virtual void OnBefore(IInvocation invocation) { }
		protected virtual void OnAfter(IInvocation invocation) { }
		protected virtual void OnException(IInvocation invocation, System.Exception e) { }
		protected virtual void OnSuccess(IInvocation invocation) { }



		
		public override void Intercept(IInvocation invocation)
		{
			var isSuccess = true;
			OnBefore(invocation); //metodun başında çalışır
			try
			{
				invocation.Proceed();
			}
			catch (Exception e) //hata alma durumunda calışır
			{
				isSuccess = false;
				OnException(invocation, e);
				throw;
			}
			finally 
			{
				if (isSuccess)
				{
					OnSuccess(invocation); //son durumda çalışsın
				}
			}
			OnAfter(invocation); //en sonda çalışsın 
		}
	}
}

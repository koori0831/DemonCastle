using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Work.Utils.Helpers
{
    public class HelperManager
    {
        private readonly List<IHelper> helpers = new();

        private Dictionary<Type, IHelper> helpersDictionary = new Dictionary<Type, IHelper>();

        public T GetHelper<T>() where T : IHelper
        {
            return (T)helpersDictionary.GetValueOrDefault(typeof(T));
        }

        public void Initialize()
        {
            //여기 들어가는건 IHelper를 구현중인 모든 클래스이다.
            IEnumerable<Type> helperTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    typeof(IHelper).IsAssignableFrom(t) && //t가 IHelper를 상속받고 있는지
                    !t.IsInterface && //인터페이스인지
                    !t.IsAbstract //추상클래스인지 
                ).Select(t => new 
                {
                    Type = t,
                    Order = t.GetCustomAttribute<HelperOrderAttribute>()?.Order ?? 0
                }) //익명 객체를 만들어서 해당 타입을 넣어주고 이 타입(Class)에 HelperOrderAttribute가 붙어 있으면 가져오고없으면 null
                .OrderBy(x => x.Order) //Order에 따라서 정렬
                .Select(x => x.Type); //다시 Type(helper class)만 뽑는다.


            foreach (Type type in helperTypes)
            {
                IHelper helper = (IHelper)Activator.CreateInstance(type);
                helper.Initialize();
                helpers.Add(helper);
                helpersDictionary.Add(type, helper);
            }
        }

        public void Dispose()
        {
            for (int i = helpers.Count - 1; i >= 0; i--)
            {
                helpers[i].Dispose();
            }
            helpers.Clear();
        }
    }
}
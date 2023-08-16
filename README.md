# Excel2CSharp
较为新颖的直接生成数据代码的C#配表工具！！！
# 优势
1. 很容易解析，new一个就OK，几乎没什么学习成本，配表 ->生成->new()
2. 你不会不相信CLR的对象分配速度吧
3. 有很完善的主键冲突分析，类型分析
4. 能自己拓展超级多的类型，比如原生的Func(不比解释器的公式强？)，Vector，struct
5. 热更？ 用HybridCLR或者ILRuntime把生成的代码当作一个数据dll！


# Excel
![image](https://github.com/yueh0607/Excel2CSharp/assets/102401735/0513ee54-7920-4815-bf96-117b2530c181)
# 转为C#
```csharp
using System;
using System.Collections;
using System.Collections.Generic;
namespace AirFramework.ConstModel
{
    public class Testdata
    {
        [AirFramework.PrimaryKey]
        public System.Int32 id = default;
        public System.Int32 value = default;
    }

    public class TestdataModel : AirFramework.IModel
    {
        public System.Collections.Generic.List<Testdata> Data = new System.Collections.Generic.List<Testdata>()
        {
new Testdata(){id = 3,value = 336}
,new Testdata(){id = 4,value = 337}
,new Testdata(){id = 5,value = 338}
,new Testdata(){id = 6,value = 339}
,new Testdata(){id = 7,value = 340}
,new Testdata(){id = 8,value = 341}
,new Testdata(){id = 9,value = 342}
,new Testdata(){id = 10,value = 343}
,new Testdata(){id = 11,value = 336}
        };

        public System.Collections.Generic.Dictionary<System.Int32,Testdata> dataMap = null;
        public System.Collections.Generic.Dictionary<System.Int32,Testdata> DataMap 
        {
            get
            {
                if(dataMap==null) dataMap = new System.Collections.Generic.Dictionary<System.Int32,Testdata>();
                foreach(var item in Data)
                {
                    DataMap.Add(item.id,item);
                }
                return dataMap;
            }

        }
        public TestdataModel(){}
    }
}




```

# 拓展类型规则
![image](https://github.com/yueh0607/Excel2CSharp/assets/102401735/adb264d0-8c83-4652-bb36-57dab67cf97f)
简单的很，只需要返回字符串，或者抛异常，然后添加策略
![image](https://github.com/yueh0607/Excel2CSharp/assets/102401735/33817ecf-3c08-4d5c-a3aa-ca8f4fe0f2fe)

#拓展分析规则
同理
![image](https://github.com/yueh0607/Excel2CSharp/assets/102401735/7c17224b-be62-4024-8efa-a2935ac3c382)
![image](https://github.com/yueh0607/Excel2CSharp/assets/102401735/72be269b-15ff-43c9-86b9-cc621b1ee7c4)

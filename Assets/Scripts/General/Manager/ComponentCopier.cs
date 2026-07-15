using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public static class ComponentCopier
{

    public static T CopyComponent<T>(GameObject original, GameObject destination) where T : class
    {
        // 通过反射来查找实现了接口的 Component
        var originalComponents = original.GetComponents<Component>().Where(comp => comp is T);

        Component copiedComponent = null;

        foreach (var originalComponent in originalComponents)
        {
            // 复制找到的第一个符合条件的组件
            System.Type type = originalComponent.GetType();
            copiedComponent = destination.AddComponent(type);

            // 复制属性
            foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(copiedComponent, propertyInfo.GetValue(originalComponent, null), null);
                }
            }

            // 复制字段
            foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                fieldInfo.SetValue(copiedComponent, fieldInfo.GetValue(originalComponent));
            }

            break; // 假设我们只需要复制一种实现了该接口的组件
        }

        return copiedComponent as T;
    }

}

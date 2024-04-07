using System;

public static class ArrayExtensions
{
    public static void For<T>(this T[] array, Action<T, int> action) 
    {
        for(int i = 0; i < array.Length; i++)
            action?.Invoke(array[i], i);
    }
}
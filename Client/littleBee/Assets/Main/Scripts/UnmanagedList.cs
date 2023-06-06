using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public unsafe class UnmanagedList<T> : IList<T>, IDisposable where T : unmanaged
{
    // public getters
    public bool IsReadOnly => false;
    public int DataSizeInBytes => dataSizeInElements_ * elementSize_;
    public int UsedSizeInBytes => Count * elementSize_;

    // public getter with setter
    public T* Data { get; private set; }
    public IntPtr DataIntPtr => (IntPtr)Data;
    public int Count { get; private set; } = 0;

    // private fields
    private int dataSizeInElements_;

    // readonly fields
#if DEBUG
    readonly
#endif
        private float overflowMult_;
#if DEBUG
    readonly
#endif
        private int elementSize_;

    public UnmanagedList(int startingBufferSize = 8, float overflowMult = 1.5f)
    {
        if ((int)(startingBufferSize * overflowMult) <= startingBufferSize)
            throw new ArithmeticException("Overflow multiplier doesn't increase size. Try increasing it.");

        overflowMult_ = overflowMult;
        elementSize_ = sizeof(T);
        dataSizeInElements_ = startingBufferSize;
        Data = (T*)Marshal.AllocHGlobal(DataSizeInBytes);
    }

    public void Dispose()
    {
        Marshal.FreeHGlobal((IntPtr)Data);
        GC.SuppressFinalize(this);
    }

    ~UnmanagedList()
    {
        Marshal.FreeHGlobal((IntPtr)Data);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item)
    {
        if (Count + 1 > dataSizeInElements_)
            GrowMemoryBlock(GetNextBlockSize());

        Data[Count] = item;
        Count++;
    }

    public void AddRange(UnmanagedList<T> unmanagedCollection)
    {
        AssureSize(Count + unmanagedCollection.Count);

        for (int i = 0; i < unmanagedCollection.Count; i++)
            Data[Count + i] = unmanagedCollection.Data[i];

        Count += unmanagedCollection.Count;
    }

    public void AddRange(IList<T> collection)
    {
        AssureSize(Count + collection.Count);

        for (int i = 0; i < collection.Count; i++)
            Data[Count + i] = collection[i];

        Count += collection.Count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AssureSize(int sizeInElements)
    {
        if (sizeInElements > dataSizeInElements_)
        {
            var nextAccomodatingSize = GetNextBlockSize();
            while (nextAccomodatingSize < sizeInElements)
                nextAccomodatingSize = GetNextBlockSize();
            GrowMemoryBlock(nextAccomodatingSize);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetNextBlockSize()
    {
        return (int)(dataSizeInElements_ * overflowMult_);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GrowMemoryBlock(int newElementCount)
    {
        var newDataSize = elementSize_ * newElementCount;
        var newData = (T*)Marshal.AllocHGlobal(newDataSize);
        Buffer.MemoryCopy(Data, newData, DataSizeInBytes, DataSizeInBytes);
        Marshal.FreeHGlobal((IntPtr)Data);
        dataSizeInElements_ = newElementCount;
        Data = newData;
    }

    public void Clear()
    {
        Count = 0;
    }

    public int IndexOf(T item)
    {
        for (int i = 0; i < Count; i++)
            if (Data[i].Equals(item))
                return i;
        return -1;
    }

    public void Insert(int index, T item)
    {
        AssureSize(Count + 1);
        var trailingSize = (Count - index) * elementSize_;
        Buffer.MemoryCopy(&Data[index], &Data[index + 1], trailingSize, trailingSize);
        Data[index] = item;
        Count++;
    }

    /// <summary>This is slow. Use RemoveAtFast() if you don't need stable order</summary>
    public void RemoveAt(int index)
    {
        var trailingSize = (Count - index) * elementSize_;
        Buffer.MemoryCopy(&Data[index + 1], &Data[index], trailingSize, trailingSize);
        Count--;
    }

    /// <summary>Removes element at index without preserving order (very fast)</summary>
    public void RemoveAtFast(int index)
    {
        Buffer.MemoryCopy(&Data[Count - 1], &Data[index], elementSize_, elementSize_);
        Count--;
    }

    public T this[int index]
    {
        set => Data[index] = value;
        get => Data[index];
    }

    public ref T GetReferenceElement(int index)
    {
        return ref Data[index];
    }


    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
            yield return this[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public unsafe void FastForeach(Action<T> loopAction)
    {
        for (int i = 0; i < Count; i++)
            loopAction(Data[i]);
    }

    public bool Contains(T item)
    {
        for (int i = 0; i < Count; i++)
            if (Data[i].Equals(item))
                return true;
        return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (arrayIndex + Count > array.Length)
            throw new IndexOutOfRangeException("Array to copy to doesn't have enough space");

        fixed (T* manArrDataPtr = &array[arrayIndex])
        {
            Buffer.MemoryCopy(Data, manArrDataPtr, UsedSizeInBytes, UsedSizeInBytes);
        }
    }

    public void CopyTo(IntPtr memAddr)
    {
        Buffer.MemoryCopy(Data, (void*)memAddr, UsedSizeInBytes, UsedSizeInBytes);
    }

    public bool Remove(T item)
    {
        var index = IndexOf(item);
        if (index < 0) return false;
        RemoveAt(index); return true;
    }
}
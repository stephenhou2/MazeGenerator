/// <summary>
///  简单队列
/// </summary>
/// <typeparam name="T"></typeparam>
public class SimpleStack<T>
{
    private SimpleStackNode<T> Head;
    private SimpleStackNode<T> Tail;

    public SimpleStackNode<T> Current;

    public T GetLast()
    {
        if(Tail != null)
        {
            return Tail.GetObj();
        }
        return default(T);
    }

    public bool MoveNext()
    {
        if (Current == null)
            return false;

        var last = Current.GetLast();
        if (last == null)
            return false;

        Current = last;
        return true;
    }

    public void Reset()
    {
        Current = Tail;
    }
    public int Count { get; private set; } = 0;

    public void Push(T obj)
    {
        SimpleStackNode<T> node = new SimpleStackNode<T>(obj);
        // 队列内没有元素
        if (Tail == null)
        {
            Head = node;
            Tail = Head;
            Count = 1;
            return;
        }

        // 队列内有元素，加到队尾
        Tail.SetNext(node);
        node.SetLast(Tail);
        Tail = node;
        Count++;
    }

    public bool HasItem()
    {
        return Head != null;
    }

    public T Pop()
    {
        // 队列内没有元素
        if (Count == 0)
        {
            return default(T);
        }

        // 队列内只有一个元素
        T obj = Tail.GetObj();
        if (Count == 1)
        {
            Head = null;
            Tail = null;
            Count = 0;
            return obj;
        }

        // 队列内有多个元素
        var last = Tail.GetLast();
        if (last != null)
        {
            last.SetNext(null);
        }
        Tail = last;
        Count--;
        return obj;
    }
}

public class SimpleStackNode<T>
{
    private SimpleStackNode<T> mLast;
    private SimpleStackNode<T> mNext;

    private T mObj;

    public SimpleStackNode(T obj)
    {
        mObj = obj;
    }

    public T GetObj()
    {
        return mObj;
    }

    public void SetLast(SimpleStackNode<T> node)
    {
        mLast = node;
    }

    public SimpleStackNode<T> GetLast()
    {
        return mLast;
    }

    public void SetNext(SimpleStackNode<T> node)
    {
        mNext = node;
    }

    public SimpleStackNode<T> GetNext()
    {
        return mNext;
    }
}
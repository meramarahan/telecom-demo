namespace Telecom.Collections;

public class CustomDictionary<TKey, TValue>
{
    private readonly LinkedList<KeyValuePair<TKey, TValue>>[] _buckets;
    private readonly int _capacity;

    public CustomDictionary(int capacity = 100)
    {
        _capacity = capacity;
        _buckets = new LinkedList<KeyValuePair<TKey, TValue>>[capacity];
    }

    public IEnumerable<TValue> Values
    {
        get
        {
            return GetAllValues();
        }
    }

    private int GetBucketIndex(TKey key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }
        return Math.Abs(key.GetHashCode()) % _capacity;
    }

    public void Add(TKey key, TValue value)
    {
        int index = GetBucketIndex(key);

        if (_buckets[index] == null)
        {
            _buckets[index] = new LinkedList<KeyValuePair<TKey, TValue>>();
        }

        foreach (var pair in _buckets[index])
        {
            if (pair.Key != null && pair.Key.Equals(key))
            {
                throw new ArgumentException($"Key '{key}' already exists in the dictionary.");
            }
        }

        _buckets[index].AddLast(new KeyValuePair<TKey, TValue>(key, value));
    }

    public TValue Get(TKey key)
    {
        int index = GetBucketIndex(key);

        if (_buckets[index] != null)
        {
            foreach (var pair in _buckets[index])
            {
                if (pair.Key != null && pair.Key.Equals(key))
                {
                    return pair.Value;
                }
            }
        }

        throw new KeyNotFoundException($"Key '{key}' was not found.");
    }

    public void Remove(TKey key)
    {
        int index = GetBucketIndex(key);

        if (_buckets[index] != null)
        {
            var current = _buckets[index].First;
            while (current != null)
            {
                if (current.Value.Key != null && current.Value.Key.Equals(key))
                {
                    _buckets[index].Remove(current);
                    return;
                }
                current = current.Next;
            }
        }

        throw new KeyNotFoundException($"Key '{key}' was not found.");
    }

    public IEnumerable<TValue> GetAllValues()
    {
        var values = new List<TValue>();
        
        foreach (var bucket in _buckets)
        {
            if (bucket != null)
            {
                foreach (var pair in bucket)
                {
                    values.Add(pair.Value);
                }
            }
        }
        
        return values;
    }

    public TValue this[TKey key]
    {
        get
        {
            return Get(key);
        }
        set
        {
            throw new NotImplementedException("Set operation is not supported yet.");
        }
    }
}
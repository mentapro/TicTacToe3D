using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class PoolExceededFixedSizeException : Exception
    {
        public PoolExceededFixedSizeException(string errorMessage)
            : base(errorMessage)
        {
        }
    }

    [Serializable]
    public class MemoryPoolSettings
    {
        public int InitialSize;
        public PoolExpandMethods ExpandMethod;
    }

    public abstract class MemoryPoolBase<TContract> : IValidatable, IMemoryPool
    {
        readonly HashSet<TContract> _activeItems = new HashSet<TContract>();

        Stack<TContract> _inactiveItems;
        IFactory<TContract> _factory;
        MemoryPoolSettings _settings;

        [Inject]
        void Construct(
            IFactory<TContract> factory,
            DiContainer container,
            MemoryPoolSettings settings)
        {
            _settings = settings;
            _factory = factory;

            _inactiveItems = new Stack<TContract>(settings.InitialSize);

            if (!container.IsValidating)
            {
                for (int i = 0; i < settings.InitialSize; i++)
                {
                    _inactiveItems.Push(AllocNew());
                }
            }
        }

        public IEnumerable<TContract> InactiveItems
        {
            get { return _inactiveItems; }
        }

        public int NumTotal
        {
            get { return NumInactive + NumActive; }
        }

        public int NumInactive
        {
            get { return _inactiveItems.Count; }
        }

        public int NumActive
        {
            get { return _activeItems.Count; }
        }

        public Type ItemType
        {
            get { return typeof(TContract); }
        }

        public void DespawnAll()
        {
            foreach (var item in _activeItems.ToList())
            {
                Despawn(item);
            }
        }

        public void Despawn(TContract item)
        {
            Assert.That(!_inactiveItems.Contains(item),
            "Tried to return an item to pool {0} twice", this.GetType());

            bool removed = _activeItems.Remove(item);

            Assert.That(removed,
                "Tried to return an item to the pool that was not originally created in pool");

            _inactiveItems.Push(item);

            OnDespawned(item);
        }

        TContract AllocNew()
        {
            try
            {
                var item = _factory.Create();
                Assert.IsNotNull(item, "Factory '{0}' returned null value when creating via {1}!", _factory.GetType(), this.GetType());
                OnCreated(item);
                return item;
            }
            catch (Exception e)
            {
                throw new ZenjectException(
                    "Error during construction of type '{0}' via {1}.Create method!".Fmt(
                        typeof(TContract), this.GetType().Name()), e);
            }
        }

        void IValidatable.Validate()
        {
            try
            {
                _factory.Create();
            }
            catch (Exception e)
            {
                throw new ZenjectException(
                    "Validation for factory '{0}' failed".Fmt(this.GetType()), e);
            }
        }

        protected TContract GetInternal()
        {
            TContract item;

            if (_inactiveItems.IsEmpty())
            {
                ExpandPool();
                Assert.That(!_inactiveItems.IsEmpty());
            }

            item = _inactiveItems.Pop();

            bool added = _activeItems.Add(item);
            Assert.That(added);

            OnSpawned(item);
            return item;
        }

        void ExpandPool()
        {
            switch (_settings.ExpandMethod)
            {
                case PoolExpandMethods.Fixed:
                {
                    throw new PoolExceededFixedSizeException(
                        "Pool factory '{0}' exceeded its max size of '{1}'!"
                        .Fmt(this.GetType(), NumTotal));
                }
                case PoolExpandMethods.OneAtATime:
                {
                    _inactiveItems.Push(AllocNew());
                    break;
                }
                case PoolExpandMethods.Double:
                {
                    if (NumTotal == 0)
                    {
                        _inactiveItems.Push(AllocNew());
                    }
                    else
                    {
                        var oldSize = NumTotal;

                        for (int i = 0; i < oldSize; i++)
                        {
                            _inactiveItems.Push(AllocNew());
                        }
                    }
                    break;
                }
                default:
                {
                    throw Assert.CreateException();
                }
            }
        }

        protected virtual void OnDespawned(TContract item)
        {
            // Optional
        }

        protected virtual void OnSpawned(TContract item)
        {
            // Optional
        }

        protected virtual void OnCreated(TContract item)
        {
            // Optional
        }
    }
}

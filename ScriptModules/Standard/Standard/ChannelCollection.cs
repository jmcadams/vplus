namespace Standard
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public class ChannelCollection : IList<VixenChannel>, ICollection<VixenChannel>, IChannelEnumerable, IEnumerable<VixenChannel>, IEnumerable
    {
        private List<VixenChannel> m_list = new List<VixenChannel>();

        public void Add(IChannelEnumerable channels)
        {
            foreach (VixenChannel channel in channels)
            {
                this.Add(channel);
            }
        }

        public void Add(VixenChannel channel)
        {
            if (!this.m_list.Contains(channel))
            {
                this.m_list.Add(channel);
            }
        }

        public void Add(params IChannelEnumerable[] channels)
        {
            foreach (IChannelEnumerable enumerable in channels)
            {
                this.Add(enumerable);
            }
        }

        public ChannelCollection ChannelRange(IChannelEnumerable startChannel, IChannelEnumerable endChannel)
        {
            ChannelCollection channels = new ChannelCollection();
            if (this.m_list.Contains(startChannel[0]) && this.m_list.Contains(endChannel[0]))
            {
                int index = this.m_list.IndexOf(startChannel[0]);
                int num2 = this.m_list.IndexOf(endChannel[0]);
                if (num2 < index)
                {
                    int num3 = index;
                    index = num2;
                    num2 = num3;
                }
                while (index <= num2)
                {
                    channels.Add(this.m_list[index++]);
                }
            }
            return channels;
        }

        public ChannelCollection ChannelRange(IChannelEnumerable startChannel, int count)
        {
            ChannelCollection channels = new ChannelCollection();
            if (this.m_list.Contains(startChannel[0]))
            {
                int index = this.m_list.IndexOf(startChannel[0]);
                int num2 = Math.Min(index + count, this.m_list.Count);
                while (index < num2)
                {
                    channels.Add(this.m_list[index++]);
                }
            }
            return channels;
        }

        public ChannelCollection ChannelRange(int start, int count)
        {
            ChannelCollection channels = new ChannelCollection();
            if (start < this.m_list.Count)
            {
                int num = Math.Min(start + count, this.m_list.Count);
                while (start < num)
                {
                    channels.Add(this.m_list[start++]);
                }
            }
            return channels;
        }

        public void Clear()
        {
            this.m_list.Clear();
        }

        public bool Contains(VixenChannel item)
        {
            return this.m_list.Contains(item);
        }

        public void CopyTo(VixenChannel[] array, int arrayIndex)
        {
            this.m_list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<VixenChannel> GetEnumerator()
        {
            return this.m_list.GetEnumerator();
        }

        public int IndexOf(VixenChannel item)
        {
            return this.m_list.IndexOf(item);
        }

        public void Insert(int index, VixenChannel item)
        {
            this.m_list.Insert(index, item);
        }

        public static implicit operator VixenChannel(ChannelCollection channels)
        {
            return channels[0];
        }

        public bool Remove(VixenChannel item)
        {
            return this.m_list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.m_list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.m_list.GetEnumerator();
        }

        public override string ToString()
        {
            if (this.m_list.Count == 1)
            {
                return this.m_list[0].ToString();
            }
            return base.ToString();
        }

        public int Count
        {
            get
            {
                return this.m_list.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public VixenChannel this[int index]
        {
            get
            {
                return this.m_list[index];
            }
            set
            {
                this.m_list[index] = value;
            }
        }
    }
}


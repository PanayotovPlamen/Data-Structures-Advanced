using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.Discord
{
    public class Discord : IDiscord
    {
        private Dictionary<string, Message> messages = new Dictionary<string, Message>();

        private Dictionary<string, int> channelCount = new Dictionary<string, int>();

        public int Count => this.messages.Count;

        public bool Contains(Message message)
        {
            return this.messages.ContainsKey(message.Id);
        }

        public void DeleteMessage(string messageId)
        {
            if (!this.messages.ContainsKey(messageId))
            {
                throw new ArgumentException();
            }

            this.messages.Remove(messageId);
        }

        public IEnumerable<Message> GetAllMessagesOrderedByCountOfReactionsThenByTimestampThenByLengthOfContent()
        {
            return this.messages.Values.OrderByDescending(x => x.Reactions.Count).ThenBy(x => x.Timestamp).ThenBy(x => x.Content.Length);
        }

        public IEnumerable<Message> GetChannelMessages(string channel)
        {
            List<Message> messagesByChannel = this.messages.Values.Where(x => x.Channel == channel).ToList();

            if (messagesByChannel.Count == 0)
            {
                throw new ArgumentException();
            }

            return messagesByChannel;
        }

        public Message GetMessage(string messageId)
        {
            if (!this.messages.ContainsKey(messageId))
            {
                throw new ArgumentException();
            }

            return this.messages[messageId];
        }

        public IEnumerable<Message> GetMessageInTimeRange(int lowerBound, int upperBound)
        {
            List<Message> result = this.messages.Values.Where(x => x.Timestamp >= lowerBound && x.Timestamp <= upperBound).ToList();

            foreach (var item in result)
            {
                if (!channelCount.ContainsKey(item.Channel))
                {
                    channelCount.Add(item.Channel, 1);
                }
                else
                {
                    channelCount[item.Channel]++;
                }
            }

            foreach (var item in result)
            {
                if (channelCount.ContainsKey(item.Channel))
                {
                    item.ChannelCount = channelCount[item.Channel];
                }
            }

            return result.OrderByDescending(x => x.ChannelCount);

        }

        public IEnumerable<Message> GetMessagesByReactions(List<string> reactions)
        {
            List<Message> result = new List<Message>();

            foreach (var msg in this.messages.Values)
            {
                bool containsAllReactions = false;

                foreach (var reaction in reactions)
                {
                    if (msg.Reactions.Contains(reaction))
                    {
                        containsAllReactions = true;
                    }
                    else
                    {
                        containsAllReactions = false;
                        break;
                    }
                }

                if (containsAllReactions)
                {
                    result.Add(msg);
                }
            }


            return result.OrderByDescending(x => x.Reactions.Count).ThenBy(x => x.Timestamp);
        }

        public IEnumerable<Message> GetTop3MostReactedMessages()
        {
            return this.messages.Values.OrderByDescending(x => x.Reactions.Count).Take(3);
        }

        public void ReactToMessage(string messageId, string reaction)
        {
            if (!this.messages.ContainsKey(messageId))
            {
                throw new ArgumentException();
            }

            this.messages[messageId].Reactions.Add(reaction);
        }

        public void SendMessage(Message message)
        {           
            this.messages.Add(message.Id, message);
        }
    }
}

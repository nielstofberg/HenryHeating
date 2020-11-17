using Heating.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heating.Core
{
    public class RelayCollection
    {
        private static int _relayIds = 1;

        private List<Relay> relays = new List<Relay>();

        /// <summary>
        /// Constructor
        /// </summary>
        public RelayCollection()
        {
            using (var db = new DataContext())
            {
                db.Database.EnsureCreated();
            }
        }

        /// <summary>
        /// Retreved relays stored in the database.
        /// </summary>
        public void Load()
        {
            relays.Clear();
            using (var db = new DataContext())
            {
                try
                {
                    relays = db.Relays.ToList();
                }
                catch (Exception ex)
                { }
            }
            foreach (var r in relays)
            {
                r.Subscribe();
            }
        }

        public Relay[] ToArray()
        {
            return relays.ToArray();
        }

        /// <summary>
        /// Add a relay to the collection.
        /// Creates a new relay with the given name and topic.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        public async Task<Relay> AddRelayAsync(string name, string topic)
        {
            Relay newRelay = new Relay(topic);
            newRelay.Name = name;
            newRelay.ID = _relayIds++;
            await AddRelayAsync(newRelay);
            return newRelay;
        }

        public async Task<bool> AddRelayAsync(Relay relay)
        {
            //if (relay.ID == 0) throw new Exception("No valid Relay ID");
            if (relay.Name.Length == 0) throw new Exception("No valid Relay Name");
            if (relays.Where(r => r.ID == relay.ID).Count() > 0)
            {
                return false;
            }
            relays.Add(relay);

            using (var db = new DataContext())
            {
                try
                {
                    db.Relays.Add(relay);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                }
            }
            return true;
        }

        public Relay GetRelay(int id)
        {
            var ret = relays.Find(r => r.ID == id);
            return ret;
        }

        public Relay GetRelay(string name)
        {
            var ret = relays.Find(r => r.Name == name);
            return ret;
        }

        public async Task UpdateRelayAsync(Relay relay)
        {
            var oldRelay = relays.First(r => r.ID == relay.ID);

            if (oldRelay != null)
            {
                using (var db = new DataContext())
                {
                    db.Relays.Update(relay);
                    await db.SaveChangesAsync();
                }
                Load();
            }
        }

        public async Task DeleteRelayAsync(int id)
        {
            var oldRelay = relays.First(r => r.ID == id);
            if (oldRelay != null)
            {
                using (var db = new DataContext())
                {
                    db.Relays.Remove(oldRelay);
                    await db.SaveChangesAsync();
                }
                Load();
            }
        }
    }
}

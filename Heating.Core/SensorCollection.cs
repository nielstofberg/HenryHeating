using Heating.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heating.Core
{
    public class SensorCollection
    {
        private List<Sensor> sensors = new List<Sensor>();

        /// <summary>
        /// Construcor
        /// </summary>
        public SensorCollection()
        {
            using (var db = new DataContext())
            {
                db.Database.EnsureCreated();
            }
        }

        public void Load()
        {
            sensors.Clear();
            using (var db = new DataContext())
            {
                try
                {
                    sensors = db.Sensors.ToList();
                }
                catch (Exception)
                { }
            }
            foreach (var s in sensors)
            {
                s.Subscribe();
            }
        }

        public Sensor[] ToArray()
        {
            return sensors.ToArray();
        }

        public async Task<Sensor> AddSensorAsync(string name, string topic)
        {
            Sensor newSensor = new Sensor(topic);
            newSensor.Name = name;
            newSensor.ID = -1;
            await AddSensorAsync(newSensor);
            return newSensor;
        }

        public async Task<bool> AddSensorAsync(Sensor sensor)
        {
            if (sensor.Name.Length==0) throw new Exception("No valid Sensor Name");
            if (sensors.Where(r => r.ID == sensor.ID).Count() > 0)
            {
                return false;
            }
            sensors.Add(sensor);
            using (var db = new DataContext())
            {
                try
                {
                    db.Sensors.Add(sensor);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                }
            }
            return true;
        }

        public Sensor GetSensor(int id)
        {
            var ret = sensors.Find(s=> s.ID == id);
            return ret;
        }

        public Sensor GetSensor(string name)
        {
            var ret = sensors.Find(s => s.Name == name);
            return ret;
        }

        public async Task UpdateSensorAsync(Sensor sensor)
        {
            var oldSensor = sensors.First(s => s.ID == sensor.ID);
            if (oldSensor != null)
            {
                using (var db = new DataContext())
                {
                    db.Sensors.Update(sensor);
                    await db.SaveChangesAsync();
                }
                Load();
            }
        }

        public async Task DeleteSensorAsync(int id)
        {
            var oldSensor = sensors.First(s => s.ID == id);
            if (oldSensor != null)
            {
                using (var db = new DataContext())
                {
                    db.Sensors.Remove(oldSensor);
                    await db.SaveChangesAsync();
                }
                Load();
            }
        }
    }
}

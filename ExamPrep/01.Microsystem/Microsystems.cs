namespace _01.Microsystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Microsystems : IMicrosystem
    {
        private Dictionary<int, Computer> pcById = new Dictionary<int, Computer>();

        public void CreateComputer(Computer computer)
        {
            if (pcById.ContainsKey(computer.Number))
            {
                throw new ArgumentException();
            }

            this.pcById.Add(computer.Number, computer);
        }

        public bool Contains(int number)
        {
            return this.pcById.ContainsKey(number);
        }

        public int Count()
        {
           return this.pcById.Count;
        }

        public Computer GetComputer(int number)
        {
            if (!this.pcById.ContainsKey(number))
            {
                throw new ArgumentException();
            }

            return this.pcById[number];
        }

        public void Remove(int number)
        {
            if (!this.pcById.ContainsKey(number))
            {
                throw new ArgumentException();
            }

            this.pcById.Remove(number);
        }

        public void RemoveWithBrand(Brand brand)
        {
           var computers = this.pcById.Values.Where(x => x.Brand == brand).ToList();

            if (computers.Count > 0)
            {
                foreach (var item in computers)
                {
                    this.pcById.Remove(item.Number);
                }
            }
            else
            {
                throw new ArgumentException();
            }

            
        }

        public void UpgradeRam(int ram, int number)
        {
            if (!this.pcById.ContainsKey(number))
            {
                throw new ArgumentException();
            }

            this.pcById[number].RAM = ram >= this.pcById[number].RAM ? ram : this.pcById[number].RAM;

        }

        public IEnumerable<Computer> GetAllFromBrand(Brand brand)
        {
            return this.pcById.Values.Where(x => x.Brand == brand).OrderByDescending(x => x.Price);
        }

        public IEnumerable<Computer> GetAllWithScreenSize(double screenSize)
        {
            return this.pcById.Values.Where(pc => pc.ScreenSize == screenSize).OrderByDescending(pc => pc.Number); ;
        }

        public IEnumerable<Computer> GetAllWithColor(string color)
        {
            return this.pcById.Values.Where(pc => pc.Color == color).OrderByDescending(pc => pc.Price);
        }

        public IEnumerable<Computer> GetInRangePrice(double minPrice, double maxPrice)
        {
            return this.pcById.Values.Where(pc => pc.Price >= minPrice && pc.Price <= maxPrice).OrderByDescending(pc => pc.Price);
        }
    }
}

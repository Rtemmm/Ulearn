using System;

namespace HotelAccounting
{
    public class AccountingModel : ModelBase
    {
        private double price;
        public double Price 
        { 
            get { return price; } 
            set 
            { 
                if (value < 0) throw new ArgumentException(); 
                price = value;
                RecalculateTotal();

                Notify(nameof(Price));
                Notify(nameof(Total));
            } 
        }

        private int nightsCount;
        public int NightsCount 
        { 
            get { return nightsCount; } 
            set 
            { 
                if (value <= 0) throw new ArgumentException(); 
                nightsCount = value;
                RecalculateTotal();

                Notify(nameof(NightsCount));
                Notify(nameof(Total));
            } 
        }

        private double discount;
        public double Discount
        { 
            get { return discount; }
            set
            {
                if (value > 100) throw new ArgumentException();

                discount = value;
                RecalculateTotal();

                Notify(nameof(Discount));
                Notify(nameof(Total));
            }
        }

        private double total;
        public double Total 
        { 
            get { return total; } 
            set 
            { 
                if (value < 0) throw new ArgumentException();

                total = value;
                discount = 100 * (1 - total / (price * nightsCount));

                Notify(nameof(Total));
                Notify(nameof(Discount));
            } 
        }

        private void RecalculateTotal() => total = 100 * (1 - total / (price * nightsCount));
    }
}
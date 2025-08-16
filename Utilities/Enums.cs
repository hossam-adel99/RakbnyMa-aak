using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RakbnyMa_aak.Utilities
{
    public class Enums
    {
        public enum UserType
        {
            Driver,
            User,
            Admin
        }

        public enum Gender
        {
            [Display(Name = "ذكر")]
            Male,

            [Display(Name = "أنثى")]
            Female
        }


        public enum CarType
        {
            [Display(Name = "Sedan")]
            Sedan,

            [Display(Name = "SUV")]
            SUV,

            [Display(Name = "Van")]
            Van,

            [Display(Name = "Bus")]
            Bus,

            [Display(Name = "Taxi")]
            Taxi,

            [Display(Name = "Motorcycle")]
            Motorcycle,

            [Display(Name = "Pickup Truck")]
            PickupTruck,

            [Display(Name = "Minibus")]
            Minibus
        }



        public enum CarColor
        {
            [Display(Name = "Black")]
            Black,

            [Display(Name = "White")]
            White,

            [Display(Name = "Red")]
            Red,

            [Display(Name = "Blue")]
            Blue,

            [Display(Name = "Green")]
            Green,

            [Display(Name = "Yellow")]
            Yellow,

            [Display(Name = "Silver")]
            Silver,

            [Display(Name = "Gray")]
            Gray,

            [Display(Name = "Brown")]
            Brown,

            [Display(Name = "Orange")]
            Orange,

            [Display(Name = "Gold")]
            Gold,

            [Display(Name = "Purple")]
            Purple,

            [Display(Name = "Pink")]
            Pink,

            [Display(Name = "Beige")]
            Beige,

            [Display(Name = "Maroon")]
            Maroon,

            [Display(Name = "Navy")]
            Navy
        }



        public enum TripStatus
        {
            [Display(Name = "Scheduled")]
            Scheduled,

            [Display(Name = "Ongoing")]
            Ongoing,

            [Display(Name = "Completed")]
            Completed,

            [Display(Name = "Cancelled")]
            Cancelled
        }



        public enum RequestStatus
        {
            [Display(Name = "Pending")]
            Pending,

            [Display(Name = "Confirmed")]
            Confirmed,

            [Display(Name = "Cancelled")]
            Cancelled,

            [Display(Name = "Rejected")]
            Rejected
        }



        public enum PaymentMethod
        {
            [Display(Name = "Cash")]
            Cash,

            [Display(Name = "Vodafone Cash")]
            VodafoneCash,

            [Display(Name = "Credit Card")]
            CreditCard,

            [Display(Name = "Wallet")]
            Wallet
        }



        public enum PaymentStatus
        {
            [Display(Name = "Pending")]
            Pending,

            [Display(Name = "Completed")]
            Completed,

            [Display(Name = "Failed")]
            Failed,

            [Display(Name = "Refunded")]
            Refunded
        }



        public enum PaymentType
        {
            BookingPayment,
            WalletTopUp,
            DriverPayout
        }
        public enum TransactionType
        {
            [Display(Name = "Credit")]
            Credit,

            [Display(Name = "Debit")]
            Debit
        }



        public enum TransactionStatus
        {
            [Display(Name = "Pending")]
            Pending,

            [Display(Name = "Completed")]
            Completed,

            [Display(Name = "Failed")]
            Failed
        }



        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum NotificationType
        {
            [Display(Name = "تسجيل السائق")]
            DriverRegistration,

            [Display(Name = "تم حجز الرحلة")]
            TripBooked,

            [Display(Name = "تمت الموافقة على الحجز")]
            BookingApproved,

            [Display(Name = "تم إلغاء الرحلة")]
            TripCancelled,

            [Display(Name = "الدفع")]
            Payment,

            [Display(Name = "رسالة محادثة")]
            ChatMessage,

            [Display(Name = "إشعار مخصص")]
            Custom
        }


    }
}

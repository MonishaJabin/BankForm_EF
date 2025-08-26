namespace BankAccount_Form.Entity
{
    public class BankAccount
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNo { get; set; }

        public string Address { get; set; }

        public string Account { get; set; }

        public bool DebitCard { get; set; }

        public bool InternetBanking { get; set; }

        public bool MobileAlert { get; set; }

        public string Selectbranch { get; set; }
    }
}

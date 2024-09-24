namespace GateKeeperV1.ViewModels
{
    public class CheckoutViewModel
    {
        public RegistCompanyViewModel company { get; set; }
        public DateTime validUntil { get; set; }

        public CheckoutViewModel(RegistCompanyViewModel companyViewModel)
        {
            company = companyViewModel;
            validUntil = DateTime.Now;
        }
    }
}

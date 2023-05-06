namespace AFRICOMDMSWEB.Models.ViewModel
{
    public class RequestViewModel
    {

        public IEnumerable<Request> Rquestrecieved { get; set; }
        public IEnumerable<Request> RquestSsended { get; set; }

        public IEnumerable<RequestAction> ActionUserTaken { get; set; }
         

        public IEnumerable<Transition> RequestsIRecivedFromPass { get; set; }
        public IEnumerable<Transition> RequstedUserPass { get; set; }
        public IEnumerable<RequestAction> ActionPassedForRequiest { get; set; }
        public Request Requests { get; set; }
        public IEnumerable<Transition> RequestIpassToHigherLevel { get; set; }
        public IEnumerable<Transition> RecivedRequiestPass { get; set; }

        public IEnumerable<Transition> ActionPassedToAnotherForMe { get; set; }
        public IEnumerable<Request> RequestsForSpecificRquest { get; set; }
        public IEnumerable<Request> RequestsPhase { get; set; }
        public IEnumerable<ReqComment> Comments { get; set; }
        public RequestFile RequestFiles { get; set; }
        public IEnumerable<RequestFile> requestFile { get; set; }

        public IEnumerable<ReqAnswerFiles> requestAnswerFiles { get; set; } 

        public IEnumerable<RequestAction> SpecificRequestAnswers { get; set; }


    }
}

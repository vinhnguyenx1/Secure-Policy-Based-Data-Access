public class Poort8Utilities
    {
        private readonly string _serviceProviders = "EU.EORI.TCNS";
        public Poort8Utilities()
        {

        }

        public PlaybookPolicy CreatePolicy(Poort8PolicyModel model)
        {
            //string resourceType = GetResourceTypeByModel(model);
            string resourceType = "";
            return new PlaybookPolicy
            {
                Email = $"{model.Email}",
                Organization = $"{model.Actor}",
                Username = $"{model.UserName}",
                Policy = new Poort8Policy(resourceType)
                {
                    Actor = $"{model.Actor}",
                    Issuer = $"{model.Issuer}",
                    Note = $"{model.Note}",
                    ContextRule = new PlaybookContextRule
                    {
                        Purpose = "Unattended delivery or return",
                        ResourceAttribute = "*",
                        ResourceIdentifier = $"{model.GenericKey}",
                        ServiceProvider = _serviceProviders,
                        NotBefore = int.Parse(DateTime.UtcNow.AddMinutes(1).ToEpoch()),
                        NotOnOrAfter = int.Parse(model.ToDate.AddDays(1).AddSeconds(-1).ToEpoch()),
                    }
                }
            };
        }

        public Poort8Delegation CreateDelegationEvidence(string genericKey, int genericType, string issuer, string actor)
        {
            return new Poort8Delegation();
        }

        public P8AccessPolicyModel ParseDelegationToken(string delegation_token)
        {
            return new P8AccessPolicyModel()
        }
    }

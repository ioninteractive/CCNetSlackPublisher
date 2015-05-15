using Exortech.NetReflector;
using SlackPublisher;
using ThoughtWorks.CruiseControl.Remote;

namespace ThoughtWorks.CruiseControl.Core.Publishers
{
    [ReflectorType("slackPublisher")]
    public class SlackPublisher : ITask
    {
        [ReflectorProperty("webhookUrl")]
        public string WebhookUrl { get; set; }

        public void Run(IIntegrationResult result)
        {
            if (string.IsNullOrEmpty(WebhookUrl))
                return;

            var payload = new Payload(FormatText(result));

            HttpPostHelper.HttpPost(WebhookUrl, payload.ToJson());
        }

        private string FormatText(IIntegrationResult result)
        {
            return string.Format("{0} <{1}|{2}> {3} #{4}",
                result.Succeeded ? ":verynice:" : ":poop:",
                result.ProjectUrl,
                result.ProjectName,
                result.Status == IntegrationStatus.Exception ? "Build Failed" : result.Status.ToString(),
                result.Label
            );
        }
    }
}

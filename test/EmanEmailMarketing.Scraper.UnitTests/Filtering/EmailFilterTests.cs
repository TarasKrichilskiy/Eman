using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EManEmailMarketing.Scraper.Email_Scraper.Filtering;

namespace EManEmailMarketing.Scraper.UnitTests.Filtering
{
    [TestClass]
    public class EmailFilterTests
    {
        [TestMethod]
        public void IsStandardTestValidEmails()
        {
            EmailFilter filter = new EmailFilter();
            List<string> emails = new List<string>();

            emails.Add("3dhome@zillowgroup.com");
            emails.Add("syndication@appfolio.com");
            emails.Add("511079@tenantturnermail.com");
            emails.Add("vybhav19@gmail.com");
            emails.Add("Arnold.Brier@Yardi.com");
            emails.Add("bmwhittaker@snopud.com");
            emails.Add("ftsantiago@yahoo.com");
            emails.Add("sfifehomes@gmail.com");
            emails.Add("eganwarmingcenter@svdp.us");

            foreach (string email in emails)
            {
                Assert.IsTrue(filter.IsStandard(email));
            }
        }

        [TestMethod()]
        public void isStandardTestInValidEmails()
        {
            EmailFilter filter = new EmailFilter();
            List<string> emails = new List<string>();

            emails.Add("25fed893115e4f6f97fe886acc5d128e@sentry.io");
            emails.Add("831126cb46b74583bf6f72c5061cba9d@sentry-viewer.wixpress.com");
            emails.Add("ofmmicommunications@ofm.wa.gov");
            emails.Add("tyler.verda@snoco.org");
            emails.Add("name@email.com");
            emails.Add("yourname@domain.com");
            emails.Add("rcc9la26d7534400a6a03514c34f9200@reply.craigslist.org");
            emails.Add("youremail@address.com");

            foreach (string email in emails)
            {
                Assert.IsFalse(filter.IsStandard(email));
            }

            Assert.IsFalse(filter.IsStandard("j3@W.Z"));
            Assert.IsFalse(filter.IsStandard("7@j.X"));
            Assert.IsFalse(filter.IsStandard("a@b.c"));
            Assert.IsFalse(filter.IsStandard("-------@redstone-group.com"));
            Assert.IsFalse(filter.IsStandard("aaaaaaaaaaaaa@gmail.com"));

        }
    }
}

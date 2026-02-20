using System;
using System.Collections.Generic;
using System.Text;

namespace cgspamd.core.Enums
{
    public enum FilterRulesType
    {
        whiteListSenderDomains,
        whiteListSenderAddresses,
        blackListSenderDomains,
        blackListSenderAddresses,
        excludedRecipients,
        prohibitedTextInBody,
        prohibitedRegExInBody
    }
}
